using System;
using System.Collections.Generic;

namespace MachineIntelligenceTPLDataFlows
{
    public class EnrichedDocument : ProjectGutenbergBook
    {
        // Source
        public new string BookTitle { get; set; }
        public new string Author { get; set; }
        public new string Url { get; set; }

        // Enrichment
        public int TextLength { get; set; }
        public string Text { get; set; }
        public string NormalizedText { get; set; }
        public string[] WordTokens { get; set; }
        public string[] WordTokensRemovedStopWords { get; set; }

        public List<WordCount> TopWordCounts { get; set; }
    }
}
