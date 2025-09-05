using System.Numerics;

namespace Common.LinearIR;

public abstract record class GenericOperationArgument<TInterfaceBacking, TActualBacking>(TActualBacking BackingValue)
    : IOpActual<TInterfaceBacking, TActualBacking>
    where TInterfaceBacking : INumber<TInterfaceBacking>
{
    private IEnumerable<TInterfaceBacking>? val;
    public IEnumerable<TInterfaceBacking> Value => val ??= GetFromOp(BackingValue);
    protected abstract IEnumerable<TInterfaceBacking> GetFromOp(TActualBacking op);
}