using System.Diagnostics;
using SmallLang.Constants;
using SmallLang.LinearIR;
using SmallLang.Metadata;
namespace SmallLang.Frontend.CodeGen;

using static Opcode;
public partial class CodeGenerator
{
    private void ParseIf(Node Self)
    {
        SETCHUNK();
        //[ExpressionStatementCombined+, Else as statement?]
        var ESC = Self.Children.Where(x => x.NodeType == ImportantASTNodeType.ExprStatementCombined);
        var Expressions = ESC.Select(x => x.Children[0]).ToArray();
        var Statements = ESC.Select(x => x.Children[1]).ToArray();
        Node? Else = ESC.Count() < Self.Children.Count ? Self.Children[^1] : null;
        int Length = Self.Children.Count - (Else is null ? 0 : 1);
        //ENTERING CHUNK CHUNK0
        if (Else is null)
        {
            Emit<int, int>(IFNE, ACHUNK(0), Length);
        }
        else
        {
            Emit<int, int>(IFELSE, ACHUNK(0), Length);
        }

        for (int i = 1; i <= Length; i++)
        {

            //CHUNK [i * 2 - 1]
            NewChunk();
            Cast(Expressions[i], TypeData.Data.BooleanTypeCode);


            //CHUNK [i * 2]
            NewChunk();
            DynamicDispatch(Statements[i]);
        }
        if (Else is null)
        {

            //CHUNK [i * 2 + 1]
            NewChunk();
            //Next
        }
        else
        {

            //CHUNK [i * 2 + 1]
            NewChunk();
            DynamicDispatch(Else);

            //CHUNK [i * 2 + 2]
            NewChunk();
            //next
        }
    }

}