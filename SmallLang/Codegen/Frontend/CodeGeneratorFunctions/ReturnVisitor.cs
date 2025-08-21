using SmallLang.IR.AST;
using SmallLang.IR.AST.Generated;
using SmallLang.IR.LinearIR;
namespace SmallLang.CodeGen.Frontend.CodeGeneratorFunctions;

internal static class ReturnVisitor
{
    public static void Visit(SmallLangNode Self, CodeGenerator Driver)
    {
        //[Expression]

        //CHUNK ENTERING
        Driver.Exec(Self.Children[0]);
        Driver.Emit(HighLevelOperation.Return());
    }
}