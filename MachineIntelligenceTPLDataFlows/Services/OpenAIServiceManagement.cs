using MachineIntelligenceTPLDataFlows.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MachineIntelligenceTPLDataFlows.Services
{
    public class OpenAIServiceManagement : IOpenAIServiceManagement
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient _httpClient;

        public OpenAIServiceManagement(HttpClient client)
        {
            _httpClient = client;
        }

        private string _apiKey;
        public string APIKey
        {
            get => _apiKey;
            set => _apiKey = value;
        }

        public async Task<List<float>> GetEmbeddings(string textToEncode)
        {
            var embeddings = new List<float>(1536);

            var httpClient = _httpClient;
            var requestBody = new { input = textToEncode, model = "text-embedding-ada-002" };
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); //ACCEPT header
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + this.APIKey);
            var responseService = await httpClient.PostAsync("https://api.openai.com/v1/embeddings", content);

            // Check the Response
            if (responseService.IsSuccessStatusCode)
            {
                var responseJsonString = await responseService.Content.ReadAsStringAsync();
                var openAIEmbeddings = JsonConvert.DeserializeObject<OpenAIEmbeddings>(responseJsonString);
                embeddings = openAIEmbeddings.data[0].embedding.ToList();
                //embeddingsString = System.Text.Json.JsonSerializer.Serialize(embeddingsVectorList);
            }

            return embeddings;
        }
    }
}
