using Core.Models;
using Core.Tests.Fakes;
using Shouldly;
using Xunit;

namespace Core.Tests.RouteParser;

public class RouteParserTests_Response
{
    private readonly FakeLogger _log;
    private readonly Core.RouteParser.RouteParser _parser;

    public RouteParserTests_Response()
    {
        _log = new FakeLogger();
        _parser = new Core.RouteParser.RouteParser(_log);
    }
    
    [Fact]
    public void Loads_Routes_With_Simple_Text_Response()
    {
        var json = @"
{
  ""response"": {
    ""statusCode"": 201,
    ""body"": ""Hello, World!""
  }
}";
        
        var route = _parser.Parse(json, string.Empty);

        route.ShouldNotBeNull();
        route.Response.StatusCode.ShouldBe(201);
        route.Response.Body.ShouldBe("Hello, World!");
    }
    
    [Fact]
    public void Empty_Response_Body_Defaults_To_Empty_String()
    {
        var json = @"
{
 ""response"": {
  }
}";
        
        var route = _parser.Parse(json, string.Empty);
        route!.Response.Body.ShouldBe(string.Empty);
    }

    [Fact]
    public void Loads_Routes_With_Json_Response()
    {
        var json = @"
{
 ""response"": {
    ""statusCode"": 201,
    ""body"": {
      ""foo"": ""bar""
    }
  }
}";
        
        var route = _parser.Parse(json, string.Empty);

        route.ShouldNotBeNull();
        route.Response.StatusCode.ShouldBe(201);
        ((JsonBody)route.Response.Body)["foo"].ShouldBe("bar");
    }
    
    [Fact]
    public void Loads_Response_Headers()
    {
        var json = @"
{
 ""response"": {
    ""headers"": {
      ""Content-Type"": ""application/json""
    }
  }
}";
        
        var route = _parser.Parse(json, string.Empty);

        route!.Response.Headers["Content-Type"].ShouldBe("application/json");
    }
    
    [Fact]
    public void No_Response_Headers_Returns_Empty_Dictionary()
    {
        var json = @"
{
 ""response"": {
  }
}";
        
        var route = _parser.Parse(json, string.Empty);
        
        route!.Response.Headers.Count.ShouldBe(0);
    }
    
    [Fact]
    public void No_Response_Defaults_To_200_OK()
    {
        var json = @"
{
 
}";
        
        var route = _parser.Parse(json, string.Empty);
        
        route!.Response.StatusCode.ShouldBe(200);
    }
    
    [Fact]
    public void No_Response_Code_Defaults_To_200_OK()
    {
        var json = @"
{
 ""response"": {
    ""body"": ""Hello, World!""
  }
}";
        
        var route = _parser.Parse(json, string.Empty);
        
        route!.Response.StatusCode.ShouldBe(200);
    }
}