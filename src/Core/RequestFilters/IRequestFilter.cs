using Core.Models;
using Core.RouteModels;

namespace Core.RequestFilters;

public interface IRequestFilter
{
    List<Route> Filter(IReadOnlyList<Route> routes, HttpRequest httpRequest);
}