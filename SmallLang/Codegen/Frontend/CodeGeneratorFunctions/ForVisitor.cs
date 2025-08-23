using System.Reflection.Emit;
using SmallLang.IR.AST;
using SmallLang.IR.AST.Generated;
using SmallLang.IR.LinearIR;
using SmallLang.IR.Metadata;

namespace SmallLang.CodeGen.Frontend.CodeGeneratorFunctions;

internal static class ForVisitor
{
    public static void Visit(ForNode Self, CodeGenerator Driver)
    {

        //entering chunk
        Driver.EnteringChunk(() =>
        {
            Driver.Exec(Self.Expression); //Compile initializing expression
            Driver.Emit(HighLevelOperation.Loop
            (
                Condition: 1,
                Epilogue: 2,
                Main: 3,
                Else: 4,
                Next: 5
            ));
        });

        Driver.NewChunk(1, () =>
        {
            Driver.Cast(Self.Expression2, TypeData.Bool); //Compile condition expression
        });

        Driver.NewChunk(2, () =>
        {
            Driver.Exec(Self.Expression3); //Compile postloop expression
        });

        Driver.NewChunk(3, () =>
        {
            Driver.Exec(Self.Statement);
        });

        Driver.NewChunk(4, () =>
        {
            if (Self.Else is null)
            {
                Driver.Emit(HighLevelOperation.NOp());
            }
            else
            {
                Driver.Exec(Self.Else);
            }
        });

        Driver.NewChunk(5, () =>
        {
            Driver.Next();
        });

        StoreUuid(Self, Driver);
    }
    public static void StoreUuid(SmallLangNode Self, CodeGenerator Driver)
    {
        Driver.Data.LoopData[(LoopGUID)Self.LoopGUID!] = (Driver.GetChild(1).Uuid, Driver.GetChild(2).Uuid, Driver.GetChild(3).Uuid, Driver.GetChild(4).Uuid, Driver.GetChild(5).Uuid);
    }
}