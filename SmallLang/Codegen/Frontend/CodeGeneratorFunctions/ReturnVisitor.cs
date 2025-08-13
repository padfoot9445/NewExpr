using SmallLang.IR.LinearIR;
namespace SmallLang.CodeGen.Frontend.CodeGeneratorFunctions;

using static Opcode;
internal static class ReturnVisitor
{
    public static void Visit(Node Self, CodeGenerator Driver)
    {
        //[Expression]

        //CHUNK ENTERING
        Driver.Exec(Self.Children[0]);
        Driver.Emit(RET);
    }
}