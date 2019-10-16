using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Text;
using static Microsoft.ML.Transforms.Text.StopWordsRemovingEstimator.Language;

namespace MachineIntelligenceTPLDataFlows

{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Machine Intelligence (Text Analytics) with TPL Data Flows";

            // Instantiate new ML.NET Context
            // Note: MlContext is thread-safe
            var mlContext = new MLContext(100);

            // Set language to English
            StopWordsRemovingEstimator.Language language = StopWordsRemovingEstimator.Language.English;

            var options = new ExecutionDataflowBlockOptions();
            options.MaxDegreeOfParallelism = 4;

            static async Task ProduceGutenbergBooks(BufferBlock<EnrichedDocument> queue,
                IEnumerable<ProjectGutenbergBook> projectGutenbergBooks)
            {
                foreach (var projectGutenbergBook in projectGutenbergBooks)
                {
                    var enrichedDocument = new EnrichedDocument
                    {
                        BookTitle = projectGutenbergBook.BookTitle,
                        Url = projectGutenbergBook.Url
                    };

                    await queue.SendAsync(enrichedDocument);
                }

                queue.Complete();
            }

            // Download the requested book resource as a string.
            var downloadBookText = new TransformBlock<EnrichedDocument, EnrichedDocument>(async enrichedDocument =>
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("Downloading '{0}'...", enrichedDocument.Url);

                enrichedDocument.Text = await new HttpClient().GetStringAsync(enrichedDocument.Url);

                return enrichedDocument;
            }, options);

            // Separates the specified text into an array of words.
            var machineLearningEnrichment = new TransformBlock<EnrichedDocument, EnrichedDocument>(enrichedDocument =>
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("Machine Learning enrichment for: " + enrichedDocument.BookTitle);

                // Replace the text
                enrichedDocument.Text = enrichedDocument.Text.Replace("\r\n", " ");

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

                // Set enrichment variables
                enrichedDocument.NormalizedText = prediction.NormalizedText;
                enrichedDocument.WordTokens = prediction.WordTokens;
                enrichedDocument.WordTokensRemovedStopWords = prediction.WordTokensRemovedStopWords;

                return enrichedDocument;
            }, options);

            var textAnalytics = new TransformBlock<EnrichedDocument, EnrichedDocument>(enrichedDocument =>
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("Text Analytics For: " + enrichedDocument.BookTitle);

                var result = enrichedDocument.WordTokensRemovedStopWords.AsParallel()
                    .Where(word => word.Length > 3)
                    .GroupBy(x => x)
                    .OrderByDescending(x => x.Count())
                    .Select(x => new Tuple<string, int>(x.Key, x.Count())).Take(100)
                    .ToList();

                enrichedDocument.TopWordCounts = result;

                return enrichedDocument;
            }, options);

            var printEnrichedDocument = new ActionBlock<EnrichedDocument>(enrichedDocument =>
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\"{0}\" | Text Size: {1} | Word Count: {2} | Word Count Removed Stop Words: {3}",
                   enrichedDocument.BookTitle, enrichedDocument.Text.Length.ToString(),
                   enrichedDocument.WordTokens.Length.ToString(),
                   enrichedDocument.WordTokensRemovedStopWords.Length.ToString());
            });

            var linkOptions = new DataflowLinkOptions { PropagateCompletion = true };

            var enrichmentPipeline = new BufferBlock<EnrichedDocument>(new DataflowBlockOptions { BoundedCapacity = 5, EnsureOrdered = true });
            enrichmentPipeline.LinkTo(downloadBookText, linkOptions);
            downloadBookText.LinkTo(machineLearningEnrichment, linkOptions);
            machineLearningEnrichment.LinkTo(textAnalytics, linkOptions);
            textAnalytics.LinkTo(printEnrichedDocument, linkOptions);

            var enrichmentProducer = ProduceGutenbergBooks(enrichmentPipeline, ProjectGutenbergBookService.GetBooks());

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
