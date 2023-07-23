using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineIntelligenceTPLDataFlows.Classes
{
    enum ProcessingOptions
    {
        None = 0, 
        RunFullDataEnrichmentPipeline = 1,
        OnlyPerformQuestionAndAnswer = 2,
        OnlyPerformQuestionAndAnswerWithReasoning = 3
    }
}
