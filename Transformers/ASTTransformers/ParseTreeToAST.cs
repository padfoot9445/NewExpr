using Common.AST;
using Common.Tokens;

namespace Transformers.ASTTransformers;
public static class ParseTreeToAST
{
    /// <summary>
    /// Returns a copy of the Parse Tree but minimized to AST form.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public static ASTNode MinimizeToAST(this ASTNode node)
    {
        throw new NotImplementedException();
    }
    /// <summary>
    /// Removes nodes which have only one or zero children. NOTE: Will remove attributes of terminals and attributes of removed nodes
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public static ASTNode MinimizeRemoveUnnecessaryNodes(this ASTNode node)
    {
        throw new Exception();
    }
    public static bool IsRedundant(this IValidASTLeaf leaf)
    {
        if (leaf is IToken) { return false; }
        else if (leaf is ASTNode node)
        {
            return node.Children.Length <= 1;
        }
        else
        {
            throw new Exception("Unexpected Type");
        }
    }
    public static IValidASTLeaf? Descend(this IValidASTLeaf leaf)
    {
        if (leaf is not ASTNode node) { return leaf; }
        while (node.IsRedundant())
        {
            if (node.Children.Length == 0)
            {
                return null;
            }
            else if (node.Children[0] is IToken TNode)
            {
                return TNode;
            }
            else if (node.Children[0] is ASTNode CNode)
            {
                node = CNode;
            }
        }
        return node;
    }
    public static ASTNode Copy(this ASTNode node) => new(node.Pattern, node.Children, node.Name);

}