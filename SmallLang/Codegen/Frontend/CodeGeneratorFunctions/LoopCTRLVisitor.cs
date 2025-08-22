using Common.Dispatchers;
using Common.Tokens;
using SmallLang.IR.AST;
using SmallLang.IR.AST.Generated;
using SmallLang.IR.LinearIR;
using SmallLang.IR.Metadata;
using OpCode = SmallLang.IR.LinearIR.HighLevelOperation;
namespace SmallLang.CodeGen.Frontend.CodeGeneratorFunctions;

internal static class LoopCtrlVisitor
{
    public static void Visit(LoopCTRLNode Self, CodeGenerator Driver)
    {
        Driver.EnteringChunk(() =>
        {

            (var v1, var v2, var v3, var v4, var v5) = Driver.Data.LoopData[(LoopGUID)Self.Attributes.GUIDOfLoopLabel!];
            Self.Data.Switch(
                Accessor: x => x.TT,
                Comparer: (x, y) => x == y,
                (TokenType.Break, () => Driver.Emit(OpCode.Break(v1, v2, v3, v4, v5))),
                (TokenType.Continue, () => Driver.Emit(OpCode.Continue(v1, v2, v3, v4, v5)))
            )();

            Driver.Next();
        });
    }
}