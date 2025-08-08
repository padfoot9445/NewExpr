using System.Numerics;

namespace Common.LinearIR;

public interface IChunk<T> : IList<Operation<T>> where T : INumber<T>
{
    //IList because we're probably going to reach in and fuck with stuff during optimisations
}