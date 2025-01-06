using Common.Logger;
namespace ParserTest;
class MockLogger : ILogger
{
    public List<string> LogRecord { get; } = new();
    public void Log(string message)
    {
        LogRecord.Add(message);
    }
}