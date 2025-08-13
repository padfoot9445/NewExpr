using System.Diagnostics;
using SmallLang.IR.LinearIR;
using SmallLang.Metadata;
namespace SmallLang.CodeGen.Frontend;

using static Opcode;
internal static class ReturnVisitor
{
    public static void Visit(Node Self, CodeGenerator Driver)
    {
        //[Expression]

        //CHUNK ENTERING
        Driver.DynamicDispatch(Self.Children[0]);
        Driver.Emit(RET);
    }
}