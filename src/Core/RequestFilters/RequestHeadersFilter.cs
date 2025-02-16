using Core.Models;
using Core.RouteModels;

namespace Core.RequestFilters;

public class RequestHeadersFilter: IRequestFilter
{
    public List<Route> Filter(IReadOnlyList<Route> routes, HttpRequest httpRequest)
    {
        return routes.Where(route =>
        {
            var routeHeaders = route.Request.Headers;
            
            return routeHeaders.All(routeHeader =>
                httpRequest.Headers.ContainsKey(routeHeader.Key) &&
                httpRequest.Headers[routeHeader.Key] == routeHeader.Value);
            
        }).ToList();
    }
}