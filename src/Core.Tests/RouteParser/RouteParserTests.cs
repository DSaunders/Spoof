using Core.Models;
using Core.Tests.Fakes;
using Shouldly;
using Xunit;

namespace Core.Tests.RouteParser;

public class RouteParserTests_Errors
{
    private readonly FakeLogger _log;
    private readonly Core.RouteParser.RouteParser _parser;

    public RouteParserTests_Errors()
    {
        _log = new FakeLogger();
        _parser = new Core.RouteParser.RouteParser(_log);
    }
    
    [Fact]
    public void Logs_Failure_To_Parse_JSON()
    {
        _parser.Parse("", @"Database\TestFiles\_empty.route.json");

        _log.ShouldHaveLoggedError(
            @"Couldn't parse Database\TestFiles\_empty.route.json. Route will be ignored"
        );
    }
    
    [Fact]
    public void Logs_Deserialisation_Errors()
    {
        _parser.Parse("{ \"foo\":", @"Database\TestFiles\_broken.route.json");

        _log.ShouldHaveLoggedError(
            @"Could not parse Database\TestFiles\_broken.route.json: Unexpected end when deserializing object. Path 'foo', line 1, position 8."
        );

        _log.ShouldHaveLoggedError(
            @"File Database\TestFiles\_broken.route.json will be ignored"
        );
    }
}