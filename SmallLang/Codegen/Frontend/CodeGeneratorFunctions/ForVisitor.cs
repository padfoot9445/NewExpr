using SmallLang.IR.AST;
using SmallLang.IR.LinearIR;
using SmallLang.IR.Metadata;

namespace SmallLang.CodeGen.Frontend.CodeGeneratorFunctions;

using static Opcode;
internal static class ForVisitor
{
    public static void Visit(Node Self, CodeGenerator Driver)
    {
        bool HasLabel = Self.Children[3].NodeType == ImportantASTNodeType.LoopLabel;
        Node Statement = HasLabel ? Self.Children[4] : Self.Children[3];
        Node? Else = Self.Children.Count == (HasLabel ? 6 : 5) ? Self.Children[^1] : null;
        //[expression, expression, expression, Label?, statement, else as Statement?]
        Driver.Verify(Self, ImportantASTNodeType.For);
        Driver.SETCHUNK();
        //entering chunk
        Driver.Exec(Self.Children[0]); //Compile initializing expression
        Driver.Emit(JMP, Driver.ACHUNK(1));

        //CHUNK1
        Driver.NewChunk();
        Driver.Exec(Self.Children[1]);//Compile conditional expression. This puts a 0 on the stack if false and a non-zero (probably 1 or 0xFF) onto the stack if true.
        Driver.Emit(BRZ, Driver.ACHUNK(2), Driver.ACHUNK(3));

        //CHUNK2
        Driver.NewChunk();
        Driver.Exec(Statement);
        Driver.Exec(Self.Children[2]); //Compile 3rd expression. This is what happens every loop; the i++, if you may.
        Driver.Emit(JMP, Driver.ACHUNK(1));

        //CHUNK3
        Driver.NewChunk();
        if (Else is not null)
        {
            Driver.Exec(Else);
        }
        Driver.Emit(JMP, Driver.ACHUNK(4));

        //CHUNK4
        Driver.NewChunk();
        Driver.Data.LoopData[(LoopGUID)Self.Attributes.LoopGUID!] = (Driver.ACHUNK(2), Driver.ACHUNK(3));
    }
}