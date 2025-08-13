using System.Numerics;
using Common.LinearIR;

namespace SmallLang.IR.LinearIR;

public record class GenericNumberWrapper<TActual> : NumberWrapper<TActual, BackingNumberType>
where TActual : IBinaryInteger<TActual>, IMinMaxValue<TActual>
{
    public GenericNumberWrapper(TActual BackingValue) : base(BackingValue)
    {
    }

    protected GenericNumberWrapper(NumberWrapper<TActual, BackingNumberType> original) : base(original)
    {
    }
    public static implicit operator GenericNumberWrapper<TActual>(TActual other) => new(other);
}
