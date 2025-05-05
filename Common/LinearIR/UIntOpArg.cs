using System.Numerics;

namespace Common.LinearIR;
public class UIntOpArg(uint val) : IOperationArgument<uint>
{
    public uint Value { get; init; } = val;
    public static implicit operator UIntOpArg(uint inp)
    {
        return new(inp);
    }
}