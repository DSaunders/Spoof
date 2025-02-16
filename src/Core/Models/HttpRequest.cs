using System.Text.RegularExpressions;

namespace Core.Models;

public partial record HttpRequest(
    string HttpMethod,
    string Path,
    string Body,
    Dictionary<string, string> Headers
)
{
    public bool IsJsonBody => BasicJsonCheck().IsMatch(Body);

    [GeneratedRegex(@"(\A{(.|\n)+}\Z)|(\A\[(.|\n)+\]\Z)")]
    private static partial Regex BasicJsonCheck();
};