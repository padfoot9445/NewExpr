using Common.Logger;
namespace MEXPTests.ParserTest;
class MockLogger : ILogger
{
    public List<string> LogRecord { get; } = new();
    public void Log(string message)
    {
        LogRecord.Add(message);
    }

    public void FlushBuffer()
    {
        throw new NotImplementedException();
    }

    public ILogger Copy()
    {
        throw new NotImplementedException();
    }

    public bool LoggingEnabled { get; set; } = false;
}