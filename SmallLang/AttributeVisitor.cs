using Common.AST;
using Common.Evaluator;
namespace SmallLang;

using Node = DynamicASTNode<ImportantASTNodeType, Attributes>;
class AttributeVisitor : IDynamicASTVisitor<ImportantASTNodeType, Attributes>
{
    Dictionary<uint, List<uint>> FunctionToFunctionArgs = new()
    {
        [1] = []
    };
    Dictionary<string, uint> FunctionNameToFunctionID = new()
    {
        ["input"] = 1
    };
    public Func<Node?, Node, bool> Dispatch(Node node)
    {
        return node.NodeType switch
        {
            ImportantASTNodeType.FunctionCall => FunctionCall,
            _ => throw new Exception()
        };
    }
    bool Changed(Attributes oldattr, Attributes newattr) => (oldattr == newattr) is false;
    private bool FunctionCall(Node? parent, Node self)
    {
        uint ID = FunctionNameToFunctionID[self.Children[0].Data!.Lexeme];
        var oldattr = self.Attributes;
        self.Attributes = self.Attributes with { FunctionID = ID, DeclArgumentTypes = FunctionToFunctionArgs[ID] };
        return Changed(oldattr, self.Attributes);
    }
}
