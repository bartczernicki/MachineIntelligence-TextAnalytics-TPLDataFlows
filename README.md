# MachineIntelligence-TextAnalytics-TPLDataFlows

Machine Intelligence Text Analytics Enrichment Actor Model, implemented using Task Parallel Library Data Flow Pipelines.

![TPL Pipeline](https://github.com/bartczernicki/MachineIntelligence-TextAnalytics-TPLDataFlows/blob/master/TPLDataFlows-Pipeline.png)

Features:
* The console uses book text from the Project Gutenberg site from various authors: Oscar Wilde, Bram Stoker, Edgar Allen Poe and performs enrichment using multiple steps
* Downloads books, processes text analytics & embeddings, creates a vector database in SQL, demonstrates vector search and answers a sample question with provided
* Rather than processing text analytics enrichment in single synchronous steps, it uses an data flow model to create efficient pipelines
* Demonstrates how to create a Machine Intelligence & Text Analytics Pipeline using TPL DataFlows
* Can saturate multiple cores of a workstation or server for efficient workflow processing
* The console application is cross-platform; it will run on macOS (screenshot below & Windows)

Requirements:
* Visual Studio 2022, .NET 8.x, SQL Server Connection, OpenAI Credentials

![Training Job](https://github.com/bartczernicki/MachineIntelligence-TextAnalytics-TPLDataFlows/blob/master/TPLVectorEmbeddingsProcessingConsole.gif)

More Information:
* TPL Dataflows: https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/dataflow-task-parallel-library
* Project Gutenberg: https://www.gutenberg.org/
