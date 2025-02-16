using Core.Models;
using Core.Tests.Fakes;
using Shouldly;
using Xunit;

namespace Core.Tests.RouteParser;

public class RouteParserTests_Request
{
    private readonly FakeLogger _log;
    private readonly Core.RouteParser.RouteParser _parser;

    public RouteParserTests_Request()
    {
        _log = new FakeLogger();
        _parser = new Core.RouteParser.RouteParser(_log);
    }
    
    [Fact]
    public void Adds_Filename_To_Routes()
    {
        var route = _parser.Parse("{}", @"Database\TestFiles\_empty.route.json");

        route!.RouteFileName.ShouldBe(@"Database\TestFiles\_empty.route.json");
    }
    
    [Fact]
    public void Loads_Request_Body()
    {
        var json = @"
{
  ""request"": {
    ""httpMethod"": ""POST"",
    ""path"": ""/api/json"",
  }
}";
        
        var route = _parser.Parse(json, string.Empty);

        route.ShouldNotBeNull();
        route.Request.HttpMethod.ShouldBe("POST");
        route.Request.Path.ShouldBe("/api/json");
    }
    
    [Fact]
    public void Loads_Request_Headers()
    {
        var json = @"
{
  ""request"": {
    ""headers"": {
      ""Content-Type"": ""application/json"",
      ""x-some-header"": ""some-value""
    }
  }
}";
        
        var route = _parser.Parse(json, string.Empty);
        
        route!.Request.Headers.Count.ShouldBe(2);
        route.Request.Headers["Content-Type"].ShouldBe("application/json");
        route.Request.Headers["x-some-header"].ShouldBe("some-value");
    }
    
    [Fact]
    public void No_Request_Headers_Returns_Empty_Dictionary()
    {
        var json = @"
{
 ""request"": {
  }
}";
        
        var route = _parser.Parse(json, string.Empty);
        
        route!.Request.Headers.Count.ShouldBe(0);
    }
    
    
    [Fact]
    public void No_Request_Method_Returns_Get()
    {
        var json = @"
{
 ""request"": {
  }
}";
        
        var route = _parser.Parse(json, string.Empty);
        
        route!.Request.HttpMethod.ShouldBe("GET");
    }
    
}