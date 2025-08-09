using System.Diagnostics;
using SmallLang.LinearIR;
using SmallLang.Metadata;

namespace SmallLang.Frontend.CodeGen;

using static Opcode;
public partial class CodeGenerator
{
    private void ParseWhile(Node Self)
    {
        bool HasLabel = Self.Children[1].NodeType == ImportantASTNodeType.LoopLabel;
        Node Statement = HasLabel ? Self.Children[2] : Self.Children[1];
        Node? Else = Self.Children.Count == (HasLabel ? 4 : 3) ? Self.Children[^1] : null;
        //[expression, Label?, statement, else as Statement?]
        Verify(Self, ImportantASTNodeType.While);
        SETCHUNK();
        //entering chunk
        Emit(JMP, ACHUNK(1));

        //CHUNK1
        NewChunk();
        DynamicDispatch(Self.Children[0]);//Compile conditional expression. This puts a 0 on the stack if false and a non-zero (probably 1 or 0xFF) onto the stack if true.
        Emit(BRZ, ACHUNK(2), ACHUNK(3));

        //CHUNK2
        NewChunk();
        DynamicDispatch(Statement);
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
        data.LoopData[(LoopGUID)Self.Attributes.LoopGUID!] = (ACHUNK(2), ACHUNK(3));
    }
}