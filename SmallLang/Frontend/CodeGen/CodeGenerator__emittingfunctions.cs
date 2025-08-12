
using System.Numerics;
using SmallLang.LinearIR;
using Common.LinearIR;
namespace SmallLang.Frontend.CodeGen;

public partial class CodeGenerator
{

    void Emit<T0>(Opcode op, NumberWrapper<T0, BackingNumberType> N0)
    where T0 : IBinaryInteger<T0>, IMinMaxValue<T0>
        => Emit(op, N0);
    void Emit<T0, T1>(Opcode op, NumberWrapper<T0, BackingNumberType> N0, NumberWrapper<T1, BackingNumberType> N1)
    where T0 : IBinaryInteger<T0>, IMinMaxValue<T0>
    where T1 : IBinaryInteger<T1>, IMinMaxValue<T1>
        => Emit(op, N0, N1);
    void Emit<T0, T1, T2>(Opcode op, NumberWrapper<T0, BackingNumberType> N0, NumberWrapper<T1, BackingNumberType> N1, NumberWrapper<T2, BackingNumberType> N2)
    where T0 : IBinaryInteger<T0>, IMinMaxValue<T0>
    where T1 : IBinaryInteger<T1>, IMinMaxValue<T1>
    where T2 : IBinaryInteger<T2>, IMinMaxValue<T2>
        => Emit(op, N0, N1, N2);
    void Emit<T0, T1, T2, T3>(Opcode op, NumberWrapper<T0, BackingNumberType> N0, NumberWrapper<T1, BackingNumberType> N1, NumberWrapper<T2, BackingNumberType> N2, NumberWrapper<T3, BackingNumberType> N3)
    where T0 : IBinaryInteger<T0>, IMinMaxValue<T0>
    where T1 : IBinaryInteger<T1>, IMinMaxValue<T1>
    where T2 : IBinaryInteger<T2>, IMinMaxValue<T2>
    where T3 : IBinaryInteger<T3>, IMinMaxValue<T3>
        => Emit(op, N0, N1, N2, N3);
    void Emit<T0, T1, T2, T3, T4>(Opcode op, NumberWrapper<T0, BackingNumberType> N0, NumberWrapper<T1, BackingNumberType> N1, NumberWrapper<T2, BackingNumberType> N2, NumberWrapper<T3, BackingNumberType> N3, NumberWrapper<T4, BackingNumberType> N4)
    where T0 : IBinaryInteger<T0>, IMinMaxValue<T0>
    where T1 : IBinaryInteger<T1>, IMinMaxValue<T1>
    where T2 : IBinaryInteger<T2>, IMinMaxValue<T2>
    where T3 : IBinaryInteger<T3>, IMinMaxValue<T3>
    where T4 : IBinaryInteger<T4>, IMinMaxValue<T4>
        => Emit(op, N0, N1, N2, N3, N4);
}
