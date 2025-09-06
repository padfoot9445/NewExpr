using System.Diagnostics;
using SmallLang.IR.AST;
using SmallLang.IR.AST.Generated;
using SmallLang.IR.LinearIR;

namespace SmallLang.CodeGen.Frontend.CodeGeneratorFunctions;

internal static class FunctionCallVisitor
{
    internal static void Visit(FunctionCallNode Self, CodeGenerator Driver)
    {
        Debug.Assert(Self.TypeOfExpression is not null);

        Driver.EnteringChunk(() =>
        {
            CallFunction(Self, Self.ArgList, Driver);
            Driver.Next();
        });
    }

    internal static void CallFunction<T1>(T1 Self, IEnumerable<ArgListElementNode> ArgList, CodeGenerator Driver)
        where T1 : ISmallLangNode, IHasAttributeFunctionID, IHasAttributeTypeOfExpression
    {
        foreach (var i in
                 ArgList)
            Driver.Cast(i.Expression,
                i.ExpectedTypeOfExpression!); //no need to load into register because we want it on the stack


        var ReturnRegister = Driver.GetRegisters(Self).First();

        var ID = Self.FunctionID!;
        Driver.Emit(HighLevelOperation.Call(ID));

        Driver.Emit(HighLevelOperation.LoadFromStack(ReturnRegister, Self.TypeOfExpression!.Size));
        Driver.Emit(
            HighLevelOperation.PushFromRegister(ReturnRegister,
                Self.TypeOfExpression
                    .Size)); //pop and push because we want to have a place to store the retval for optimising purposes
    }
}