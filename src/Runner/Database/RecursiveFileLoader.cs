using Core.Database;
using Core.RouteParser;
using ILogger = Core.Logging.ILogger;
using Route = Core.RouteModels.Route;

namespace Runner.Database;

public class RecursiveFileLoader(ILogger log, IRouteParser parser) : IRoutesLoader
{
    public List<Route> LoadRoutes(string startFolder)
    {
        var routes = new List<Route>();

        var allFiles = Directory.GetFiles(startFolder, "*.route.json", SearchOption.AllDirectories);

        foreach (var file in allFiles)
        {
            var relativePath = Path.GetRelativePath(startFolder, file);

            log.Verbose("Loading route from file {0}", relativePath);
        
            var route = parser.Parse(File.ReadAllText(file), relativePath);
            if (route is not null)
                routes.Add(route);
        }

        return routes;
    }
}