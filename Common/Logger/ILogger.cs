namespace Common.Logger;
public interface ILogger
{
    public void Log(string message);
    public virtual void SuppressLog()
    {
        LoggingEnabled = false;
    }
    public virtual void EnableLog()
    {
        LoggingEnabled = true;
    }
    public bool LoggingEnabled { get; set; }
}