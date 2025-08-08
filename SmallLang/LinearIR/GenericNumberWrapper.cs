using Common.LinearIR;

namespace SmallLang.LinearIR;

public record class GenericNumberWrapper : NumberWrapper<OpcodeBackingType, BackingNumberType>
{
    public GenericNumberWrapper(uint BackingValue) : base(BackingValue)
    {
    }

    protected GenericNumberWrapper(NumberWrapper<uint, byte> original) : base(original)
    {
    }
    public static implicit operator GenericNumberWrapper(uint other) => new(other);
}
