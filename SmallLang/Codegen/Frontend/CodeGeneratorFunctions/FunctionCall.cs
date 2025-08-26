using System.Diagnostics;
using Common.Metadata;
using Common.Tokens;
using SmallLang.IR.AST;
using SmallLang.IR.AST.Generated;
using SmallLang.IR.LinearIR;
using SmallLang.IR.Metadata;

namespace SmallLang.CodeGen.Frontend.CodeGeneratorFunctions;

internal static class FunctionCallVisitor
{
    internal static void Visit(FunctionCallNode Self, CodeGenerator Driver)
    {
        Debug.Assert(Self.TypeOfExpression is not null);

        Driver.EnteringChunk(() =>
        {
            foreach (var i in Self.ArgList)
            {
                Driver.Cast(i.Expression, i.Expression.ExpectedTypeOfExpression!); //no need to load into register because we want it on the stack
            }


            var ReturnRegister = Driver.GetRegisters(Self).First();

            FunctionID<BackingNumberType> ID = Self.FunctionID!;
            Driver.Emit(HighLevelOperation.Call(ID));

            Driver.Emit(HighLevelOperation.LoadFromStack(ReturnRegister, Self.TypeOfExpression.Size));
            Driver.Emit(HighLevelOperation.PushFromRegister(ReturnRegister, Self.TypeOfExpression.Size)); //pop and push because we want to have a place to store the retval for optimising purposes
        });
    }
}