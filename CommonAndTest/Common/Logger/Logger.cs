namespace Common.Logger;
public class Logger : ILogger
{
    public TextWriter Writer { get; init; }

    public bool LoggingEnabled { get; set; } = false;

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
        Writer.WriteLine(message);
    }
}