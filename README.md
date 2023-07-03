# MachineIntelligence-TextAnalytics-TPLDataFlows

Machine Intelligence Text Analytics Enrichment implemented using Task Parallel Library Data Flow Pipelines:
* Document Enrichment Pipeline - Builds the entire Vector DB in SQL using selected book text
* Q&A Over Vector Database Pipeline - Searches the Vector DB with provided query
* Total Text (OpenAI) Tokens Processed:              2,531,238  
* Total Text (Characters) Length Processed:         10,529,043
* Total cost for OpenAI Embeddings (06.2023 prices): $0.25 (25 cents)

![TPL Pipeline](https://github.com/bartczernicki/MachineIntelligence-TextAnalytics-TPLDataFlows/blob/master/TPLDataFlows-Pipeline.png)

Features:
* The console uses book text from the Project Gutenberg site from various authors: Oscar Wilde, Bram Stoker, Edgar Allen Poe and performs enrichment using multiple enrichment steps
* Downloads book text, processes text analytics & embeddings, creates a vector database in SQL, demonstrates vector search and answers a sample question using semantic meaning from OpenAI embeddings
* Rather than processing text analytics enrichment in single synchronous steps, it uses an data flow model to create efficient pipelines that can saturate multiple logical CPU cores
* Shows that SQL Server or Azure SQL can be used as a Vector Store, vector search and provide Q&A
* Demonstrates how to create a Machine Intelligence & Text Analytics Pipeline using TPL DataFlows
* The console application is cross-platform; it will run on macOS, Windows 10/11, Windows 11 ARM

Requirements:
* Visual Studio 2022, .NET 8.x
* SQL Server Connection to either SQL Server 2022 (Devolpment SKU or higher) or Azure SQL
* OpenAI or Azure OpenAI API Access to both embeddings and completions

![Training Job](https://github.com/bartczernicki/MachineIntelligence-TextAnalytics-TPLDataFlows/blob/master/TPLVectorEmbeddingsProcessingConsole.gif)

More Information:
* TPL Dataflows: https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/dataflow-task-parallel-library
* Project Gutenberg: https://www.gutenberg.org/
