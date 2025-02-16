using System.Text.RegularExpressions;
using Core.RouteModels;

namespace Core.Validation;

public class RouteValidator
{
    public List<string> Validate(Route route)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(route.Request.Path))
        {
            errors.Add("does not contain a request.path property");
        }
        else
        {
            var routeWithMoreThanSlash = @"[^\/\\]+";
            if (!Regex.IsMatch(route.Request.Path, routeWithMoreThanSlash))
                errors.Add("specifies a mock API at the root ('/')");
        }
        
        if (string.IsNullOrWhiteSpace(route.Request.HttpMethod))
            errors.Add("does not contain a request.httpMethod property");
        
        return errors;
    }
}