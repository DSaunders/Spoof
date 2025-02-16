using Runner.Logging;
using Runner.Scaffolding;

namespace Runner.Tests.Fakes;

public class FakeTableLogger : ITableLogger
{
    public string Col1Header { get; set; } = string.Empty;
    public string Col2Header { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Footer { get; set; } = string.Empty;
    public Dictionary<string,string> Data { get; set; } = new();
    
    public void LogTable(string title, string footer, string col1, string col2, Dictionary<string, string> data)
    {
        Title = title;
        Footer = footer;
        Data = data;
        Col1Header = col1;
        Col2Header = col2;
    }

}