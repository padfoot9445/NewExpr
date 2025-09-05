using System.Diagnostics;
using SmallLang.IR.AST.Generated;
using SmallLang.IR.LinearIR;
using SmallLang.IR.Metadata;

namespace SmallLang.CodeGen.Frontend.CodeGeneratorFunctions;

internal static class CopyExpressionVisitor
{
    internal static void Visit(CopyExprNode Self, CodeGenerator Driver)
    {
        Debug.Assert(Self.Expression.TypeOfExpression is not null);
        Driver.EnteringChunk(() =>
        {
            var Register = Driver.GetRegisters(Self).First();
            Driver.Exec(Self);
            Driver.Emit(HighLevelOperation.LoadFromStack(Register, Self.Expression.TypeOfExpression.Size));
            var DstRegister = Driver.GetRegisters(Self).First();

            if (Self.Expression.TypeOfExpression.IsRefType)
                Driver.Emit(HighLevelOperation.CopyStruct(Register, DstRegister, TypeData.Int.Size));
            else
                Driver.Emit(HighLevelOperation.MoveRegister(Register, DstRegister,
                    Self.Expression.TypeOfExpression.Size));

            Driver.Emit(HighLevelOperation.PushFromRegister(DstRegister, Self.Expression.TypeOfExpression.Size));
        });
    }
}