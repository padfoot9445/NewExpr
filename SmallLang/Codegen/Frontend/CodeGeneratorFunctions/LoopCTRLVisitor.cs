using Common.Tokens;
using SmallLang.IR.AST;
using SmallLang.IR.AST.Generated;
using SmallLang.IR.LinearIR;
using SmallLang.IR.Metadata;

namespace SmallLang.CodeGen.Frontend.CodeGeneratorFunctions;

internal static class LoopCtrlVisitor
{
    public static void Visit(SmallLangNode Self, CodeGenerator Driver)
    {
        //[Data: Break | Continue; Label?]

        //ENTERINGCHUNK
        bool IsBreak = Self.Data!.TT == TokenType.Break;
        if (Self.Children.Count == 0)
        {
            if (IsBreak)
            {
                Driver.Emit(HighLevelOperation.Jump(Driver.RCHUNK(2)));
            }
            else
            {
                Driver.Emit(HighLevelOperation.Jump(Driver.RCHUNK(-1)));
            }
        }
        else
        {
            (var Break, var Cont) = Driver.Data.LoopData[(LoopGUID)Self.Attributes.GUIDOfLoopLabel!];
            if (IsBreak)
            {
                Driver.Emit(HighLevelOperation.Jump(Break));
            }
            else
            {
                Driver.Emit(HighLevelOperation.Jump(Cont));
            }
        }
    }
}