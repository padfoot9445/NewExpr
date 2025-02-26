using System.Text;

namespace Common.Logger;
public class Logger : ILogger
{
    public TextWriter Writer { get; init; }

    public bool LoggingEnabled { get; set; } = false;
    public StringBuilder Buffer { get; init; } = new();

    public Logger(TextWriter writer)
    {
        this.Writer = writer;
    }
    public Logger()
    {
        this.Writer = Console.Out;
    }
    public void Log(string message)
    {
        if (!LoggingEnabled)
        {
            return;
        }
        Buffer.AppendLine(message);
    }

    public void FlushBuffer()
    {
        Writer.WriteLine(Buffer.ToString());
        Buffer.Clear();
    }

    public ILogger Copy()
    {
        return new Logger() { Writer = Writer, Buffer = Buffer, LoggingEnabled = LoggingEnabled };
    }
}