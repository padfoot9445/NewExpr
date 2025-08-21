using SmallLang.IR.AST;
using SmallLang.IR.AST.Generated;
using SmallLang.IR.LinearIR;
namespace SmallLang.CodeGen.Frontend.CodeGeneratorFunctions;

using static Opcode;
internal static class SwitchVisitor
{
    public static void Visit(SmallLangNode Self, CodeGenerator Driver)
    {
        Driver.SETCHUNK();
        SmallLangNode Expression = Self.Children[0];
        var Expressions = Self.Children.Skip(1).Select(x => x.Children[0]).ToArray();
        var Statements = Self.Children.Skip(1).Select(x => x.Children[1]).ToArray();
        int Length = Self.Children.Count - 1;

        //ENTERING CHUNK CHUNK0
        Driver.Exec(Expression);
        Driver.Emit(JMP, Driver.ACHUNK(Length * 2 + 1));

        for (int i = 1; i <= Length; i++)
        {

            //CHUNK [i * 2 - 1]
            Driver.NewChunk();
            Driver.Cast(Expressions[i - 1], Expression.Attributes.TypeLiteralType!);


            //CHUNK [i * 2]
            Driver.NewChunk();
            Driver.Exec(Statements[i - 1]);
        }


        //CHUNK [LENGTH * 2 + 1]
        Driver.NewChunk();
        Driver.Emit<int, int, BackingNumberType>(SWITCH, Length, Driver.ACHUNK(0), Expression.Attributes.TypeOfExpression!);


        //CHUNK [LENGTH * 2 + 2]
        Driver.NewChunk();
        //NEXT
    }
}