using System.Diagnostics;
using Common.Tokens;
using SmallLang.IR.AST;
using SmallLang.IR.AST.Generated;
using SmallLang.IR.LinearIR;
using SmallLang.IR.Metadata;

namespace SmallLang.CodeGen.Frontend.CodeGeneratorFunctions;

internal static class IndexVisitor
{
    internal static void Visit(IndexNode Self, CodeGenerator Driver)
    {
        Debug.Assert(Self.Expression1.TypeOfExpression is not null);
        Debug.Assert(Self.Expression2.ExpectedTypeOfExpression is not null);
        Debug.Assert(Self.TypeOfExpression is not null);

        Driver.EnteringChunk(() =>
        {
            var StructRegister = Driver.GetRegisters(Self.Expression1.TypeOfExpression).First();
            Driver.Exec(Self.Expression1);
            Driver.Emit(HighLevelOperation.PushFromRegister(StructRegister, Self.Expression1.TypeOfExpression.Size));


            if (Self.TypeOfExpression == TypeData.Dict)
            {

            }
            else
            {

            }

            Driver.Next();
        });
    }
}