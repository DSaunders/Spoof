using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.RegularExpressions;
using ILogger = Core.Logging.ILogger;

namespace Runner.Logging;

[ExcludeFromCodeCoverage]
public class ConsoleLogger : ILogger
{
    public ConsoleLogger()
    {
        Console.OutputEncoding = Encoding.UTF8;
    }
    
    internal bool VerboseMode { get; set; } = true;

    public void Verbose(string template, params object?[]? values)
    {
        if (!VerboseMode)
            return;
        
        Console.ForegroundColor = ConsoleColor.DarkGray;
        PrintMessage(template, ConsoleColor.DarkGray, values);
    }

    public void Information(string template, params object?[]? values)
    {
        Console.ResetColor();
        PrintMessage(template, ConsoleColor.Cyan, values);
    }

    public void Warning(string template, params object?[]? values)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        PrintMessage(template, ConsoleColor.Yellow, values);
        Console.ResetColor();
    }

    public void Error(string template, params object?[]? values)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        PrintMessage(template, ConsoleColor.DarkRed, values);
        Console.ResetColor();
    }

    private void PrintMessage(string message, ConsoleColor paramColour, params object?[]? values)
    {
        var originalColour = Console.ForegroundColor;
        if (values == null || values.Length == 0)
        {
            Console.WriteLine(message);
            return;
        }

        var parts = Regex.Split(message, @"(\{[^}]*\})");

        var paramIndex = 0;
        foreach (var part in parts)
        {
            if (!part.StartsWith("{"))
            {
                Console.Write(part);
                continue;
            }

            var partValue = values[paramIndex++];

            Console.ForegroundColor = paramColour;
            Console.Write(partValue);
            Console.ForegroundColor = originalColour;
        }

        Console.WriteLine();
    }
}