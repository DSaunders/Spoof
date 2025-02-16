using Newtonsoft.Json;
using ILogger = Core.Logging.ILogger;
using Route = Core.RouteModels.Route;

namespace Core.RouteParser;

public class RouteParser(ILogger log) : IRouteParser
{
    public Route? Parse(string contents, string filename)
    {
        Route? route;
        try
        {
            route = JsonConvert.DeserializeObject<Route>(
                contents,
                new JsonSerializerSettings
                {
                    Converters = new List<JsonConverter> { new RouteResponseJsonParser() }
                }
            );
            
        }
        catch (Exception e)
        {
            log.Error("Could not parse {0}: " + e.Message, filename);
            log.Error("File {0} will be ignored", filename);
            return null;
        }

        if (route is null)
        {
            log.Error("Couldn't parse {0}. Route will be ignored", filename);
            return null;
        }
        
        route.RouteFileName = filename;
        return route;
    }
}