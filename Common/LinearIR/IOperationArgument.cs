using System.Numerics;

namespace Common.LinearIR;

public interface IOperationArgument<TBacking> where TBacking : INumber<TBacking>
{
    public IEnumerable<TBacking> Value { get; }
}