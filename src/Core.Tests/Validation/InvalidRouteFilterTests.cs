using Core.RouteModels;
using Core.Tests.Fakes;
using Core.Validation;
using Shouldly;
using Xunit;

namespace Core.Tests.Validation;

public class InvalidRouteFilterTests
{
    private readonly FakeLogger _logger = new();
    private readonly InvalidRouteFilter _filter;

    public InvalidRouteFilterTests()
    {
        _filter = new InvalidRouteFilter(_logger);
    }

    [Fact]
    public void Removes_Bad_Routes_And_Logs()
    {
        var routes = new List<Route>
        {
            new() { Request = new RouteRequest { HttpMethod = "get", Path = "/valid1" } },
            new() { Request = new RouteRequest { HttpMethod = "get", Path = "/valid2" } },
            new()
            {
                RouteFileName = "foo.json",
                Request = new RouteRequest
                {
                    HttpMethod = string.Empty,
                    Path = "/invalid"
                }
            },
        };

        var result = _filter.Filter(routes);

        result.Count.ShouldBe(2);
        result[0].Request.Path.ShouldBe("/valid1");
        result[1].Request.Path.ShouldBe("/valid2");
        _logger.ShouldHaveLoggedError("Route foo.json does not contain a request.httpMethod property");
        _logger.ShouldHaveLoggedError("Route foo.json will be ignored");
    }
}