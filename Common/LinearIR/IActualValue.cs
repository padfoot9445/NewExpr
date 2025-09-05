using JetBrains.Annotations;

namespace Common.LinearIR;

[PublicAPI]
public interface IActualValue<out TActualValue>
{
    public TActualValue BackingValue { get; }
}