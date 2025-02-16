using Core.Models;
using Core.RequestFilters;
using Core.RouteModels;
using Shouldly;
using Xunit;

namespace Core.Tests.RequestFilters;

public class HttpMethodFilterTests
{
    private readonly HttpMethodFilter _filter = new();
    private readonly List<Route> _routes = new();

    [Fact]
    public void Returns_Methods_With_Correct_Method_Only()
    {
        var get1 = Route("Get");
        var get2 = Route("GET");
        _routes.AddRange([
            get1,
            get2,
            Route("Post"),
            Route("Put"),
            Route("Delete"),
        ]);

        var request = Request("get");
        var result = _filter.Filter(_routes, request);
        
        result.Count.ShouldBe(2);
        result.ShouldContain(get1);
        result.ShouldContain(get2);
    }

    private Route Route(string httpMethod) => new()
        { Request = new RouteRequest { HttpMethod = httpMethod, Path = "/api/foo" } };
    
    private HttpRequest Request(string httpMethod) => 
        new(httpMethod, "/api/foo", "", new Dictionary<string, string>());
}