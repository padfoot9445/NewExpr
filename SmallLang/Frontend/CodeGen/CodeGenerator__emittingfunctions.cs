
using System.Numerics;
using SmallLang.LinearIR;

namespace SmallLang.Frontend.CodeGen;

public partial class CodeGenerator
{

    void Emit<T0>(Opcode op, GenericNumberWrapper<T0> N0)
    where T0 : IBinaryInteger<T0>, IMinMaxValue<T0>
        => Emit(op, N0);
    void Emit<T0, T1>(Opcode op, GenericNumberWrapper<T0> N0, GenericNumberWrapper<T1> N1)
    where T0 : IBinaryInteger<T0>, IMinMaxValue<T0>
    where T1 : IBinaryInteger<T1>, IMinMaxValue<T1>
        => Emit(op, N0, N1);
    void Emit<T0, T1, T2>(Opcode op, GenericNumberWrapper<T0> N0, GenericNumberWrapper<T1> N1, GenericNumberWrapper<T2> N2)
    where T0 : IBinaryInteger<T0>, IMinMaxValue<T0>
    where T1 : IBinaryInteger<T1>, IMinMaxValue<T1>
    where T2 : IBinaryInteger<T2>, IMinMaxValue<T2>
        => Emit(op, N0, N1, N2);
    void Emit<T0, T1, T2, T3>(Opcode op, GenericNumberWrapper<T0> N0, GenericNumberWrapper<T1> N1, GenericNumberWrapper<T2> N2, GenericNumberWrapper<T3> N3)
    where T0 : IBinaryInteger<T0>, IMinMaxValue<T0>
    where T1 : IBinaryInteger<T1>, IMinMaxValue<T1>
    where T2 : IBinaryInteger<T2>, IMinMaxValue<T2>
    where T3 : IBinaryInteger<T3>, IMinMaxValue<T3>
        => Emit(op, N0, N1, N2, N3);
    void Emit<T0, T1, T2, T3, T4>(Opcode op, GenericNumberWrapper<T0> N0, GenericNumberWrapper<T1> N1, GenericNumberWrapper<T2> N2, GenericNumberWrapper<T3> N3, GenericNumberWrapper<T4> N4)
    where T0 : IBinaryInteger<T0>, IMinMaxValue<T0>
    where T1 : IBinaryInteger<T1>, IMinMaxValue<T1>
    where T2 : IBinaryInteger<T2>, IMinMaxValue<T2>
    where T3 : IBinaryInteger<T3>, IMinMaxValue<T3>
    where T4 : IBinaryInteger<T4>, IMinMaxValue<T4>
        => Emit(op, N0, N1, N2, N3, N4);
}
