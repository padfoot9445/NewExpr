using System.Diagnostics;
using SmallLang.LinearIR;

namespace SmallLang.Frontend.CodeGen;

public partial class CodeGenerator
{
    private void ParseFunction(Node Self)
    {
        Verify(Self, ImportantASTNodeType.Function);
        //assume that Entering chunk does not have JMP CHUNK1

        //CHUNK1
        Data.Sections.NewChunk();
        DynamicDispatch(Self.Children.Last());
    }
}