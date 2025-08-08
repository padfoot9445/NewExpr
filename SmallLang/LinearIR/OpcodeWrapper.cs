using Common.LinearIR;

namespace SmallLang.LinearIR;

public record class OpcodeWrapper(Opcode Op) : GenericOperationArgument<BackingNumberType, Opcode>(Op)
{
    protected override IEnumerable<BackingNumberType> GetFromOp(Opcode op)
    {
        OpcodeBackingType NumVal = (OpcodeBackingType)op;
        int BITS = (int)Math.Ceiling(Math.Log2(OpcodeBackingType.MaxValue));
        for (int i = 8; i <= BITS; i += 8)
        {
            yield return (BackingNumberType)((NumVal >> (BITS - i)) & 0xFF);
        }
    }
    public static implicit operator OpcodeWrapper(Opcode inp)
    {
        return new(inp);
    }
    public override string ToString()
    {
        return Op.ToString();
    }
}