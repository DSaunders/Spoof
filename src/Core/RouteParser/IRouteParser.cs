using Route = Core.RouteModels.Route;

namespace Core.RouteParser;

public interface IRouteParser
{
    Route? Parse(string contents, string filename);
}