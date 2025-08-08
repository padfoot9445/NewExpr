using System.Collections;
using System.Numerics;

namespace Common.LinearIR;

class Chunk<T> : IChunk<T> where T : INumber<T>
{
    private List<Operation<T>> operations = [];
    public IEnumerable<Operation<T>> Instructions => operations.Select(x => x);

    public void AddOperation(Operation<T> Operation)
    {
        operations.Add(Operation);
    }

    public IEnumerator<Operation<T>> GetEnumerator()
    {
        return operations.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}