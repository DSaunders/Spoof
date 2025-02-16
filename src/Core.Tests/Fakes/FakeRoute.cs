using Core.Models;
using Core.RouteModels;

namespace Core.Tests.Fakes;

internal class FakeRoute : Route
{
    private readonly string _response;

    public FakeRoute(string httpMethod, string path, string response)
    {
        _response = response;
        Request = new RouteRequest { HttpMethod = httpMethod, Path = path };
    }
    
    public override Response Handle(HttpRequest httpHttpRequest) => 
        new(200, _response, new Dictionary<string, string>());
}