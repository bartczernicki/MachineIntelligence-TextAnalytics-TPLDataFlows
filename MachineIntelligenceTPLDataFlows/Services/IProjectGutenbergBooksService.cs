using MachineIntelligenceTPLDataFlows.Classes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MachineIntelligenceTPLDataFlows.Services
{
    public interface IProjectGutenbergBooksService
    {
        public Task<string> GetBookText(string bookUrl);
        public List<ProjectGutenbergBook> GetBooksList();
        public List<SearchMessage> GetQueriesList();
    }
}
