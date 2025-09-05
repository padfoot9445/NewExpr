using System.Numerics;
using Common.LinearIR;

namespace Common.Metadata;

public record class Pointer<TBacking>(int BackingValue) : NumberWrapper<int, TBacking>(BackingValue)
    where TBacking : IBinaryInteger<TBacking>, IMinMaxValue<TBacking>
{
    public Pointer<TBacking> Next()
    {
        return Add(1);
    }

    public Pointer<TBacking> Add(int amount)
    {
        return this with { BackingValue = BackingValue + amount };
    }

    public static Pointer<TBacking> GetDefault()
    {
        return new Pointer<TBacking>(0);
    }

    public static bool operator <(Pointer<TBacking> self, Pointer<TBacking> other)
    {
        return self.BackingValue < other.BackingValue;
    }

    public static bool operator >(Pointer<TBacking> self, Pointer<TBacking> other)
    {
        return self.BackingValue > other.BackingValue;
    }
}