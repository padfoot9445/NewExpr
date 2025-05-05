using System.Diagnostics;
using Common.AST;
using Common.Evaluator;
using Common.Tokens;
namespace SmallLang;

using Node = DynamicASTNode<ImportantASTNodeType, Attributes>;
class OptimisingVisitor : IDynamicASTVisitor<ImportantASTNodeType, Attributes>
{
    public Func<Node?, Node, bool> Dispatch(Node node)
    {
        return node.NodeType switch
        {
            ImportantASTNodeType.FunctionCall => FunctionCall,
            _ => throw new Exception()
        };
    }
    private bool FunctionCall(Node? parent, Node self)
    {
        Debug.Assert(self.Children[0].NodeType == ImportantASTNodeType.Primary);
        Debug.Assert(self.Children[0].Data is IToken token && token.Lexeme is not null);
        if (self.Children[0].NodeType == ImportantASTNodeType.FunctionIdentifier) return false;
        self.Children[0] = self.Children[0] with { NodeType = ImportantASTNodeType.FunctionIdentifier };
        return true;
    }
}
