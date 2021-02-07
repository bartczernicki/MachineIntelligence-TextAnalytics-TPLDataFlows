using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Text;
using Newtonsoft.Json;

namespace MachineIntelligenceTPLDataFlows

{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Machine Intelligence (Text Analytics) with TPL Data Flows";

            // CONFIG 
            // Instantiate new ML.NET Context
            // Note: MlContext is thread-safe
            var mlContext = new MLContext(100);

            // GET Current Environment Folder
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

            // SET the Data Flow Block Options
            // This controls the data flow from the Producer level
            var dataFlowBlockOptions = new DataflowBlockOptions {
                BoundedCapacity = 5,
                MaxMessagesPerTask = 5 };

            // SET the data flow pipeline options
            // Note: Set MaxMessages to the number of books to process
            // Note: For example, setting MaxMessages to 2 will run only two books through the pipeline
            var dataFlowLinkOptions = new DataflowLinkOptions {
                PropagateCompletion = true,
                //MaxMessages = 1
            };


            static async Task ProduceGutenbergBooks(BufferBlock<EnrichedDocument> queue,
                IEnumerable<ProjectGutenbergBook> projectGutenbergBooks)
            {
                foreach (var projectGutenbergBook in projectGutenbergBooks)
                {
                    // Add baseline information from the document
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

            // Download the requested Gutenberg book resources as a string.
            var downloadBookText = new TransformBlock<EnrichedDocument, EnrichedDocument>(async enrichedDocument =>
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("Downloading '{0}'...", enrichedDocument.BookTitle);

                enrichedDocument.Text = await new HttpClient().GetStringAsync(enrichedDocument.Url);

                return enrichedDocument;
            }, executionDataFlowOptions);

            // Peforms Machine Learning on the book texts
            var machineLearningEnrichment = new TransformBlock<EnrichedDocument, EnrichedDocument>(enrichedDocument =>
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("Machine Learning enrichment for: " + enrichedDocument.BookTitle);

                // Replace the text
                enrichedDocument.Text = enrichedDocument.Text.Replace("\r\n", " ");
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

                // Set Enriched document variables
                enrichedDocument.NormalizedText = prediction.NormalizedText;
                enrichedDocument.WordTokens = prediction.WordTokens;
                enrichedDocument.WordTokensRemovedStopWords = prediction.WordTokensRemovedStopWords;

                return enrichedDocument;
            }, executionDataFlowOptions);

