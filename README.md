# MachineIntelligence-TextAnalytics-TPLDataFlows

Machine Intelligence Text Analytics using Task Parallel Library Data Flow Pipelines.

Features:
* The console uses book text from the Project Gutenberg site from various authors: Oscar Wilde, Bram Stoker, Edgar Allen Poe and performs enrichment using multiple steps
* Rather than processing text analytics enrichment in single synchronous steps, it uses an actor model to create efficient pipelines
* Demonstrates how to create a Machine Intelligence & Text Analytics Pipeline using TPL DataFlows
* Can saturate multiple cores of a workstation or server for efficient workflow processing
* The console application is cross-platform; it will run on macOS (screenshot below & Windows)

Requirements:
* Visual Studio 2019, .NET Core 3.0, 
* NuGet Packages Used: ML.NET v1.3.1, Newtonsoft.Json, System.Threading.Tasks.Dataflow

![Training Job](https://github.com/bartczernicki/MachineIntelligence-TextAnalytics-TPLDataFlows/blob/master/ConsoleScreenshot.png)

More Information:
* TPL Dataflows: https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/dataflow-task-parallel-library
* Project Gutenberg: https://www.gutenberg.org/
