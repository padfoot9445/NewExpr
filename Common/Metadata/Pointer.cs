using System.Numerics;
using Common.LinearIR;

namespace Common.Metadata;

public record class Pointer<TActual, TBacking> : NumberWrapper<TActual, TBacking>
where TActual : IBinaryInteger<TActual>, IMinMaxValue<TActual>
where TBacking : IBinaryInteger<TBacking>, IMinMaxValue<TBacking>
{
    public Pointer(TActual BackingValue) : base(BackingValue)
    {
    }

    protected Pointer(NumberWrapper<TActual, TBacking> original) : base(original)
    {
    }
}
