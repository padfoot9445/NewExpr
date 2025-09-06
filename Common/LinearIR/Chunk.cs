using System.Collections;
using System.Numerics;

namespace Common.LinearIR;

public abstract class Chunk<TOpcode, T> : IChunk<TOpcode, T> where T : INumber<T>
{
    private readonly List<Operation<TOpcode, T>> operations = [];

    public Operation<TOpcode, T> this[int index] => operations[index];

    Operation<TOpcode, T> IList<Operation<TOpcode, T>>.this[int index]
    {
        get => operations[index];
        set => operations[index] = value;
    }


    public int Count => operations.Count;

    public bool IsReadOnly => false;

    public void Add(Operation<TOpcode, T> item)
    {
        operations.Add(item);
    }


    public void Clear()
    {
        throw new NotSupportedException();
    }

    public bool Contains(Operation<TOpcode, T> item)
    {
        return operations.Contains(item);
    }

    public void CopyTo(Operation<TOpcode, T>[] array, int arrayIndex)
    {
        operations.CopyTo(array, arrayIndex);
    }

    public IEnumerator<Operation<TOpcode, T>> GetEnumerator()
    {
        return operations.GetEnumerator();
    }

    public int IndexOf(Operation<TOpcode, T> item)
    {
        return operations.IndexOf(item);
    }

    public void Insert(int index, Operation<TOpcode, T> item)
    {
        operations.Insert(index, item);
    }

    public bool Remove(Operation<TOpcode, T> item)
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