using System.Diagnostics;
using Common.AST;
using Common.Evaluator;
using Common.Tokens;
namespace SmallLang;

using Node = DynamicASTNode<ImportantASTNodeType, Attributes>;
class OptimisingVisitor : IDynamicASTVisitor<ImportantASTNodeType, Attributes>
{
    Func<Node?, Node, bool> Identity = (x, y) => false;
    public Func<Node?, Node, bool> Dispatch(Node node)
    {
        return node.NodeType switch
        {
            ImportantASTNodeType.FunctionCall => FunctionCall,
            ImportantASTNodeType.Section => (x, y) => false,
            ImportantASTNodeType.FunctionIdentifier => (x, y) => false,
            ImportantASTNodeType.Primary => Identity,
            _ => throw new Exception()
        };
    }
    private bool FunctionCall(Node? parent, Node self)
    {
        if (self.Children[0].NodeType == ImportantASTNodeType.FunctionIdentifier) return false;
        Debug.Assert(self.Children[0].NodeType == ImportantASTNodeType.Identifier);
        Debug.Assert(self.Children[0].Data is IToken token && token.Lexeme is not null);
        self.Children[0] = self.Children[0] with { NodeType = ImportantASTNodeType.FunctionIdentifier };
        return true;
    }
}
