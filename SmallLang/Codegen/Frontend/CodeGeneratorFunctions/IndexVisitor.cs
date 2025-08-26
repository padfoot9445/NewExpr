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

            var IndexerRegister = Driver.GetRegisters(Self.Expression2.ExpectedTypeOfExpression).First();
            Driver.Cast(Self.Expression2, Self.Expression2.ExpectedTypeOfExpression);
            Driver.Emit(HighLevelOperation.PushFromRegister(IndexerRegister, Self.Expression2.ExpectedTypeOfExpression.Size));

            var DstRegister = Driver.GetRegisters(Self).First();

            if (Self.TypeOfExpression == TypeData.Dict)
            {
                Driver.Emit(HighLevelOperation.QueryHashMap<int, int, int, byte, byte>(IndexerRegister, StructRegister, DstRegister, Self.Expression2.ExpectedTypeOfExpression, Self.TypeOfExpression));
            }
            else
            {
                //we allow indexing of sets and stuff as a foreach thing. Worst case we handle generation of a view in the instruction
                var PointerRegister = Driver.GetRegisters(TypeData.Int).First();
                Driver.Emit(HighLevelOperation.IndexVectorLike(StructRegister, IndexerRegister, PointerRegister));
                Driver.Emit(HighLevelOperation.LoadFromMemory(PointerRegister, DstRegister, Self.TypeOfExpression.Size));
            }

            Driver.Next();
        });
    }
}