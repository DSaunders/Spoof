namespace Runner.Logging;

public interface ITableLogger
{
    public void LogTable(
        string title, 
        string footer,
        string col1Header,
        string col2Header,
        Dictionary<string,string> table
    );
}