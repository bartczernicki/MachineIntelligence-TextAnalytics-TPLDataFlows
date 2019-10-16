using System;
using System.Collections.Generic;

namespace MachineIntelligenceTPLDataFlows
{
    public class EnrichedDocument
    {
        // Source
        public string BookTitle { get; set; }
        public string Url { get; set; }

        // Enrichment
        public string Text { get; set; }
        public string NormalizedText { get; set; }
        public string[] WordTokens { get; set; }
        public string[] WordTokensRemovedStopWords { get; set; }

        public List<Tuple<string, int>> TopWordCounts { get; set; }
    }
}
