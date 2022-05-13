// See https://aka.ms/new-console-template for more information
using GraphAZLib;
using GraphAZLib.Helpers;
using Newtonsoft.Json;

Console.WriteLine("GraphAZConnector");
Console.WriteLine("--------------------------------");

try
{

    Console.Write("Please enter your Sentinel Workspace Id: ");    
    var lWorkspaceId = Console.ReadLine();

    Console.Write("Please enter your Sentinel Workspace Shared key: ");    
    var lSharedKey = Console.ReadLine();

    Console.Write("Please enter your Bloodhound neo4j export file path: ");
    var lFilePath = Console.ReadLine();

    if (lSharedKey != null && lWorkspaceId != null && lFilePath != null)
    {
        // Nodes
        var lNodes = Neo4jHelper.GetNodes(lFilePath);

        var lNodesJson = JsonConvert.SerializeObject(lNodes);

        LogAnalyticsHelper.PostNeo4JDb(lWorkspaceId, lSharedKey, lNodesJson, "UBHnodes");

        // Relationships
        var lRels = Neo4jHelper.GetRelationships(lFilePath);

        var lRelJson = JsonConvert.SerializeObject(lRels);

        LogAnalyticsHelper.PostNeo4JDb(lWorkspaceId, lSharedKey, lRelJson, "UBHrelationships");

        Console.WriteLine("Finished.");
    }
    else
    {
        Console.WriteLine("Please provide proper value and retry...");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Something went wrong: {ex.Message}");
}