            // Performs additional book analytics
            var textAnalytics = new TransformBlock<EnrichedDocument, EnrichedDocument>(enrichedDocument =>
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("Text Analytics For: " + enrichedDocument.BookTitle);

                var result = enrichedDocument.WordTokensRemovedStopWords.AsParallel()
                    .Where(word => word.Length > 3)
                    .GroupBy(x => x)
                    .OrderByDescending(x => x.Count())
                    .Select(x => new WordCount { WordName = x.Key, Count = x.Count() }).Take(100)
                    .ToList();

                enrichedDocument.TopWordCounts = result;

                return enrichedDocument;
            }, executionDataFlowOptions);

            // Convert final enriched document to Json
            var convertToJson = new TransformBlock<EnrichedDocument, EnrichedDocument>(enrichedDocument =>
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("Converting '{0}' to JSON file", enrichedDocument.BookTitle);

                // Convert to JSON String
                var jsonString = JsonConvert.SerializeObject(enrichedDocument);

                // Remove special characters from book titles
                // Author + Book Title
                var jsonBookFileName =
                    enrichedDocument.Author.Replace(" ", string.Empty) + "-" +
                    enrichedDocument.BookTitle
                        .Replace(" ", string.Empty)
                        .Replace("'", string.Empty)
                        .Replace(":", string.Empty)
                        + ".json";

                // Create the book file path
                var jsonBookFilePath = Path.Combine(currentEnrichmentFolder, jsonBookFileName);

                // Write out JSON string to local storage
                using (StreamWriter outputFile = new StreamWriter(jsonBookFilePath))
                {
                    outputFile.WriteLine(jsonString);
                }

                return enrichedDocument;
            });

            // Prints out final information of enriched document
            var printEnrichedDocument = new ActionBlock<EnrichedDocument>(enrichedDocument =>
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\"{0}\" | Text Size: {1} | Word Count: {2} | Word Count Removed Stop Words: {3}",
                   enrichedDocument.BookTitle, enrichedDocument.Text.Length.ToString(),
                   enrichedDocument.WordTokens.Length.ToString(),
                   enrichedDocument.WordTokensRemovedStopWords.Length.ToString());
            });


            // Build the pipeline graph
            var enrichmentPipeline = new BufferBlock<EnrichedDocument>(dataFlowBlockOptions);
            enrichmentPipeline.LinkTo(downloadBookText, dataFlowLinkOptions);
            downloadBookText.LinkTo(machineLearningEnrichment, dataFlowLinkOptions);
            machineLearningEnrichment.LinkTo(textAnalytics, dataFlowLinkOptions);
            textAnalytics.LinkTo(convertToJson, dataFlowLinkOptions);
            convertToJson.LinkTo(printEnrichedDocument, dataFlowLinkOptions);

            // Start the producer by feeding it a list of books
            var enrichmentProducer = ProduceGutenbergBooks(enrichmentPipeline, ProjectGutenbergBookService.GetBooks());

            // Since this is an asynchronous Task proceess, wait for the producer to finish
            Task.WhenAll(enrichmentProducer);

            // Wait for the last block in the pipeline to process all messages.
            printEnrichedDocument.Completion.Wait();

            //// Separates the specified text into an array of words.
            //var createWordList = new TransformBlock<string, string[]>(text =>
            //{
            //    Console.WriteLine("Creating word list...");

            //    var textData = new TextData { Text = text };
            //    var textDataArray = new TextData[] { textData };

            //    IDataView emptyDataView = mlContext.Data.LoadFromEnumerable(new List<TextData>());
            //    EstimatorChain<StopWordsRemovingTransformer> textPipeline = mlContext.Transforms.Text
            //        .NormalizeText("Text", caseMode: TextNormalizingEstimator.CaseMode.Lower, keepDiacritics: true, keepPunctuations: false, keepNumbers: false)
            //        .Append(mlContext.Transforms.Text.TokenizeIntoWords("Words", "Text", separators: new[] { ' ' }))
            //        .Append(mlContext.Transforms.Text.RemoveDefaultStopWords("WordsRemovedStopWords", "Words", language: language));
            //    TransformerChain<StopWordsRemovingTransformer> textTransformer = textPipeline.Fit(emptyDataView);
            //    PredictionEngine<TextData, TransformedTextData> predictionEngine = mlContext.Model.CreatePredictionEngine<TextData, TransformedTextData>(textTransformer);

            //    //    var prediction = predictionEngine.Predict(textData);

            //    var textDataView = mlContext.Data.LoadFromEnumerable<TextData>(textDataArray);

            //    //var pipeline =
            //    //    // One-stop shop to run the full text featurization.
            //    //    mlContext.Transforms.Text.FeaturizeText("TextFeatures", "Text");
            //    //var transformedData = pipeline.Fit(textDataView).Transform(textDataView);
            //    //var test = transformedData.GetColumn<float[]>("TextFeatures").Take(10).ToArray();

            //    var pipeline2 = mlContext.Transforms.Text
            //             .NormalizeText("Text", caseMode: TextNormalizingEstimator.CaseMode.Lower, keepDiacritics: true, keepPunctuations: false, keepNumbers: false)
            //            .Append(mlContext.Transforms.Text.TokenizeIntoWords("Words", "Text"))
            //            .Append(mlContext.Transforms.Text.ApplyWordEmbedding("Features", "Words", WordEmbeddingEstimator.PretrainedModelKind.GloVe50D));
            //    var transformedData2 = pipeline2.Fit(textDataView);

            //    var predictionEngine2 = mlContext.Model.CreatePredictionEngine<TextData,
            //        TransformedTextData>(transformedData2);
            //    // Call the prediction API to convert the text into embedding vector.
            //    var data2 = new TextData
            //    {
            //        Text = "This is a great product. I would " +
            //        "like to buy it again."
            //    };
            //    var prediction2 = predictionEngine2.Predict(data2);

            //    // Remove common punctuation by replacing all non-letter characters 
            //    // with a space character.
            //    char[] tokens = text.Select(c => char.IsLetter(c) ? c : ' ').ToArray();
            //    text = new string(tokens);

            //    // Separate the text into an array of words.
            //    return text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            //}, options);
        }
    }

    public class TextData
    {
        public string Text { get; set; }
    }

    public class TransformedTextData
    {
        public string NormalizedText { get; set; }

        public string[] WordTokens { get; set; }

        public string[] WordTokensRemovedStopWords { get; set; }
    }
}
