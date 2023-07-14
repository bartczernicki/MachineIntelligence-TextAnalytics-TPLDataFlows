using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineIntelligenceTPLDataFlows.Services
{
    public interface IOpenAIServiceManagement
    {
        Task<string> GetEmbeddings(string textToEncode);
        string APIKey { get; set; }
    }
}
