using Core.Models;
using Core.RouteModels;

namespace Core.RequestFilters;

public class HttpMethodFilter : IRequestFilter
{
    public List<Route> Filter(IReadOnlyList<Route> routes, HttpRequest httpRequest)
    {
        return routes.Where(route =>
            route.Request.HttpMethod.Equals(httpRequest.HttpMethod, StringComparison.InvariantCultureIgnoreCase)
        ).ToList();
    }
}