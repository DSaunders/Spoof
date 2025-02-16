using System.Diagnostics.CodeAnalysis;
using Runner.Scaffolding;

namespace Runner.Logging;

[ExcludeFromCodeCoverage]
public class ConsoleTableLogger : ITableLogger
{
    private ConsoleColor LineColour => ConsoleColor.DarkGray;
    
    public void LogTable(string title, string footer, string col1Header, string col2Header, Dictionary<string, string> table)
    {
        Console.WriteLine();
        Console.WriteLine(title);
        Console.WriteLine();
        
        // Get max column widths
        var col1Size  = table.Keys.Max(k => k.Length);
        var col2Size = table.Values
            .SelectMany(v => v.Split("\n"))
            .Max(l => l.Length + 1);
        
        WriteRow(col1Size, col1Header, col2Header, isHeader: true);
        WriteSeparator(col1Size, col2Size);
        foreach (var (key, value) in table)
            WriteRow(col1Size, key, value);
        
        Console.WriteLine();
        Console.WriteLine(footer);
        Console.WriteLine();
    }

    private void WriteRow(int col1Size, string col1, string col2, bool isHeader = false)
    {
        if (isHeader)
            Console.ForegroundColor = LineColour;
        
        var col2Lines = col2.Split("\n");
        var isLine1 = true;
        foreach (var line in col2Lines)
        {
            var col1Text = isLine1 ? col1 : string.Empty;
            Console.Write($"  {col1Text.PadRight(col1Size)} ");
            
            Console.ForegroundColor = LineColour;
            Console.Write("| ");
            if(!isHeader)
                Console.ResetColor();
            
            Console.WriteLine(line);
            isLine1 = false;
        }
        
        Console.ResetColor();
    }

    private void WriteSeparator(int col1Length, int col2Length)
    {
        Console.ForegroundColor = LineColour;
        Console.WriteLine($"  {new string('-', col1Length)}-|-{new string('-', col2Length)}");
        Console.ResetColor();
    }
}