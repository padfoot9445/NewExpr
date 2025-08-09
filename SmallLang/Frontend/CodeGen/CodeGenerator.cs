using System.Diagnostics;
using Common.Backend;
using static SmallLang.ImportantASTNodeType;

namespace SmallLang.Frontend.CodeGen;

public partial class CodeGenerator(Node RootNode)
{
    private readonly Data data = new();
    public Data Parse()
    {
        throw new NotImplementedException();
        return data;
    }
    private void Verify(Node node, ImportantASTNodeType Expected)
    {
        Debug.Assert(node.NodeType == Expected);
    }
    private void DynamicDispatch(Node node) =>
    Common.Backend.CodeGenerator.Dispatch(node,

    (Section, ParseSection)
    );
}