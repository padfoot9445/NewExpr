using System.Collections;
using System.Numerics;

namespace Common.LinearIR;

class Chunks<T> : IChunks<T> where T : INumber<T>
{
    private readonly List<IChunk<T>> chunks = [new Chunk<T>()];
    public IChunk<T> this[int index] => chunks[index];

    public IChunk<T> CurrentChunk => chunks[^1];

    public IEnumerator<IChunk<T>> GetEnumerator()
    {
        return chunks.GetEnumerator();
    }

    public void NewChunk()
    {
        chunks.Add(new Chunk<T>());
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}