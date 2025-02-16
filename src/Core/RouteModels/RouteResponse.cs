namespace Core.RouteModels;

public class RouteResponse
{
    public int StatusCode { get; set; } = 200;
    public Dictionary<string, string> Headers { get; set; } = new();
    public object Body { get; set; } = string.Empty;
}