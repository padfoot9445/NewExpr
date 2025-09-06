using System.Numerics;

namespace Common.LinearIR;

public interface IOpActual<TO, TA> : IOperationArgument<TO>, IActualValue<TA>
    where TO : INumber<TO>;