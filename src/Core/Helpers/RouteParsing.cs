using System.Text.RegularExpressions;

namespace Core.Helpers;

internal static partial class RouteParsing
{
    [GeneratedRegex(@"^\{(?<variable>[^:}]+)(:(?<type>[^}]+))?\}$")]
    private static partial Regex RouteParamRegex();

    // public static (bool isParameter, string variable, string type) ParseRouteParam(string routePart)
    // {
    //     var isRouteParam = RouteParamRegex().Match(routePart);
    //     if (!isRouteParam.Success)
    //     {
    //         // Stryker disable once all - empty string args are not used
    //         return (false, string.Empty, string.Empty);
    //     }
    //
    //     var groups = RouteParamRegex().Match(routePart).Groups;
    //     var variable = groups["variable"].Value;
    //     var type = groups["type"].Value;
    //
    //     return (true, variable, type);
    // }
    //
    
    /// <summary>
    /// Scoring summary:
    ///     Each part that matches exactly is 0
    ///     Each part that is a typed variable (e.g. {id:int}) is 1
    ///     Each part that is an untyped variable (e.g. {id}) is 2
    /// The lower the total score, the better the match
    /// </summary>
    public static (bool match, int score) ParseRouteSection(string routePart, string requestUrlPart)
    {
        // Exact text match
        if (routePart.Equals(requestUrlPart, StringComparison.OrdinalIgnoreCase))
            return (true, 0);
        
        // Not a route parameter, but didn't match above - fail
        var isRouteParam = RouteParamRegex().Match(routePart);
        if (!isRouteParam.Success)
            return (false, 99);

        var groups = RouteParamRegex().Match(routePart).Groups;
        var type = groups["type"].Value;

        return type switch
        {
            "int" => (int.TryParse(requestUrlPart, out _), 1),
            "uuid" => (Guid.TryParse(requestUrlPart, out _), 1),
            "" => (true, 2),
            _ => (false, 99)
        };
    }
}