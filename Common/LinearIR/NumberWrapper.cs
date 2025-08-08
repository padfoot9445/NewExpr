using System.Numerics;

namespace Common.LinearIR;

public record class NumberWrapper<T, TBacking> : GenericOperationArgument<TBacking, T>
where T : IBinaryInteger<T>, IMinMaxValue<T>
where TBacking : IBinaryInteger<TBacking>, IMinMaxValue<TBacking>
{
    public NumberWrapper(T BackingValue) : base(BackingValue)
    {
    }
    private int TBits => GetBits<T>();
    static int GetBits<TInner>() where TInner : IBinaryInteger<TInner>, IMinMaxValue<TInner>
    => TInner.MaxValue.GetByteCount() * 4;
    protected override IEnumerable<TBacking> GetFromOp(T op)
    {
        for (int i = 8; i <= TBits; i += 8)
        {
            yield return TBacking.CreateTruncating(op >> (TBits - i));
        }
    }

}