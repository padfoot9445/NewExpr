using Common.LinearIR;

namespace SmallLang.LinearIR;
public record class OpcodeWrapper(Opcode Op) : IOperationArgument<int>
{
    public int Value => (int)Op;
    public static implicit operator OpcodeWrapper(Opcode inp)
    {
        return new(inp);
    }
}