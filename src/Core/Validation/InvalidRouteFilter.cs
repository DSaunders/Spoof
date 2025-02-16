using Core.Logging;
using Core.RouteModels;

namespace Core.Validation;

public class InvalidRouteFilter(ILogger logger)
{
    private readonly RouteValidator _validator = new();
    
    public List<Route> Filter(List<Route> routes)
    {
        var validRoutes = new List<Route>();
       
        foreach (var route in routes)
        {
            var results = _validator.Validate(route);

            foreach (var result in results)
            {
                logger.Error("Route {0} " + result, route.RouteFileName);
            }

            if (!results.Any())
                validRoutes.Add(route);
            else 
                logger.Error("Route {0} will be ignored", route.RouteFileName);
        }

        return validRoutes;
    }
}