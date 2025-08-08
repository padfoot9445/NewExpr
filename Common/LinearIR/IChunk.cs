using System.Numerics;

namespace Common.LinearIR;

public interface IChunk<T> : IEnumerable<Operation<T>> where T : INumber<T>
{
    public IEnumerable<Operation<T>> Instructions { get; }
    public void AddOperation(Operation<T> Operation);
}