using Common.LinearIR;

namespace SmallLang.LinearIR;
public record class OpcodeWrapper(Opcode Op) : IOperationArgument<uint>
{
    public uint Value => (uint)Op;
    public static implicit operator OpcodeWrapper(Opcode inp)
    {
        return new(inp);
    }
}