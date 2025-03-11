using System.Numerics;

namespace Common.LinearIR;
public class IntOpArg(int val) : IOperationArgument<int>
{
    public int Value { get; init; } = val;
    public static implicit operator IntOpArg(int inp)
    {
        return new(inp);
    }
}