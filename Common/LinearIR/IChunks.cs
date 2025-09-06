using System.Numerics;

namespace Common.LinearIR;

public interface IChunks<TOpcode, T> : IEnumerable<IChunk<TOpcode, T>> where T : INumber<T>
{
    public IChunk<TOpcode, T> this[int index] { get; }
    public IChunk<TOpcode, T> CurrentChunk { get; }
    public void NewChunk();
}