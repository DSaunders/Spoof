using System.Diagnostics;
using System.Text.Json.Serialization;
using Core.Models;
using Newtonsoft.Json.Linq;

namespace Core.RouteModels;

// When adding new properties to this class:
// - Tests for loading it in 'RouteParser.cs' (+ tests for any default value)
// - (if required) Verify behaviour with tests in 'RouteTests.cs'
// - (if required) Modify program.cs to get/set the value from HttpContext
// - (if required) Add a new route filter and add to 'Handler.cs' 

[DebuggerDisplay("Route: {Request.HttpMethod} {Request.Path}")]
public class Route
{
    [JsonIgnore]
    public string RouteFileName { get; set; } = string.Empty;
    
    public RouteRequest Request { get; set; } = new();

    public RouteResponse Response { get; set; } = new();

    public virtual Response Handle(HttpRequest httpHttpRequest)
    {
        var responseBody = DoReplacements(
            httpHttpRequest, 
            Response.Body.ToString()!
        );
        
        return new Response(Response.StatusCode, responseBody, Response.Headers);
    }

    private string DoReplacements(HttpRequest httpHttpRequest, string responseBody)
    {
        // Response is a text string, simple token replacement
        if (Response.Body is not JsonBody) 
            return responseBody.Replace("{{request.body}}", httpHttpRequest.Body);
        
        // If the request is JSON, tokens will be in quotes "{{token}}"
        // We need to remove the surrounding "" so we can correctly 'merge' the JSON 
        //  and not stuff it into a string
        var tokenToReplace = httpHttpRequest.IsJsonBody
            ? "\"{{request.body}}\""
            : "{{request.body}}";

        responseBody = responseBody.Replace(tokenToReplace, httpHttpRequest.Body);
        responseBody = JToken.Parse(responseBody).ToString();
        
        return responseBody;
    }
}