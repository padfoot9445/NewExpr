using System.Collections;
using System.Numerics;

namespace Common.LinearIR;

public abstract class Chunks<T, C, TOpcode> : IChunks<TOpcode, T> where T : INumber<T> where C : IChunk<TOpcode, T>, new()
{
    private readonly List<IChunk<TOpcode, T>> chunks = [new C()];
    public IChunk<TOpcode, T> this[int index] => chunks[index];

    public IChunk<TOpcode, T> CurrentChunk => chunks[^1];

    public IEnumerator<IChunk<TOpcode, T>> GetEnumerator()
    {
        return chunks.GetEnumerator();
    }

    public void NewChunk()
    {
        chunks.Add(new C());
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}