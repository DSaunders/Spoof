using System.Reflection;
using Newtonsoft.Json;
using Runner.Logging;
using ILogger = Core.Logging.ILogger;

namespace Runner.Scaffolding;

public class Scaffolder(ILogger logger, ITableLogger tableLogger)
{
    public const string ResourcePrefix = "Runner._scaffold.";

    public void ListTemplates()
    {
        var templates = new Dictionary<string, string>();

        foreach (var resourceName in GetAllResourceNames())
        {
            var template = GetRouteFromFile(resourceName);

            templates.Add(template.Name, template.Description);
        }

        tableLogger.LogTable(
            "The following scaffolding templates are available:",
            $"For example:{Environment.NewLine}  'spoof scaffold hello-world'",
            "Template Name",
            "Description",
            templates);
    }

    public string Scaffold(string source, string destination)
    {
        var template = GetAllResourceNames()
            .Select(GetRouteFromFile)
            .FirstOrDefault(t => t.Name == source);

        if (template is null)
        {
            logger.Warning(
                "Could not find scaffolding template {0}. Run 'spoof scaffold list' to show available templates",
                source);
            return string.Empty;
        }

        File.WriteAllText($"{destination}.route.json", template.Route.ToString());
        
        logger.Information("Created new mock file: {0}", $"{destination}.route.json");
        
        return $"{destination}.route.json";
    }

    private IEnumerable<string> GetAllResourceNames()
    {
        return Assembly.GetExecutingAssembly().GetManifestResourceNames();
    }

    private ScaffoldTemplate GetRouteFromFile(string file)
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(file);
        using var reader = new StreamReader(stream!);

        return JsonConvert.DeserializeObject<ScaffoldTemplate>(reader.ReadToEnd())!;
    }
}