using Core.Logging;
using Shouldly;

namespace Core.Tests.Fakes;

public class FakeLogger : ILogger
{
    private readonly List<string> _informationCalls = new();
    private readonly List<string> _warningCalls = new();
    private readonly List<string> _verboseCalls = new();
    private readonly List<string> _errorCalls = new();

    public void Verbose(string template, params object?[]? values)
    {
        _verboseCalls.Add(string.Format(template, values ?? []));
    }
    
    public void Information(string template, params object?[]? values)
    {
        _informationCalls.Add(string.Format(template, values ?? []));
    }

    public void Warning(string template, params object?[]? values)
    {
        _warningCalls.Add(string.Format(template, values ?? []));
    }

    public void Error(string template, params object?[]? values)
    {
        _errorCalls.Add(string.Format(template, values ?? []));
    }

    public void ShouldHaveLoggedVerbose(string message) => _verboseCalls.ShouldContain(x => x == message);
    public  void ShouldHaveLoggedWarning(string message) => _warningCalls.ShouldContain(x => x == message);
    public  void ShouldHaveLoggedInformation(string message) => _informationCalls.ShouldContain(x => x == message);
    public  void ShouldHaveLoggedError(string message) => _errorCalls.ShouldContain(x => x == message);
}