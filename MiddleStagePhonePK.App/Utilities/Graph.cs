namespace MiddleStagePhonePK.App.Utilities;

public static class Graph
{
    public static string LoadDefinitions()
    {
        string filePath = Path.GetFullPath(Path.Combine(".", "Models", "OutboundGraphs"));
        var extensions = new List<string>() { ".graphql", ".gql" };

        List<string> files = Directory.GetFiles(filePath)
          .Where(file => extensions.Any(file.ToLower().EndsWith)).ToList();

        string typeDefs = "";

        foreach (var file in files)
        {
            typeDefs += File.ReadAllText(file);
        }

        return typeDefs;
    }
}
