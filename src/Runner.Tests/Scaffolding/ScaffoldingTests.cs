using Core.Tests.Fakes;
using Runner.Scaffolding;
using Runner.Tests.Fakes;
using Shouldly;
using Xunit;

namespace Runner.Tests.Scaffolding;

public class ScaffoldingTests
{
    private readonly FakeLogger _logger = new();
    private readonly FakeTableLogger _tableLogger = new();
    
    private readonly Scaffolder _scaffold;

    public ScaffoldingTests()
    {
        _scaffold = new Scaffolder(_logger, _tableLogger);
    }
    
    [Fact]
    public void Lists_All_Templates()
    {
        _scaffold.ListTemplates();
        
        _tableLogger.Title.ShouldBe("The following scaffolding templates are available:");
        _tableLogger.Footer.ShouldBe("For example:\r\n  'spoof scaffold hello-world'");
        
        _tableLogger.Col1Header.ShouldBe("Template Name");
        _tableLogger.Col2Header.ShouldBe("Description");
        _tableLogger.Data.ShouldContain(v =>
            v.Key == "hello-world" &&
            v.Value == "Responds to all GET /api/hello requests with a greeting"
        );
    }

    [Fact]
    public void Scaffolds_A_Template()
    {
        var source = "hello-world";
        var destination = "test-output";
        
        if (File.Exists($"{destination}.route.json"))
            File.Delete($"{destination}.route.json");

        var fileName = _scaffold.Scaffold(source, destination);
        
        File.Exists($"{destination}.route.json").ShouldBeTrue();
        var route = File.ReadAllText($"{destination}.route.json");
        route.ShouldContain("/api/hello");
        
        _logger.ShouldHaveLoggedInformation("Created new mock file: test-output.route.json");
        fileName.ShouldBe("test-output.route.json");
    }
    
    [Fact]
    public void Logs_When_Source_Template_Does_Not_Exist()
    {
        var source = "wrong-template";
        var fileName = _scaffold.Scaffold(source, "foo");

        fileName.ShouldBeEmpty();
        _logger.ShouldHaveLoggedWarning(
            "Could not find scaffolding template wrong-template. Run 'spoof scaffold list' to show available templates"
        );
    }
}