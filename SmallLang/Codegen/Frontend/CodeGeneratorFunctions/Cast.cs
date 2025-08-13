using System.Diagnostics;
using SmallLang.IR.LinearIR;
using SmallLang.IR.Metadata;

namespace SmallLang.CodeGen.Frontend;

using static Opcode;
public partial class CodeGenerator
{
    void Cast(Node self, SmallLangType dstType)
    {
        if (self.Attributes.TypeLiteralType! == dstType) DynamicDispatch(self);
        throw new NotImplementedException();
    }
}