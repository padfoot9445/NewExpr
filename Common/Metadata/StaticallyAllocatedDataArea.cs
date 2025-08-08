using System.Numerics;
using Common.LinearIR;

namespace Common.Metadata;

public record class StaticallyAllocatedDataArea<TKey, TBacking>
where TBacking : IBinaryInteger<TBacking>, IMinMaxValue<TBacking>
where TKey : notnull
{
    public HashSet<Pointer<TBacking>> UsedPositions { get; init; } = new();
    public Dictionary<TKey, Pointer<TBacking>> KeyToPointerStartMap = new();
    public Dictionary<TKey, int> KeyToNumberOfCellsUsed = new();
    public Pointer<TBacking> AllFreePointer { get; private set; } = Pointer<TBacking>.GetDefault();//all pointer values above this should be free
    public DataCollection<TBacking> Store { get; } = new();
    public Pointer<TBacking> Allocate(int width = 1)
    {
        var x = AllFreePointer;
        for (var i = AllFreePointer; i < AllFreePointer.Add(width); i = i.Add(1))
        {
            if (UsedPositions.Contains(i)) throw new Exception("Accidentally allocated a used ptr");
            UsedPositions.Add(i);
        }
        return x;
    }
    public Pointer<TBacking> Allocate(TKey Key, int width = 1)
    {
        KeyToNumberOfCellsUsed[Key] = width;
        KeyToPointerStartMap[Key] = AllFreePointer;
        return Allocate(width);
    }
}