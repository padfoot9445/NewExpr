using System.Diagnostics;
using Common.Tokens;
using SmallLang.IR.LinearIR;
using SmallLang.IR.Metadata;

namespace SmallLang.CodeGen.Frontend;

using static Opcode;
public partial class CodeGenerator
{
    private void ParseLoopCTRL(Node Self)
    {
        //[Data: Break | Continue; Label?]

        //ENTERINGCHUNK
        bool IsBreak = Self.Data!.TT == TokenType.Break;
        if (Self.Children.Count == 0)
        {
            if (IsBreak)
            {
                Emit(JMP, RCHUNK(2));
            }
            else
            {
                Emit(JMP, RCHUNK(-1));
            }
        }
        else
        {
            (var Break, var Cont) = Data.LoopData[(LoopGUID)Self.Attributes.GUIDOfLoopLabel!];
            if (IsBreak)
            {
                Emit(JMP, Break);
            }
            else
            {
                Emit(JMP, Cont);
            }
        }
    }
}