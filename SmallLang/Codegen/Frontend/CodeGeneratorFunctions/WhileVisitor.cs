using SmallLang.IR.AST;
using SmallLang.IR.AST.Generated;
using SmallLang.IR.LinearIR;
using SmallLang.IR.Metadata;

namespace SmallLang.CodeGen.Frontend.CodeGeneratorFunctions;

internal static class WhileVisitor
{
    public static void Visit(WhileNode Self, CodeGenerator Driver)
    {
        Driver.EnteringChunk(() =>
        {
            Driver.Emit(HighLevelOperation.Loop(1, 2, 3, 4, 5));
        });

        Driver.NewChunk(1, () =>
        {
            Driver.Cast(Self.Expression1, TypeData.Bool);
        });

        Driver.NewChunk(2, () =>
        {
            Driver.Emit(HighLevelOperation.NOp());
        });

        Driver.NewChunk(3, () =>
        {
            Driver.Exec(Self.Statement1);
        });

        Driver.NewChunk(4, () =>
        {
            if (Self.Else1 is null)
            {
                Driver.Emit(HighLevelOperation.NOp());
            }
            else
            {
                Driver.Exec(Self.Else1);
            }
        });

        Driver.Next(5);

        ForVisitor.StoreUuid(Self, Driver);
    }
}