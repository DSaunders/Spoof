using Core.Helpers;
using Core.Models;
using Core.RouteModels;

namespace Core.RequestFilters;

public class PathFilter : IRequestFilter
{
    public List<Route> Filter(IReadOnlyList<Route> routes, HttpRequest httpRequest)
    {
        var newRoutes = new List<(Route route, int score)>();

        var pathParts = httpRequest.Path.Split('/');

        foreach (var route in routes)
        {
            var routePathParts = route.Request.Path.Split('/');

            if (pathParts.Length != routePathParts.Length)
                continue;

            if (pathParts.Where((t, i) => !RouteParsing.ParseRouteSection(routePathParts[i], t).match).Any())
                continue;

            var score = pathParts.Select((t, i) => 
                RouteParsing.ParseRouteSection(routePathParts[i], t).score
            ).Sum();
            
            newRoutes.Add((route, score));
        }

        // Return the successful routes, re-ordered by score
        return newRoutes
            .OrderBy(r => r.score)
            .Select(r => r.route)
            .ToList();
    }
}