# MachineIntelligence-TextAnalytics-TPLDataFlows

Machine Intelligence Text Analytics Enrichment implemented using Task Parallel Library Data Flow Pipelines:
* Document Enrichment Pipeline - Builds the entire Vector Database using OpenAI embeddings in SQL using 30 selected books
* Q&A Over Vector Database Pipeline - Searches the Vector Database with provided phrase using Semantic Kernel
* Total Text (OpenAI) Tokens Processed:...............2,531,238  
* Total Text (Characters) Length Processed:..........10,529,043
* Total cost for OpenAI Embeddings (June 2023 prices):..$0.25 (25 cents)

![TPL Pipeline](https://github.com/bartczernicki/MachineIntelligence-TextAnalytics-TPLDataFlows/blob/master/TPLDataFlows-Pipeline.png)

Features:
* The console app uses selected 30 books from the Project Gutenberg site from various authors: Oscar Wilde, Bram Stoker, Edgar Allen Poe and performs enrichment using multiple enrichment steps
* Downloads book text, processes text analytics & embeddings, creates a vector database in SQL, demonstrates vector search and answers a sample question using semantic meaning from OpenAI embeddings
* Stores all enrichment output in a JSON file
* Rather than processing text analytics enrichment in single synchronous steps, it uses an data flow model to create efficient pipelines that can saturate multiple logical CPU cores
* Illustrates that SQL Server or Azure SQL can be used as a valid Vector Store, can perform vector search and provide Q&A over the database
* Demonstrates how to create a Machine Intelligence & Text Analytics Pipeline can be combbined using TPL DataFlows
* The console application is cross-platform .NET 8.x; it will run on macOS, Linux, Windows 10/11 x64, Windows 11 ARM

Requirements:
* Visual Studio 2022, .NET 8.x
* SQL Server Connection to either local SQL Server 2022 (free Devolpment SKU or higher) or Azure SQL Database
* OpenAI or Azure OpenAI API Access to both embeddings and completions

![Training Job](https://github.com/bartczernicki/MachineIntelligence-TextAnalytics-TPLDataFlows/blob/master/TPLVectorEmbeddingsProcessingConsole.gif)

Getting Started - 1) Configuration of SQL Connection and API Keys
* Ensure to add .NET Secrets or JSON configuration (you will need to add the JSON code if using a file)
* Right-click on the C# Project and select "Manage User Secrets"
* Add the SQL Connection (SQLConnection) and OpenAI (APIKey) (if using Azure OpenAPI, use AzureOpenAPI section)
![Getting Started - Console App](https://github.com/bartczernicki/MachineIntelligence-TextAnalytics-TPLDataFlows/blob/master/TPLDataFlows-Secrets.png)

Getting Started - 2) Processing (after adding proper SQL and OpenAI/Azure OpenAI connections):
* Select option 1 to process the entire Data Enrichment Pipeline (build the embeddings Vector Database in SQL)
* Select option 2 to just process the Q&A pipeline using Semantic Kernel over the Vector Database (Note: Option #1 must have been run beforehand at some point)

![Getting Started - Console App](https://github.com/bartczernicki/MachineIntelligence-TextAnalytics-TPLDataFlows/blob/master/TPLDataFlows-ConsoleApp.png)

More Information:
* Semenantic Kernel: https://aka.ms/semantic-kernel
* TPL Dataflows .NET: https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/dataflow-task-parallel-library
* Project Gutenberg (over 70,000 free eBooks): https://www.gutenberg.org/
* SharpToken (C# for encoding/decoding LLM tokens): https://github.com/dmitry-brazhenko/SharpToken  
