using Core.Models;
using Core.RouteModels;
using Core.Tests.Fakes;
using Shouldly;
using Xunit;

namespace Core.Tests;

public class HandlerTests
{
    private readonly Handler _handler;
    private readonly FakeLogger _logger;
    private readonly List<Route> _routes = new();

    public HandlerTests()
    {
        _logger = new FakeLogger();
        _handler = new Handler(_logger, _routes);
    }

    [Fact]
    public void Calls_Route_Mappers()
    {
        _routes.AddRange([
            CreateRoute("get", "/api/v1/users", "wrong route"),
            CreateRoute("post", "/api/test", "wrong method"),
            CreateRoute("get", "/api/test", "right")
        ]);

        var request = new HttpRequest("get", "/api/test", "", new Dictionary<string, string>());
        var response = _handler.HandleRequest(request);

        response.Body.ShouldBe("right");
    }

    [Fact]
    public void Logs_When_Trying_To_Match_Route()
    {
        var request = new HttpRequest("get", "/api/test", "", new Dictionary<string, string>());
        
        _handler.HandleRequest(request);

        _logger.ShouldHaveLoggedInformation("Received request GET to /api/test");
    }
    
    [Fact]
    public void Logs_When_Route_Not_Matched()
    {
        var request = new HttpRequest("get", "/api/test", "", new Dictionary<string, string>());
        
        _handler.HandleRequest(request);

        _logger.ShouldHaveLoggedWarning("Did not find a mock for GET /api/test. Returning a 404 response");
    }
    
    [Fact]
    public void Logs_When_Route_Matched()
    {
        _routes.Add(CreateRoute("get", "/api/test", "will match"));
        var request = new HttpRequest("get", "/api/test", "", new Dictionary<string, string>());
        
        _handler.HandleRequest(request);

        _logger.ShouldHaveLoggedInformation("Matched a route for GET /api/test. Returning a response");
    }
    
    [Fact]
    public void Returns_Not_Found_Response_When_No_Route_Found()
    {
        var request = new HttpRequest("get", "/api/test", "", new Dictionary<string, string>());
        
        var response = _handler.HandleRequest(request);

        response.StatusCode.ShouldBe(404);
        response.Body.ShouldBe(string.Empty);
    }
    

    private Route CreateRoute(string method, string path, string response) =>
        new FakeRoute(method, path, response);
}