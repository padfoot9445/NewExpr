namespace Common.Logger;

public interface ILogger
{
    public bool LoggingEnabled { get; set; }
    public void FlushBuffer();
    public void Log(string message);

    public virtual void SuppressLog()
    {
        LoggingEnabled = false;
    }

    public virtual void EnableLog()
    {
        LoggingEnabled = true;
    }

    public ILogger Copy();
}