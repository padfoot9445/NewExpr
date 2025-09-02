using System.Numerics;
using Common.LinearIR;

namespace Common.Metadata;

public record class FunctionID<TBackingType>(uint BackingValue) : NumberWrapper<uint, TBackingType>(BackingValue)
where TBackingType : IBinaryInteger<TBackingType>, IMinMaxValue<TBackingType>
{
    static uint Last { get; set; } = uint.MaxValue / 2;
    public static FunctionID<TBackingType> GetNext() => new(Last++);
}