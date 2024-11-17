# MachineIntelligence-TextAnalytics-TPLDataFlows

## Machine Intelligence Text Analytics Enrichment implemented using Task Parallel Library Data Flow Pipelines:

   * Document Enrichment Pipeline - Builds the entire Vector Database using OpenAI embeddings in SQL using 50 selected books  
   * Q&A Over Vector Database Pipeline - Searches the SQL Vector Database with provided question phrase using Semantic Kernel
   * Total Text (OpenAI) Tokens Processed:...............8,267,408    
   * Total Text (Characters) Length Processed:..........33,702,085  
   * Total cost for processing and building Vector Database using OpenAI Embeddings (Feb 2024 prices):
       * **text-embedding-ada-002** with 1536 dimensions: ~$0.84 (~84 cents; this depends on how the chunking of text is configured)
       * **text-embedding-3-small** with 512 dimensions: ~$0.17 (~17 cents; this depends on how the chunking of text is configured)

![TPL Pipeline](https://github.com/bartczernicki/MachineIntelligence-TextAnalytics-TPLDataFlows/blob/master/TPLDataFlows-Pipeline.png)

## Features:
* The console app uses 50 selected books from the Project Gutenberg site from various authors: Oscar Wilde, Bram Stoker, Edgar Allen Poe, Alexandre Dumas and performs enrichment using multiple AI enrichment steps
* Downloads book text, processes text analytics & embeddings, creates a vector database in SQL, demonstrates vector search and answers a sample question using semantic meaning from OpenAI embeddings
* Stores all enrichment output for each book in a seperate JSON file
* Rather than processing text analytics enrichment in single synchronous steps, it uses an data flow model to create efficient pipelines that can saturate multiple logical CPU cores  
* Illustrates that SQL Server or Azure SQL can be used as a valid Vector Store, can perform vector search and provide Q&A over the database
* Demonstrates how to create a Machine Intelligence & Text Analytics Pipeline can be combbined using TPL DataFlows
* The console application is cross-platform .NET 8.x. It will run on macOS, Linux, Windows 10/11 x64, Windows 11 ARM

## Requirements:
* Visual Studio 2022, .NET 9.x: https://dotnet.microsoft.com/en-us/download/dotnet/9.0  
* SQL Server Connection to either a local SQL Server 2022 (free Devolpment SKU or higher) or Azure SQL Database
* ******Note: SQL Server 2022 / Azure SQL Database features are used for JSON processing and ordered Columnstore Indexes
* Azure OpenAI for both embeddings and Chat Completions

![Training Job](https://github.com/bartczernicki/MachineIntelligence-TextAnalytics-TPLDataFlows/blob/master/TPLVectorEmbeddingsProcessingConsole.gif)

## Getting Started - Step 1) Configuration of SQL Connection and OpenAI API Keys (example of secrets.json shown below)
* Ensure to add .NET Secrets or JSON configuration (you will need to add the JSON code if using a file)
* Right-click on the C# Project and select "Manage User Secrets"
* Add the SQL Connection (SQLConnection) and OpenAI (APIKey) (if using Azure OpenAPI, use AzureOpenAPI section)  

```javascript
{
  "SQL": {
    "SqlConnection": "Server=[NAME OF SERVER],1433;Initial Catalog=MachineIntelligenceDb;Persist Security Info=False;User ID=[USERID];Password=[PASSWORD];MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=5000;"
  },
  "OpenAI": {
    "APIKey": "[YOUR OPENAPI KEY]"
  },
  "AzureOpenAI": {
    "APIKey": "[YOUR AZURE OPENAPI KEY]"
  }
}
```
  
## Getting Started - Step 2) Processing (after adding proper SQL and OpenAI/Azure OpenAI connections):
* Select option 1 to process the entire Data Enrichment Pipeline (build the embeddings Vector Database in SQL)  
* Select option 2 to only process the Q&A pipeline using Semantic Kernel over the Vector Database (Note: Option #1 must have been run beforehand)  
* Select option 3 to only process the Q&A pipeline with reasoning using Semantic Kernel over the Vector Database (Note: Option #1 must have been run beforehand). This option is similar to option #2 except it provides details on how the AI agent achieved the results.

![Getting Started - Console App](https://github.com/bartczernicki/MachineIntelligence-TextAnalytics-TPLDataFlows/blob/master/TPLDataFlows-ConsoleApp.png)

## Learn more about the concepts used in this repository:
* Semenantic Kernel: https://aka.ms/semantic-kernel
* OpenAI Embeddings: https://platform.openai.com/docs/guides/embeddings  
* TPL Dataflows .NET: https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/dataflow-task-parallel-library
* SQL Server Columnstore Indexes: https://learn.microsoft.com/en-us/sql/relational-databases/indexes/columnstore-indexes-overview  
* Project Gutenberg (over 70,000 free eBooks): https://www.gutenberg.org/  
* SharpToken (C# for encoding/decoding LLM tokens): https://github.com/dmitry-brazhenko/SharpToken  
* Use .NET secrets in a Console Application: https://www.programmingwithwolfgang.com/use-net-secrets-in-console-application/  
