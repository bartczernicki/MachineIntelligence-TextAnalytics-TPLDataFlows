using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineIntelligenceTPLDataFlows.Classes
{
    public class SearchMessage
    {
        public string SearchString { get; set; }
        public List<string> TopParagraphs { get; set; }
    }
}
