using Core.RouteModels;
using Core.Validation;
using Shouldly;
using Xunit;

namespace Core.Tests.Validation;

public class RouteValidatorTests
{
    private RouteValidator _validator = new();
    
    [Fact]
    public void No_Route_Returns_Error()
    {
        var route = new Route();
    
        _validator.Validate(route).ShouldContain(
            "does not contain a request.path property"
        );
    }
   
    [Fact]
    public void Root_Route_Returns_Error()
    {
        var route = new Route { Request = new RouteRequest { Path = "/" } };
    
        _validator.Validate(route).ShouldContain(
            "specifies a mock API at the root ('/')"
        );
    }
    
    [Fact]
    public void Root_With_Other_Slashes_Route_Returns_Error()
    {
        var route1 = new Route { Request = new RouteRequest { Path = @"\" } };
        var route2 = new Route { Request = new RouteRequest { Path = @"\\" } };
        var route3 = new Route { Request = new RouteRequest { Path = "/" } };
        var route4 = new Route { Request = new RouteRequest { Path = "///" } };

        var results = _validator.Validate(route1)
            .Concat(_validator.Validate(route2))
            .Concat(_validator.Validate(route3))
            .Concat(_validator.Validate(route4));

        results.Count(v => v == "specifies a mock API at the root ('/')").ShouldBe(4);
    }
}