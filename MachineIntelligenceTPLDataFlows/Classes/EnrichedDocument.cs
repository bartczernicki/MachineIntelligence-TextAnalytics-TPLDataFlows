using System;
using System.Collections.Generic;

namespace MachineIntelligenceTPLDataFlows.Classes
{
    public class EnrichedDocument : ProjectGutenbergBook
    {
        // SOURCE
        public string ID => this.Author + "-" + this.BookTitle;
        public new string Author { get; set; }
        public new string BookTitle { get; set; }
        public new string Url { get; set; }
        public string JsonFileName => this.Author.Replace(" ", string.Empty) + "-" +
                        this.BookTitle
                        .Replace(" ", string.Empty)
                        .Replace("'", string.Empty)
                        .Replace(":", string.Empty)
                        + ".json";

        // ENRICHMENT - Order of properties is specific so the properties don't get lost in JSON with large amount of text
        public int TextLength { get; set; }
        public int TokenLength { get; set; }
        public string Text { get; set; }
        public List<string> Paragraphs { get; set; } = new List<string>(200);
        public List<string> ParagraphsWithNoTokensOverlap { get; set; } = new List<string>(200);
        public List<List<float>> ParagraphEmbeddings { get; set; } = new List<List<float>>(100);
        public string NormalizedText { get; set; }
        public string[] WordTokens { get; set; }
        public string[] WordTokensRemovedStopWords { get; set; }

        public List<WordCount> TopWordCounts { get; set; }
    }
}
