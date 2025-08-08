using System.Collections;
using System.Numerics;

namespace Common.LinearIR;

class Chunk<T> : IChunk<T> where T : INumber<T>
{
    private readonly List<Operation<T>> operations = [];

    public Operation<T> this[int index] => operations[index];

    Operation<T> IList<Operation<T>>.this[int index] { get => operations[index]; set => operations[index] = value; }

    // public IEnumerable<Operation<T>> Instructions => operations.Select(x => x);

    public int Count => operations.Count;

    public bool IsReadOnly => false;

    public void Add(Operation<T> item)
    {
        operations.Add(item);
    }


    public void Clear()
    {
        throw new NotSupportedException();
    }

    public bool Contains(Operation<T> item)
    {
        return operations.Contains(item);
    }

    public void CopyTo(Operation<T>[] array, int arrayIndex)
    {
        operations.CopyTo(array, arrayIndex);
    }

    public IEnumerator<Operation<T>> GetEnumerator()
    {
        return operations.GetEnumerator();
    }

    public int IndexOf(Operation<T> item)
    {
        return operations.IndexOf(item);
    }

    public void Insert(int index, Operation<T> item)
    {
        operations.Insert(index, item);
    }

    public bool Remove(Operation<T> item)
    {
        return operations.Remove(item);
    }

    public void RemoveAt(int index)
    {
        operations.RemoveAt(index);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}