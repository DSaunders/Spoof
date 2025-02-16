using System.Diagnostics.CodeAnalysis;
using Core.Models;
using Core.RouteModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core.RouteParser;

public class RouteResponseJsonParser : JsonConverter
{
    public override bool CanConvert(Type objectType) => objectType == typeof(RouteResponse);

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue,
        JsonSerializer serializer)
    {
        var jo = JObject.Load(reader);

        var statusCode = jo["statusCode"] is null
            ? 200
            : (int)jo["statusCode"]!;

        var headers = jo["headers"] is null
            ? new Dictionary<string, string>()
            : jo["headers"]!.ToObject<Dictionary<string, string>>()!;

        var response = new RouteResponse
        {
            StatusCode = statusCode,
            Headers = headers
        };

        var body = SetBody(jo);
        if (body is not null)
            response.Body = body;
        
        return response;
    }


    private static object? SetBody(JObject jo)
    {
        // No body
        if (jo["body"] is null)
            return null;

        // String body
        if (jo["body"]!.Type == JTokenType.String)
            return (string)jo["body"]!;

        // JSON body
        return (JsonBody)jo["body"]!;
    }

    [ExcludeFromCodeCoverage]
    public override bool CanWrite => false;
    
    [ExcludeFromCodeCoverage]
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
    }
}