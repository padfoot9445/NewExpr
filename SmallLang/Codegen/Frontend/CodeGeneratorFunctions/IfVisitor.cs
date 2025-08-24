using SmallLang.IR.AST;
using SmallLang.IR.AST.Generated;
using SmallLang.IR.LinearIR;
using SmallLang.IR.Metadata;
namespace SmallLang.CodeGen.Frontend.CodeGeneratorFunctions;

internal static class IfVisitor
{
    public static void Visit(IfNode Self, CodeGenerator Driver)
    {

        //[ExpressionStatementCombined+, Else as statement?]
        var ESC = Self.ExprStatementCombineds.ToArray();
        var Expressions = ESC.Select(x => x.Expression).ToArray();
        var Statements = ESC.Select(x => x.Section).ToArray();
        int CondReg = Driver.GetRegisters((int)TypeData.Bool.Size).First();

        Driver.EnteringChunk(() =>
        {
            Driver.Emit(HighLevelOperation.IfElse
            (
                StartingBlockID: 1,
                NumberOfCases: ESC.Length,
                ElseBlock: ESC.Length * 2 + 1,
                NextBlock: ESC.Length * 2 + 2,
                ConditionStoringRegister: CondReg,
                ConditionTypeWidth: TypeData.Bool.Size
            ));
        });

        for (int i = 1; i <= ESC.Length; i++)
        {
            Driver.NewChunk(i * 2 - 1, () =>
            {
                Driver.Cast(Expressions[i], TypeData.Bool);
                Driver.Emit(HighLevelOperation.LoadFromStack(CondReg, TypeData.Bool.Size));
            });

            Driver.NewChunk(i * 2 - 1, () =>
            {
                Driver.Exec(Statements[i]);
            });
        }

        Driver.NewChunk(ESC.Length * 2 + 1, () =>
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

        Driver.NewChunk(ESC.Length * 2 + 2, () =>
        {
            Driver.Next();
        });
    }

}