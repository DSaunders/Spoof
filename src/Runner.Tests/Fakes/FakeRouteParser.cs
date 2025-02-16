using Core.RouteModels;
using Core.RouteParser;
using Runner.Database;

namespace Runner.Tests.Fakes;

public class FakeRouteParser :IRouteParser {
    
    public Queue<Route?> RoutesToReturn { get; } = new();
    
    public List<string> ParseCalls { get; } = new();
    
    public Route? Parse(string contents, string filename) {
        ParseCalls.Add(filename);
        
        return RoutesToReturn.Count != 0 ? RoutesToReturn.Dequeue() : null;
    }
}