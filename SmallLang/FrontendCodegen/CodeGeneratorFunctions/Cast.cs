using System.Diagnostics;
using SmallLang.LinearIR;
using SmallLang.Metadata;
namespace SmallLang.Frontend.CodeGen;

using static Opcode;
public partial class CodeGenerator
{
    void Cast(Node self, SmallLangType dstType)
    {
        if (self.Attributes.TypeLiteralType! == dstType) DynamicDispatch(self);
        throw new NotImplementedException();
    }
}