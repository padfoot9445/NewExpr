using System.Collections;
using System.Numerics;
using Common.Metadata;

namespace Common.LinearIR;

public record class DataCollection<T> : IList<T>
    where T : IBinaryInteger<T>, IMinMaxValue<T>
{
    private readonly List<T> Data = new();

    public T this[Pointer<T> index]
    {
        get => Data[index.BackingValue];
        set => Data[index.BackingValue] = value;
    }

    T IList<T>.this[int index]
    {
        get => this[new Pointer<T>(index)];
        set => this[new Pointer<T>(index)] = value;
    }

    public int Count => Data.Count;

    public bool IsReadOnly => ((ICollection<T>)Data).IsReadOnly;

    public void Add(T item)
    {
        Data.Add(item);
    }

    public void Clear()
    {
        Data.Clear();
    }

    public bool Contains(T item)
    {
        return Data.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        Data.CopyTo(array, arrayIndex);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return Data.GetEnumerator();
    }

    public int IndexOf(T item)
    {
        return Data.IndexOf(item);
    }

    public void Insert(int index, T item)
    {
        Data.Insert(index, item);
    }

    public bool Remove(T item)
    {
        return Data.Remove(item);
    }

    public void RemoveAt(int index)
    {
        Data.RemoveAt(index);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}