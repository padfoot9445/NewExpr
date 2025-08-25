namespace Common.LinearIR;

public interface IActualValue<TActualValue>
{
    public TActualValue BackingValue { get; }
}