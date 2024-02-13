using System.Collections.Generic;
using System.Threading.Tasks;

namespace MachineIntelligenceTPLDataFlows.Services
{
    public interface IOpenAIServiceManagement
    {
        Task<List<float>> GetEmbeddings(string textToEncode);
        string APIKey { get; set; }
        string ModelIdEmbeddings { get; set; }
        int ModelIdEmbeddingsDimensions { get; set; }
    }
}
