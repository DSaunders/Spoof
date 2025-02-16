using Core.Models;
using Core.RequestFilters;
using Core.RouteModels;
using Shouldly;
using Xunit;

namespace Core.Tests.RequestFilters;

public class RequestHeadersFilterTests
{
    private readonly RequestHeadersFilter _filter = new();
    private readonly List<Route> _routes = new();

    [Fact]
    public void Ignores_Route_Unless_All_Headers_Match()
    {
        var request = Request(new Dictionary<string, string>
        {
            { "Content-Type", "application/json" },
            { "x-custom", "testing-123" }
        });
        
        
        var noHeaders = new Dictionary<string, string>();
        
        var exactMatch = new Dictionary<string, string>
        {
            { "Content-Type", "application/json" },
            { "x-custom", "testing-123" }
        };
        var requestHasExtraHeaderNotMentioned = new Dictionary<string, string>
        {
            { "Content-Type", "application/json" }
        };
          
        var bothHeadersWrong = new Dictionary<string, string>
        {
            { "Content-Type", "text/plain" },
            { "x-custom", "testing-999" }
        };
          
        var oneHeadersWrong = new Dictionary<string, string>
        {
            { "Content-Type", "text/plain" },
            { "x-custom", "testing-123" }
        };
        
        var requestIsMissingHeader = new Dictionary<string, string>
        {
            { "Content-Type", "application/json" },
            { "x-custom", "testing-123" },
            { "something", "else" }
        };
      
        
        _routes.AddRange(new[]
        {
            Route(noHeaders),
            Route(exactMatch),
            Route(requestHasExtraHeaderNotMentioned),
            Route(bothHeadersWrong),
            Route(oneHeadersWrong),
            Route(requestIsMissingHeader)
        });

        var result = _filter.Filter(_routes, request);
        
        result.Count.ShouldBe(3);
        result.ShouldContain(route => route.Request.Headers == noHeaders);
        result.ShouldContain(route => route.Request.Headers == exactMatch);
        result.ShouldContain(route => route.Request.Headers == requestHasExtraHeaderNotMentioned);
    }

    private Route Route(Dictionary<string, string> headers) => new()
    {
        Request = new RouteRequest
        {
            HttpMethod = "get",
            Path = "/api/foo",
            Headers = headers
        }
    };

    private HttpRequest Request(Dictionary<string, string> headers) =>
        new("get", "/api/foo", "", headers);
}