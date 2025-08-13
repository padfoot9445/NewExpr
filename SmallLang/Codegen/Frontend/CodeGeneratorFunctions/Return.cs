using System.Diagnostics;
using SmallLang.IR.LinearIR;
using SmallLang.Metadata;
namespace SmallLang.CodeGen.Frontend;

using static Opcode;
public partial class CodeGenerator
{
    private void ParseReturn(Node Self)
    {
        //[Expression]

        //CHUNK ENTERING
        DynamicDispatch(Self.Children[0]);
        Emit(RET);
    }
}