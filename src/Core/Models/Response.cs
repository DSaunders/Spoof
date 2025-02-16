namespace Core.Models;

public record Response(int StatusCode, string Body, Dictionary<string,string> Headers);