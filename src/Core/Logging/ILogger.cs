namespace Core.Logging;

public interface ILogger
{
    void Verbose(string template, params object?[]? values);
    void Information(string template, params object?[]? values);
    void Warning(string template, params object?[]? values);
    void Error(string template, params object?[]? values);
}