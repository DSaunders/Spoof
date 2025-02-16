using Core.Models;
using Core.RouteModels;
using Shouldly;
using Xunit;

namespace Core.Tests.RouteModels;

public class RouteTests
{
    [Fact]
    public void Route_Filename_Is_Empty_By_Default()
    {
        // We shouldn't hit this, because the parser should always set the filename
        // This checks it's not null to avoid any potential null ref exceptions if I've missed anything
        var route = Route("get", "/api/foo");
        route.RouteFileName.ShouldBeEmpty();
    }
  
    [Fact]
    public void Returns_Body_And_StatusCode()
    {
        var route = Route("get", "/api/foo");
        route.Response = new RouteResponse
        {
            Body = "Hello, world!",
            StatusCode = 201
        };

        var request = new HttpRequest("get", "/api/foo", "", new Dictionary<string, string>());

        var response = route.Handle(request);

        response.StatusCode.ShouldBe(201);
        response.Body.ShouldBe("Hello, world!");
    }

    [Fact]
    public void Returns_Headers()
    {
        var route = Route("get", "/api/foo");
        route.Response = new RouteResponse
        {
            Headers = new() { { "Content-Type", "text/plain" } }
        };

        var request = new HttpRequest("get", "/api/foo", "", new Dictionary<string, string>());

        var response = route.Handle(request);

        response.Headers.ShouldContainKeyAndValue("Content-Type", "text/plain");
    }
    
    private Route Route(string method, string path) => new()
    {
        Request = new RouteRequest { HttpMethod = method, Path = path }
    };
}