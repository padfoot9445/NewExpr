using System.Collections;
using System.Numerics;

namespace Common.LinearIR;

public abstract class Chunks<T, C> : IChunks<T> where T : INumber<T> where C : IChunk<T>, new()
{
    private readonly List<IChunk<T>> chunks = [new C()];
    public IChunk<T> this[int index] => chunks[index];

    public IChunk<T> CurrentChunk => chunks[^1];

    public IEnumerator<IChunk<T>> GetEnumerator()
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