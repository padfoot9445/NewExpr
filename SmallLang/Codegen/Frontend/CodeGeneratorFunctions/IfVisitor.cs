using SmallLang.IR.AST;
using SmallLang.IR.AST.Generated;
using SmallLang.IR.LinearIR;
using SmallLang.IR.Metadata;
namespace SmallLang.CodeGen.Frontend.CodeGeneratorFunctions;

using static Opcode;
internal static class IfVisitor
{
    public static void Visit(SmallLangNode Self, CodeGenerator Driver)
    {
        Driver.SETCHUNK();
        //[ExpressionStatementCombined+, Else as statement?]
        var ESC = Self.Children.Where(x => x.NodeType == ImportantASTNodeType.ExprStatementCombined);
        var Expressions = ESC.Select(x => x.Children[0]).ToArray();
        var Statements = ESC.Select(x => x.Children[1]).ToArray();
        SmallLangNode? Else = ESC.Count() < Self.Children.Count ? Self.Children[^1] : null;
        int Length = Self.Children.Count - (Else is null ? 0 : 1);
        //ENTERING CHUNK CHUNK0
        if (Else is null)
        {
            Driver.Emit<int, int>(IFNE, Driver.ACHUNK(0), Length);
        }
        else
        {
            Driver.Emit<int, int>(IFELSE, Driver.ACHUNK(0), Length);
        }

        for (int i = 1; i <= Length; i++)
        {

            //CHUNK [i * 2 - 1]
            Driver.NewChunk();
            Driver.Cast(Expressions[i], TypeData.Bool);


            //CHUNK [i * 2]
            Driver.NewChunk();
            Driver.Exec(Statements[i]);
        }
        if (Else is null)
        {

            //CHUNK [i * 2 + 1]
            Driver.NewChunk();
            //Next
        }
        else
        {

            //CHUNK [i * 2 + 1]
            Driver.NewChunk();
            Driver.Exec(Else);

            //CHUNK [i * 2 + 2]
            Driver.NewChunk();
            //next
        }
    }

}