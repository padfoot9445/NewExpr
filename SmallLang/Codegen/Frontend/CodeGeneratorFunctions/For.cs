using System.Diagnostics;
using SmallLang.IR.AST;
using SmallLang.IR.LinearIR;
using SmallLang.IR.Metadata;

namespace SmallLang.CodeGen.Frontend;

using static Opcode;
public partial class CodeGenerator
{
    private void ParseFor(Node Self)
    {
        bool HasLabel = Self.Children[3].NodeType == ImportantASTNodeType.LoopLabel;
        Node Statement = HasLabel ? Self.Children[4] : Self.Children[3];
        Node? Else = Self.Children.Count == (HasLabel ? 6 : 5) ? Self.Children[^1] : null;
        //[expression, expression, expression, Label?, statement, else as Statement?]
        Verify(Self, ImportantASTNodeType.For);
        SETCHUNK();
        //entering chunk
        DynamicDispatch(Self.Children[0]); //Compile initializing expression
        Emit(JMP, ACHUNK(1));

        //CHUNK1
        NewChunk();
        DynamicDispatch(Self.Children[1]);//Compile conditional expression. This puts a 0 on the stack if false and a non-zero (probably 1 or 0xFF) onto the stack if true.
        Emit(BRZ, ACHUNK(2), ACHUNK(3));

        //CHUNK2
        NewChunk();
        DynamicDispatch(Statement);
        DynamicDispatch(Self.Children[2]); //Compile 3rd expression. This is what happens every loop; the i++, if you may.
        Emit(JMP, ACHUNK(1));

        //CHUNK3
        NewChunk();
        if (Else is not null)
        {
            DynamicDispatch(Else);
        }
        Emit(JMP, ACHUNK(4));

        //CHUNK4
        NewChunk();
        Data.LoopData[(LoopGUID)Self.Attributes.LoopGUID!] = (ACHUNK(2), ACHUNK(3));
    }
}