namespace Parser;
class Logger
{
    public TextWriter Writer { get; init; }
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
        Writer.WriteLine(message);
    }
}