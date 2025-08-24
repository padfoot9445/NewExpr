using System.Numerics;

namespace Common.LinearIR;

public record class Operation<TOpCode, TBacking>(IOpActual<TBacking, TOpCode> Op, params IOperationArgument<TBacking>[] Operands)
where TBacking : INumber<TBacking>
{
    public override string ToString()
    {
        return $"{Op} {Operands.Select(x => x.Value.ToString()).Aggregate((x, y) => $"{x} {y}")}";
    }
}
