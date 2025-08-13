using System.Diagnostics;
using Common.AST;
using Common.Evaluator;
using Common.Tokens;
using SmallLang.IR.Metadata;

namespace SmallLang.IR.AST.ASTVisitors;

using Node = DynamicASTNode<ImportantASTNodeType, Attributes>;
class PostProcessingVisitor : IDynamicASTVisitor<ImportantASTNodeType, Attributes>
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
            ImportantASTNodeType.ArgList => ArgList,
            ImportantASTNodeType.ArgListElement => Identity,

            ImportantASTNodeType.NewExpr => (x, y) => false,
            ImportantASTNodeType.GenericType => (x, y) => false,
            ImportantASTNodeType.TypeCSV => (x, y) => false,
            ImportantASTNodeType.BaseType => (x, y) => false,
            ImportantASTNodeType.Declaration => (x, y) => false,

            ImportantASTNodeType.AssignmentPrime => (x, y) => false,
            ImportantASTNodeType.DeclarationModifiersCombined => (x, y) => false,
            ImportantASTNodeType.Identifier => (x, y) => false,
            ImportantASTNodeType.DeclarationModifier => (x, y) => false,
            _ => throw new Exception()
        };
    }
    private bool ArgList(Node? parent, Node self)
    {
        if (self.Children.All(x => x.NodeType != ImportantASTNodeType.ArgListElement))
            return false;
        for (int i = 0; i < self.Children.Count; i++)
        {
            var child = self.Children[i];
            Debug.Assert(child.NodeType == ImportantASTNodeType.ArgListElement);
            if (child.Children.Count != 1)
            {
                throw new NotImplementedException("Arg Labels not yet supported"); //transform named into positional arguments here
            }
            self.Children[i] = child.Children.First();
        }
        return true;
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
