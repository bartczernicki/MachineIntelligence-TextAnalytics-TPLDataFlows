using Azure.AI.OpenAI;
using MachineIntelligenceTPLDataFlows.Classes;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Text;
using Newtonsoft.Json;
using SharpToken;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace MachineIntelligenceTPLDataFlows

{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "Machine Intelligence (Text Analytics) with TPL Data Flows & Vector Embeddings";

            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            IConfiguration configuration = configurationBuilder.AddUserSecrets<Program>().Build();
            string connectionString = configuration.GetSection("SQL")["SqlConnection"];
            var openAIAPIKey = configuration.GetSection("OpenAI")["APIKey"];
            var azureOpenAIAPIKey = configuration.GetSection("AzureOpenAI")["APIKey"];

            // START the timer
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var tokenLength = 0;

            // CONFIG 
            // Instantiate new ML.NET Context
            // Note: MlContext is thread-safe
            var mlContext = new MLContext(100);

            // var openAIEmbeddingsGeneration = new OpenAITextEmbeddingGeneration("text-embedding-ada-002", openAIAPIKey);

            // Azure OpenAI (Azure not OpenAI)
            //var openAIClient = new OpenAIClient(
            //    new Uri("https://openaiappliedai.openai.azure.com"), new Azure.AzureKeyCredential(azureOpenAIAPIKey));
            // OpenAI Client (not Azure OpenAI)
            var openAIClient = new OpenAIClient(openAIAPIKey);

            // GET Current Environment Folder
            // Note: This will have the JSON documents from the checked-in code and overwrite each time run
            // Note: Can be used for offline mode in-stead of downloading (not to get your IP blocked), or use VPN
            var currentEnrichmentFolder = System.IO.Path.Combine(Environment.CurrentDirectory, "EnrichedDocuments");
            System.IO.Directory.CreateDirectory(currentEnrichmentFolder);

            // SET language to English
            StopWordsRemovingEstimator.Language language = StopWordsRemovingEstimator.Language.English;


            // SET the max degree of parallelism
            // Note: Default is to use 75% of the workstation or server cores.
            // Note: If cores are hyperthreaded, adjust accordingly (i.e. multiply *2)
            var isHyperThreaded = false;
            var executionDataFlowOptions = new ExecutionDataflowBlockOptions();
            executionDataFlowOptions.MaxDegreeOfParallelism =
                // Use 75% of the cores, if hyper-threading multiply cores *2
                Convert.ToInt32(Math.Ceiling((Environment.ProcessorCount * 0.75) *
                (isHyperThreaded ? 2: 1)));
            executionDataFlowOptions.MaxMessagesPerTask = 1;

            // SET the Data Flow Block Options
            // This controls the data flow from the Producer level
            // Note: This example is making web requests directly to Project Gutenberg
            // Note: If this is set to high, you may receive errors. In production, you would ensure request throughput
            var dataFlowBlockOptions = new DataflowBlockOptions {
                EnsureOrdered = true, // Ensures order, this is purely optional but messages will flow in order
                BoundedCapacity = 10,
                MaxMessagesPerTask = 10 };

            // SET the data flow pipeline options
            // Note: OPTIONAL Set MaxMessages to the number of books to process
            // Note: For example, setting MaxMessages to 2 will run only two books through the pipeline
            // Note: Set MaxMessages to 1 to process in sequence
            var dataFlowLinkOptions = new DataflowLinkOptions {
                PropagateCompletion = true,
                MaxMessages = -1
            };

            // TPL: BufferBlock - Seeds the queue with selected Project Gutenberg Books
            async Task ProduceGutenbergBooks(BufferBlock<EnrichedDocument> queue,
                IEnumerable<ProjectGutenbergBook> projectGutenbergBooks)
            {
                var currentSQLScriptsFolder = System.IO.Path.Combine(Environment.CurrentDirectory, "SQL");
                var sqlScriptsFilePath = Path.Combine(currentSQLScriptsFolder, "ProjectGutenbergScripts.sql");
                var scriptText = File.ReadAllText(sqlScriptsFilePath);

                // Execute script to create database objects (tables, stored procedures)
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(string.Empty, connection))
                    {
                        command.CommandText = scriptText;
                        command.ExecuteNonQuery();
                    }

                    connection.Close();
                }

                foreach (var projectGutenbergBook in projectGutenbergBooks)
                {
                    // Add baseline information from the document book list
                    var enrichedDocument = new EnrichedDocument
                    {
                        BookTitle = projectGutenbergBook.BookTitle,
                        Author = projectGutenbergBook.Author,
                        Url = projectGutenbergBook.Url
                    };

                    await queue.SendAsync(enrichedDocument);
                }

                queue.Complete();
            }

            // TPL Block: Download the requested Gutenberg book resources as a text string
            var downloadBookText = new TransformBlock<EnrichedDocument, EnrichedDocument>(async enrichedDocument =>
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("Downloading: '{0}'", enrichedDocument.BookTitle);

                enrichedDocument.Text = await new HttpClient().GetStringAsync(enrichedDocument.Url);

                // Remove the beginning part of the Project Gutenberg i`nfo
                var indexOfBookBeginning = enrichedDocument.Text.IndexOf("GUTENBERG EBOOK") + "GUTENBERG EBOOK ".Length + enrichedDocument.BookTitle.Length;
                if (indexOfBookBeginning > 0)
                {
                    enrichedDocument.Text = enrichedDocument.Text.Substring(indexOfBookBeginning, enrichedDocument.Text.Length - indexOfBookBeginning);
                }

                if (indexOfBookBeginning < 0)
                {
                    indexOfBookBeginning = enrichedDocument.Text.IndexOf("GUTENBERG EBOOK") + "GUTENBERG EBOOK ".Length + enrichedDocument.BookTitle.Length;
                    if (indexOfBookBeginning > 0)
                    {
                        enrichedDocument.Text = enrichedDocument.Text.Substring(indexOfBookBeginning, enrichedDocument.Text.Length - indexOfBookBeginning);
                    }
                }

                // Remove the last part of the Project Gutenberg info to retrieve just the text
                var indexOfBookEnd = enrichedDocument.Text.IndexOf("*** END OF");
                if (indexOfBookEnd > 0)
                {
                    enrichedDocument.Text = enrichedDocument.Text.Substring(0, enrichedDocument.Text.IndexOf("*** END OF"));
                }

                if (indexOfBookEnd < 0)
                {
                    indexOfBookEnd = enrichedDocument.Text.IndexOf("***END OF");
                    if (indexOfBookEnd > 0)
                    {
                        enrichedDocument.Text = enrichedDocument.Text.Substring(0, enrichedDocument.Text.IndexOf("***END OF"));
                    }
                }

                return enrichedDocument;
            }, executionDataFlowOptions);

            // TPL Block: Download the requested Gutenberg book resources as a text string
            var chunkedLinesEnrichment = new TransformBlock<EnrichedDocument, EnrichedDocument>(enrichedDocument =>
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("Chunking text for: '{0}'", enrichedDocument.BookTitle);

                // Get the encoding for text-embedding-ada-002
                var cl100kBaseEncoding = GptEncoding.GetEncoding("cl100k_base");
                // Return the optimal text encodings, this is if tokens can be split perfect (no overlap)
                var encodedTokens = cl100kBaseEncoding.Encode(enrichedDocument.Text);

                enrichedDocument.TokenLength = encodedTokens.Count;
                tokenLength += encodedTokens.Count;

                return enrichedDocument;
            }, executionDataFlowOptions);

            // TPL Block: Performs text Machine Learning on the book texts
            var machineLearningEnrichment = new TransformBlock<EnrichedDocument, EnrichedDocument>(enrichedDocument =>
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("Machine Learning enrichment for: '{0}'", enrichedDocument.BookTitle);

                // Replace the text
                //enrichedDocument.Text = enrichedDocument.Text.Replace("\r\n", " ");
                enrichedDocument.TextLength = enrichedDocument.Text.Length;

                var textData = new TextData { Text = enrichedDocument.Text };
                var textDataArray = new TextData[] { textData };

                IDataView emptyDataView = mlContext.Data.LoadFromEnumerable(new List<TextData>());
                EstimatorChain<StopWordsRemovingTransformer> textPipeline = mlContext.Transforms.Text
                    .NormalizeText("NormalizedText", "Text", caseMode: TextNormalizingEstimator.CaseMode.Lower, keepDiacritics: true, keepPunctuations: false, keepNumbers: false)
                    .Append(mlContext.Transforms.Text.TokenizeIntoWords("WordTokens", "NormalizedText", separators: new[] { ' ' }))
                    .Append(mlContext.Transforms.Text.RemoveDefaultStopWords("WordTokensRemovedStopWords", "WordTokens", language: language));
                TransformerChain<StopWordsRemovingTransformer> textTransformer = textPipeline.Fit(emptyDataView);
                PredictionEngine<TextData, TransformedTextData> predictionEngine = mlContext.Model.CreatePredictionEngine<TextData, TransformedTextData>(textTransformer);

                var prediction = predictionEngine.Predict(textData);

                // Set ML Enriched document variables
                enrichedDocument.NormalizedText = prediction.NormalizedText;
                enrichedDocument.WordTokens = prediction.WordTokens;
                enrichedDocument.WordTokensRemovedStopWords = prediction.WordTokensRemovedStopWords;

                // Calculate Stop Words
                var result = enrichedDocument.WordTokensRemovedStopWords.AsParallel()
                    .Where(word => word.Length > 3)
                    .GroupBy(x => x)
                    .OrderByDescending(x => x.Count())
                    .Select(x => new WordCount { WordName = x.Key, Count = x.Count() }).Take(100)
                    .ToList();

                enrichedDocument.TopWordCounts = result;

                // Calculate the Paragraphs based on TokenCount
                var enrichedDocumentLines = Microsoft.SemanticKernel.Text.TextChunker.SplitPlainTextLines(enrichedDocument.Text, 200);
                enrichedDocument.Paragraphs = Microsoft.SemanticKernel.Text.TextChunker.SplitPlainTextParagraphs(enrichedDocumentLines, 600);

                return enrichedDocument;
            }, executionDataFlowOptions);

            // TPL Block: Get OpenAI Embeddings
            var retrieveEmbeddings = new TransformBlock<EnrichedDocument, EnrichedDocument>(async enrichedDocument =>
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("OpenAI Embeddings for: '{0}'", enrichedDocument.BookTitle);

                foreach (var paragraph in enrichedDocument.Paragraphs)
                {
                    var embeddings = new EmbeddingsOptions(paragraph);
                    var result = await openAIClient.GetEmbeddingsAsync("text-embedding-ada-002", embeddings);
                    var embeddingsVector = result.Value.Data[0].Embedding;
                    enrichedDocument.ParagraphEmbeddings.Add(embeddingsVector.ToList());
                }

                return enrichedDocument;

            }, executionDataFlowOptions);

            // TPL Block: Persist the document embeddings and the final enriched document to Json
            var persistToDatabaseAndJson = new TransformBlock<EnrichedDocument, EnrichedDocument>(enrichedDocument =>
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("Persisting to DB & converting to JSON file: '{0}'", enrichedDocument.BookTitle);

                // 1 - Save to SQL Server
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    for (int i = 0; i != enrichedDocument.Paragraphs.Count; i++)
                    {
                        using (SqlCommand command = new SqlCommand(string.Empty, connection))
                        {
                            command.CommandText = "INSERT INTO ProjectGutenbergBooks(Author, BookTitle, Url, Paragraph, ParagraphEmbeddings) VALUES(@author, @bookTitle, @url, @paragraph, @paragraphEmbeddings)";
                            command.Parameters.AddWithValue("@author", enrichedDocument.Author);
                            command.Parameters.AddWithValue("@bookTitle", enrichedDocument.BookTitle);
                            command.Parameters.AddWithValue("@url", enrichedDocument.Url);
                            command.Parameters.AddWithValue("@paragraph", enrichedDocument.Paragraphs[i]);
                            var jsonStringParagraphEmbeddings = JsonConvert.SerializeObject(enrichedDocument.ParagraphEmbeddings[i]);
                            command.Parameters.AddWithValue("@paragraphEmbeddings", jsonStringParagraphEmbeddings);

                            command.ExecuteNonQuery();
                        }
                    }

                    connection.Close();
                }

                // 2 - Write out JSON file
                // Convert to JSON String (all the public properties)
                var jsonString = JsonConvert.SerializeObject(enrichedDocument);

                // Create the book file path
                var jsonBookFilePath = Path.Combine(currentEnrichmentFolder, enrichedDocument.JsonFileName);

                // Write out JSON string to local storage
                using (StreamWriter outputFile = new StreamWriter(jsonBookFilePath))
                {
                    outputFile.WriteLine(jsonString);
                }

                return enrichedDocument;
            });

            // TPL Block: Prints out final information of enriched document
            var printEnrichedDocument = new ActionBlock<EnrichedDocument>(enrichedDocument =>
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\"{0}\" | Text Size: {1} | Word Count: {2} | Word Count Removed Stop Words: {3}",
                   enrichedDocument.ID,
                   enrichedDocument.Text.Length.ToString(),
                   enrichedDocument.WordTokens.Length.ToString(),
                   enrichedDocument.WordTokensRemovedStopWords.Length.ToString());

                Console.ForegroundColor = ConsoleColor.Green;
            });


            // TPL Pipeline: Build the pipeline workflow graph from the TPL Blocks
            var enrichmentPipeline = new BufferBlock<EnrichedDocument>(dataFlowBlockOptions);
            enrichmentPipeline.LinkTo(downloadBookText, dataFlowLinkOptions);
            downloadBookText.LinkTo(chunkedLinesEnrichment, dataFlowLinkOptions);
            chunkedLinesEnrichment.LinkTo(machineLearningEnrichment, dataFlowLinkOptions);
            machineLearningEnrichment.LinkTo(retrieveEmbeddings, dataFlowLinkOptions);
            retrieveEmbeddings.LinkTo(persistToDatabaseAndJson, dataFlowLinkOptions);
            persistToDatabaseAndJson.LinkTo(printEnrichedDocument, dataFlowLinkOptions);

            // TPL: Start the producer by feeding it a list of books
            var enrichmentProducer = ProduceGutenbergBooks(enrichmentPipeline, ProjectGutenbergBookService.GetBooks());

            // Since this is an asynchronous Task process, wait for the producer to finish putting all the messages on the queue
            // Works when queue is limited and not a "forever" queue
            await Task.WhenAll(enrichmentProducer);

            // Wait for the last block in the pipeline to process all messages.
            printEnrichedDocument.Completion.Wait();


            stopwatch.Stop();
            // Print out duration of work
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Job Completed In:  {0} seconds", + stopwatch.Elapsed.TotalSeconds);
            Console.WriteLine("Total Text Tokens: " + tokenLength);
        }
    }
}
