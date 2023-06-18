using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineIntelligenceTPLDataFlows.Classes
{
    public class TransformedTextData
    {
        public string NormalizedText { get; set; }

        public string[] WordTokens { get; set; }

        public string[] WordTokensRemovedStopWords { get; set; }
    }
}
