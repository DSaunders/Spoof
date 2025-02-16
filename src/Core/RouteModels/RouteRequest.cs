namespace Core.RouteModels;

public class RouteRequest()
{
    public string HttpMethod { get; set;  } = "GET";
    public string Path { get; set; } = string.Empty;
    public Dictionary<string,string> Headers { get; set; } = new();
}