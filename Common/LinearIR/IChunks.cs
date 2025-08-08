using System.Numerics;

namespace Common.LinearIR;

public interface IChunks<T> : IEnumerable<IChunk<T>> where T : INumber<T>
{
    public IChunk<T> this[int index] { get; }
    public void NewChunk();
    public IChunk<T> CurrentChunk { get; }
}