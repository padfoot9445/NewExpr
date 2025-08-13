using System.Diagnostics;
using Common.Tokens;
using SmallLang.IR.LinearIR;
using SmallLang.IR.Metadata;

namespace SmallLang.CodeGen.Frontend;

using static Opcode;
internal static class LoopCtrl
{
    public static void ParseLoopCTRL(Node Self, CodeGenerator Driver)
    {
        //[Data: Break | Continue; Label?]

        //ENTERINGCHUNK
        bool IsBreak = Self.Data!.TT == TokenType.Break;
        if (Self.Children.Count == 0)
        {
            if (IsBreak)
            {
                Driver.Emit(JMP, Driver.RCHUNK(2));
            }
            else
            {
                Driver.Emit(JMP, Driver.RCHUNK(-1));
            }
        }
        else
        {
            (var Break, var Cont) = Driver.Data.LoopData[(LoopGUID)Self.Attributes.GUIDOfLoopLabel!];
            if (IsBreak)
            {
                Driver.Emit(JMP, Break);
            }
            else
            {
                Driver.Emit(JMP, Cont);
            }
        }
    }
}