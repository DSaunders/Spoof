using Core.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shouldly;
using Xunit;

namespace Core.Tests.Models;

public class JsonBodyTests
{
    [Fact]
    public void Loads_Routes_With_JSON_Response_Value_Properties()
    {
        var body = GetJsonBody(new
        {
            foo = "bar",
            baz = 42,
            active = true,
            floaty = 3.14,
            datetime = "2012-03-19T07:22:00Z",
            id = Guid.Parse("173a3867-792a-49cf-b1ae-84630d7af619")
        });

        body["foo"].ShouldBe("bar");
        body["baz"].ShouldBe(42);
        body["active"].ShouldBe(true);
        Math.Abs((float)body["floaty"] - 3.14).ShouldBeLessThan(0.00001);
        body["datetime"].ShouldBe(DateTime.Parse("2012-03-19T07:22:00Z"));
        body["id"].ShouldBe("173a3867-792a-49cf-b1ae-84630d7af619");
    }
    
    [Fact]
    public void Unsupported_Properties_Are_Mapped_To_String_Empty()
    {
        var body = GetJsonBody(new
        {
            foo = default(string) // null JToken type
        });

        body["foo"].ShouldBe(string.Empty);
    }

    [Fact]
    public void Returns_JSON_String()
    {
        var body = GetJsonBody(new
        {
            baz = 42
        });

        body.ToString().ShouldBe(
            JsonConvert.SerializeObject(new
                {
                    baz = 42
                },
                Formatting.Indented
            ));
    }

    [Fact]
    public void Loads_Routes_With_JSON_Response_Nested_Properties()
    {
        var body = GetJsonBody(new
        {
            nest = new
            {
                child = "child value"
            }
        });

        body["nest.child"].ShouldBe("child value");
    }

    [Fact]
    public void Loads_Routes_With_JSON_Response_Nested_String_Arrays()
    {
        var body = GetJsonBody(new
        {
            nest = new
            {
                simpleArrayStrings = new[] { "simpleString1", "simpleString2" }
            }
        });

        body["nest.simpleArrayStrings.0"].ShouldBe("simpleString1");
        body["nest.simpleArrayStrings.1"].ShouldBe("simpleString2");
    }

    [Fact]
    public void Loads_Routes_With_JSON_Response_Nested_Int_Arrays()
    {
        var body = GetJsonBody(new
        {
            nest = new
            {
                simpleArrayInts = new[] { 1, 5 }
            }
        });

        body["nest.simpleArrayInts.0"].ShouldBe(1);
        body["nest.simpleArrayInts.1"].ShouldBe(5);
    }

    [Fact]
    public void Loads_Routes_With_JSON_Response_Nested_Object_Arrays()
    {
        var body = GetJsonBody(new
        {
            nest = new
            {
                complexArray = new[]
                {
                    new { name = "name1", value = "value1" },
                    new { name = "name2", value = "value2" }
                }
            }
        });

        body["nest.complexArray.0.name"].ShouldBe("name1");
        body["nest.complexArray.0.value"].ShouldBe("value1");
        body["nest.complexArray.1.name"].ShouldBe("name2");
        body["nest.complexArray.1.value"].ShouldBe("value2");
    }

    [Fact]
    public void Indexing_Off_End_Of_Array_Returns_Empty_String()
    {
        var body = GetJsonBody(new
        {
            nest = new
            {
                simpleArrayInts = new[] { 1 }
            }
        });

        body["nest.simpleArrayInts.1"].ShouldBe(string.Empty);
    }

    [Fact]
    public void Indexing_Incorrect_Property_Returns_Empty_String()
    {
        var body = GetJsonBody(new
        {
            a = new
            {
                b = "foo"
            }
        });

        body["a.f.d"].ShouldBe(string.Empty);
    }

    [Fact]
    public void Indexing_Off_End_Of_Property_Chain_Returns_Empty_String()
    {
        var body = GetJsonBody(new
        {
            a = new
            {
                b = "foo"
            }
        });

        body["a.b.c"].ShouldBe(string.Empty);
    }

    [Fact]
    public void Indexing_Earlier_Into_The_Object_Serialises_The_Rest()
    {
        var body = GetJsonBody(new
        {
            a = new
            {
                b = new
                {
                    c = "foo"
                }
            }
        });

        body["a"].ShouldBe(
            JsonConvert.SerializeObject(new
                {
                    b = new
                    {
                        c = "foo"
                    }
                },
                Formatting.Indented
            ));
    }

    private JsonBody GetJsonBody(object body)
    {
        return JObject.Parse(
            JsonConvert.SerializeObject(body)
        );
    }
}