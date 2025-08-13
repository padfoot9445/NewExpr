using System.Diagnostics;
using SmallLang.LinearIR;
using SmallLang.Metadata;
namespace SmallLang.Frontend.CodeGen;

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