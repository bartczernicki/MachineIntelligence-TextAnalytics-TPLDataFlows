using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineIntelligenceTPLDataFlows.Classes
{
    public class ParagraphResults
    {
        public int Id { get; set; }
        public string BookTitle { get; set; }
        public string Author { get; set; }
        public string Paragraph { get; set; }
        public double CosineDistance { get; set; }
    }
}
