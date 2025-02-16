using Core.Models;
using Core.RequestFilters;
using Core.RouteModels;
using Shouldly;
using Xunit;

namespace Core.Tests.RequestFilters;

public class PathFilterTests
{
    private readonly PathFilter _filter = new();
    private readonly List<Route> _routes = new();

    [Fact]
    public void Matches_Simple_Routes()
    {
        var request = Request("/api/values");
        _routes.Add(Route("/api/values"));
        
        var result = _filter.Filter(_routes, request); 
        
        result.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Ignores_Case_When_Matching_Routes()
    {
        var request = Request("/aPi/vAluEs");
        _routes.Add(Route("/api/values"));
        
        var result = _filter.Filter(_routes, request); 
        
        result.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Matches_Long_Simple_Routes()
    {
        var request = Request("/api/one/two/three/four");
        _routes.Add(Route("/api/one/two/three/four"));
        
        var result = _filter.Filter(_routes, request); 
        
        result.Count.ShouldBe(1);
    }

    [Fact]
    public void Matches_Multiple_Matching_Routes()
    {
        var request = Request("/api/values");
        _routes.Add(Route("/api/values"));
        _routes.Add(Route("/api/values"));
        
        var result = _filter.Filter(_routes, request); 
        
        result.Count.ShouldBe(2);
    }

    [Fact]
    public void Only_Returns_Matching_Simple_Routes()
    {
        var request = Request("/api/values");
        _routes.Add(Route("/api/values"));
        _routes.Add(Route("/api/values/childRoute"));
        _routes.Add(Route("/api/wrongRoute"));
        
        var result = _filter.Filter(_routes, request); 
        
        result.Count.ShouldBe(1);
        result[0].Request.Path.ShouldBe("/api/values");
    }

    [Fact]
    public void Matches_Route_With_Parameter()
    {
        var request = Request("/api/values");
        _routes.Add(Route("/api/values"));
        _routes.Add(Route("/api/values"));
        
        var result = _filter.Filter(_routes, request); 
        
        result.Count.ShouldBe(2);
    }
    
    [Fact]
    public void Matches_Route_With_AnythingParameter()
    {
        var request = Request("/api/foods/fruits/apples");
        _routes.Add(Route("/api/foods/{type}/apples"));
        
        var result = _filter.Filter(_routes, request); 
        
        result.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Matches_Route_With_Anything_Parameter_And_Fixed_Route()
    {
        var request = Request("/api/foods/fruits/apples");
        _routes.Add(Route("/api/foods/{type}/apples"));
        _routes.Add(Route("/api/foods/fruits/apples"));
        
        var result = _filter.Filter(_routes, request); 
        
        result.Count.ShouldBe(2);
    }
    
    [Fact]
    public void Matches_Route_With_Multiple_Anything_Parameters()
    {
        var request = Request("/api/foods/fruits/apples");
        _routes.Add(Route("/api/{thing}/{type}/apples"));
        
        var result = _filter.Filter(_routes, request); 
        
        result.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Matches_Route_With_Integer_Parameters()
    {
        var request = Request("/api/foods/1/apples");
        _routes.Add(Route("/api/foods/{id:int}/apples"));
        
        var result = _filter.Filter(_routes, request); 
        
        result.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Does_Not_Match_String_For_Integer_Parameters()
    {
        var request = Request("/api/foods/one/apples");
        _routes.Add(Route("/api/foods/{id:int}/apples"));
        
        var result = _filter.Filter(_routes, request); 
        
        result.Count.ShouldBe(0);
    }
    
   [Fact]
    public void Matches_Route_With_GUID_Parameters()
    {
        var request = Request("/api/foods/E34E1B87-4E54-4572-B0C9-FC5C35B8731D/apples");
        _routes.Add(Route("/api/foods/{id:uuid}/apples"));
        
        var result = _filter.Filter(_routes, request); 
        
        result.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Does_Not_Match_String_For_GuId_Parameters()
    {
        var request = Request("/api/foods/one/apples");
        _routes.Add(Route("/api/foods/{id:uuid}/apples"));
        
        var result = _filter.Filter(_routes, request); 
        
        result.Count.ShouldBe(0);
    }
    
    [Fact]
    public void Matches_Parameters_Of_Multiple_Types()
    {
        var request = Request("/api/foods/1/apples/13FADFBE-C6EC-4550-AA77-E0C6E4AC3CF0");
        _routes.Add(Route("/api/{anything}/{id:int}/apples/{id:uuid}"));
        
        var result = _filter.Filter(_routes, request); 
        
        result.Count.ShouldBe(1);
    }

    [Fact]
    public void Returns_Routes_In_Correct_Order_Simple_Scoring()
    {
        var request = Request("/api/foods/1/apples");
        
        _routes.Add(Route("/api/foods/{id:int}/apples"));
        _routes.Add(Route("/api/foods/1/apples"));
        _routes.Add(Route("/api/foods/{one}/apples"));
        
        var result = _filter.Filter(_routes, request); 
        
        result[0].Request.Path.ShouldBe("/api/foods/1/apples"); // Exact match 1st (1 point)
        result[1].Request.Path.ShouldBe("/api/foods/{id:int}/apples"); // Types parameter 2nd (2 points)
        result[2].Request.Path.ShouldBe("/api/foods/{one}/apples"); // Anything parameter 3rd (3 points)
    }
    
    
    [Fact]
    public void Returns_Routes_In_Correct_Order_Simple_Scoring_1()
    {
         var request = Request("/1/2/3/4");

        _routes.Add(Route("/1/2/3/{x}")); // (0,0.0,2) = 2 points
        _routes.Add(Route("/1/{x:int}/{y:int}/{x:int}")); // (0,1,1,1) = 3 points
        
        var result = _filter.Filter(_routes, request); 
        
        result[0].Request.Path.ShouldBe("/1/2/3/{x}");
        result[1].Request.Path.ShouldBe("/1/{x:int}/{y:int}/{x:int}");
    }
    
    [Fact]
    public void Does_Not_Match_Variables_With_Unknown_Type()
    {
        var request = Request("/api/foods/one/apples");
        _routes.Add(Route("/api/foods/{id:bloop}/apples"));
        
        var result = _filter.Filter(_routes, request); 
        
        result.Count.ShouldBe(0);
    }

    private Route Route(string path) => new() 
        { Request = new RouteRequest { HttpMethod = "get", Path = path } };
    
    private HttpRequest Request(string path) => 
        new("Get", path, "", new Dictionary<string, string>());
}