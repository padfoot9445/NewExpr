using System.Numerics;
using Common.LinearIR;

namespace Common.Metadata;

public record class StaticallyAllocatedDataArea<TKey, TBacking>
    where TBacking : IBinaryInteger<TBacking>, IMinMaxValue<TBacking>, new()
    where TKey : notnull
{
    public Dictionary<TKey, int> KeyToNumberOfCellsUsed = new();
    public Dictionary<TKey, Pointer<TBacking>> KeyToPointerStartMap = new();
    public HashSet<Pointer<TBacking>> UsedPositions { get; init; } = new();

    public Pointer<TBacking> AllFreePointer { get; } =
        Pointer<TBacking>.GetDefault(); //all pointer values above this should be free

    public DataCollection<TBacking> Store { get; } = new();

    public Pointer<TBacking> Allocate(int width = 1)
    {
        for (var i = AllFreePointer; i < AllFreePointer.Add(width); i = i.Add(1))
        {
            if (UsedPositions.Contains(i)) throw new Exception("Accidentally allocated a used ptr");
            UsedPositions.Add(i);
        }

        return AllFreePointer;
    }

    public Pointer<TBacking> Allocate(TKey Key, int width = 1)
    {
        KeyToNumberOfCellsUsed[Key] = width;
        KeyToPointerStartMap[Key] = AllFreePointer;
        return Allocate(width);
    }

    public void FillFrom(Pointer<TBacking> start, params IEnumerable<TBacking> Values)
    {
        while (Store.Count < start.BackingValue + Values.Count()) Store.Add(new TBacking());
    }

    public Pointer<TBacking> AllocateAndFill(TKey Key, int width, params IEnumerable<TBacking> Values)
    {
        var ptr = Allocate(Key, width);
        FillFrom(ptr, Values);
        return ptr;
    }

    public Pointer<TBacking> AllocateAndFill(int width, params IEnumerable<TBacking> Values)
    {
        var ptr = Allocate(width);
        FillFrom(ptr, Values);
        return ptr;
    }
}