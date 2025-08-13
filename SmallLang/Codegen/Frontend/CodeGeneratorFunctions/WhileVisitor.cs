using SmallLang.IR.AST;
using SmallLang.IR.LinearIR;
using SmallLang.IR.Metadata;

namespace SmallLang.CodeGen.Frontend.CodeGeneratorFunctions;

using static Opcode;
internal static class WhileVisitor
{
    public static void Visit(Node Self, CodeGenerator Driver)
    {
        bool HasLabel = Self.Children[1].NodeType == ImportantASTNodeType.LoopLabel;
        Node Statement = HasLabel ? Self.Children[2] : Self.Children[1];
        Node? Else = Self.Children.Count == (HasLabel ? 4 : 3) ? Self.Children[^1] : null;
        //[expression, Label?, statement, else as Statement?]
        Driver.Verify(Self, ImportantASTNodeType.While);
        Driver.SETCHUNK();
        //entering chunk
        Driver.Emit(JMP, Driver.ACHUNK(1));

        //CHUNK1
        Driver.NewChunk();
        Driver.DynamicDispatch(Self.Children[0]);//Compile conditional expression. This puts a 0 on the stack if false and a non-zero (probably 1 or 0xFF) onto the stack if true.
        Driver.Emit(BRZ, Driver.ACHUNK(2), Driver.ACHUNK(3));

        //CHUNK2
        Driver.NewChunk();
        Driver.DynamicDispatch(Statement);
        Driver.Emit(JMP, Driver.ACHUNK(1));

        //CHUNK3
        Driver.NewChunk();
        if (Else is not null)
        {
            Driver.DynamicDispatch(Else);
        }
        Driver.Emit(JMP, Driver.ACHUNK(4));

        //CHUNK4
        Driver.NewChunk();
        Driver.Data.LoopData[(LoopGUID)Self.Attributes.LoopGUID!] = (Driver.ACHUNK(2), Driver.ACHUNK(3));
    }
}