using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MachineIntelligenceTPLDataFlows.Classes
{
    public class Data
    {
        public int index { get; set; }
        public IList<float> embedding { get; set; }
    }

    public class Usage
    {
        public int prompt_tokens { get; set; }
        public int total_tokens { get; set; }

    }

    public class OpenAIEmbeddings
    {
        public IList<Data> data { get; set; }
        public string model { get; set; }
        public Usage usage { get; set; } 
    }
}
