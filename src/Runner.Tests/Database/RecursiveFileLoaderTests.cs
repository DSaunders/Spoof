using System.Reflection;
using Core.RouteModels;
using Core.Tests.Fakes;
using Runner.Database;
using Runner.Tests.Fakes;
using Shouldly;
using Xunit;

namespace Runner.Tests.Database;

public class RecursiveFileLoaderTests
{
    private FakeLogger _log;
    private RecursiveFileLoader _loader;
    private readonly string _thisFolder;
    private readonly FakeRouteParser _parser;

    public RecursiveFileLoaderTests()
    {
        _log = new FakeLogger();
        _parser = new FakeRouteParser();
        _loader = new RecursiveFileLoader(_log, _parser);
        _thisFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
    }
  
    
    [Fact]
    public void Passes_Each_Route_To_Parser()
    {
        _loader.LoadRoutes(_thisFolder);

        _parser.ParseCalls.ShouldContain(@"Database\TestFiles\_test.route.json");
    }

    [Fact]
    public void Logs_Each_Route()
    {
        _loader.LoadRoutes(_thisFolder);

        _log.ShouldHaveLoggedVerbose(@"Loading route from file Database\TestFiles\_test.route.json");
    }
    
    [Fact]
    public void Only_Load_Routes_With_Correct_Extension()
    {
        _loader.LoadRoutes(_thisFolder);

        _parser.ParseCalls.ShouldNotContain(@"Database\TestFiles\_dont-load.foo.json");
    }
    
    [Fact]
    public void Returns_Routes()
    {
        var routeQueue = new List<Route?> { new(), new() };
        _parser.RoutesToReturn.Enqueue(routeQueue[0]);
        _parser.RoutesToReturn.Enqueue(null);
        _parser.RoutesToReturn.Enqueue(routeQueue[1]);
        
        var route = _loader.LoadRoutes(_thisFolder);

        route.Count.ShouldBe(2);
        route[0].ShouldBe(routeQueue[0]);
        route[1].ShouldBe(routeQueue[1]);
    }
}