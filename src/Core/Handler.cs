using Core.Models;
using Core.RequestFilters;
using ILogger = Core.Logging.ILogger;
using Route = Core.RouteModels.Route;

namespace Core;

public class Handler(ILogger logger, List<Route> routes)
{
    private readonly List<IRequestFilter> _routeFilters = new()
    {
        new HttpMethodFilter(),
        new PathFilter(),
        new RequestHeadersFilter()
    };

    public Response HandleRequest(HttpRequest httpRequest)
    {
        logger.Information("Received request {0} to {1}", httpRequest.HttpMethod.ToUpper(), httpRequest.Path);

        var matchedRoutes = new List<Route>(routes);
        foreach (var routeFilter in _routeFilters)
        {
            matchedRoutes = routeFilter.Filter(matchedRoutes, httpRequest);
        }

        var route = matchedRoutes.FirstOrDefault();
        if (route is null)
        {
            logger.Warning(
                "Did not find a mock for {0} {1}. Returning a 404 response",
                httpRequest.HttpMethod.ToUpper(),
                httpRequest.Path
            );
            return new Response(404, string.Empty, new Dictionary<string, string>());
        }

        logger.Information("Matched a route for {0} {1}. Returning a response", httpRequest.HttpMethod.ToUpper(), httpRequest.Path);
        return route.Handle(httpRequest);
    }
}