using System.Text;

namespace Common.Logger;

public class Logger : ILogger
{
    public Logger(TextWriter writer)
    {
        Writer = writer;
    }

    public Logger()
    {
        Writer = Console.Out;
    }

    public TextWriter Writer { get; init; }
    public StringBuilder Buffer { get; init; } = new();

    public bool LoggingEnabled { get; set; }

    public void Log(string message)
    {
        if (!LoggingEnabled) return;
        Buffer.AppendLine(message);
    }

    public void FlushBuffer()
    {
        Writer.WriteLine(Buffer.ToString());
        Buffer.Clear();
    }

    public ILogger Copy()
    {
        return new Logger { Writer = Writer, Buffer = Buffer, LoggingEnabled = LoggingEnabled };
    }
}