using Core.Models;
using Core.RouteModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shouldly;
using Xunit;

namespace Core.Tests.RouteModels;

public class RouteTestsReplacements
{
    [Fact]
    public void Returns_Empty_String_If()
    {
        var route = new Route
        {
            Response = new()
            {
                Body = "Body was {{request.body}}"
            }
        };

        var request = Request("Hello, world!");

        var response = route.Handle(request);

        response.Body.ShouldBe("Body was Hello, world!");
    }

    [Fact]
    public void Replaces_Request_Body_Text_In_Text_Out()
    {
        var route = new Route
        {
            Response = new()
            {
                Body = "Body was {{request.body}}"
            }
        };

        var request = Request("Hello, world!");

        var response = route.Handle(request);

        response.Body.ShouldBe("Body was Hello, world!");
    }

    [Fact]
    public void Replaces_Request_Body_Text_In_Json_Out()
    {
        var route = new Route
        {
            Response = new()
            {
                Body = (JsonBody)(JObject)JToken.Parse("{\"message\": \"{{request.body}}\"}")
            }
        };

        var request = Request("Hello, world!");

        var response = route.Handle(request);

        var expectedResponse = new
        {
            message = "Hello, world!"
        };

        response.Body.ShouldBe(
            JsonConvert.SerializeObject(expectedResponse, Formatting.Indented)
        );
    }

    [Fact]
    public void Replaces_Request_Body_Json_In_Json_Out()
    {
        var route = new Route
        {
            Response = new()
            {
                Body = (JsonBody)(JObject)JToken.Parse("{\"message\": \"{{request.body}}\"}")
            }
        };

        var request = Request(JsonConvert.SerializeObject(new
            {
                Hello = "World",
                Foo = 99
            },
            Formatting.Indented)
        );

        var response = route.Handle(request);

        var expectedResponse = new
        {
            message = new
            {
                Hello = "World",
                Foo = 99
            }
        };
        response.Body.ShouldBe(JsonConvert.SerializeObject(expectedResponse, Formatting.Indented));
    }

    [Fact]
    public void Replaces_Request_Body_Json_In_Text_Out()
    {
        var route = new Route
        {
            Response = new()
            {
                Body = "Body was \"{{request.body}}\""
            }
        };

        var request = Request(JsonConvert.SerializeObject(new
            {
                Hello = "World",
                Foo = 99
            },
            Formatting.Indented)
        );

        var response = route.Handle(request);

        var expectedResponse = "Body was \"{\r\n  \"Hello\": \"World\",\r\n  \"Foo\": 99\r\n}\"";

        response.Body.ShouldBe(expectedResponse);
    }

    private HttpRequest Request(string body) =>
        new("", "", "", new Dictionary<string, string>())
        {
            Body = body
        };
}