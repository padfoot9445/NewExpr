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
    public static IValidASTLeaf MinimizeToAST(this IValidASTLeaf node)
    {
        throw new NotImplementedException();
    }
    /// <summary>
    /// Removes nodes which have only one or zero children. NOTE: Will remove attributes of terminals and attributes of removed nodes
    /// </summary>
    /// <param name="leaf"></param>
    /// <returns></returns>
    public static IValidASTLeaf MinimizeRemoveUnnecessaryNodes(this IValidASTLeaf leaf)
    {
        if (leaf is not ASTNode node) return leaf;
        var Children = node.Children.Select(x => x.Descend()).Where(x => x is not null).Select(x => x!.MinimizeRemoveUnnecessaryNodes());
        string Name = "Minimum-Tree-" + node.Name;
        var Pattern = Children.Select(x => x.Type);
        return new ASTNode(Pattern, Children, Name);
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
    public static bool MatchDescendEnd(this ASTNode focus, ASTLeafType[] pattern)
    {
        //we assume that terminals are indented by one and non-terminals are not indented at all; we are trying to match against the reduced version, so we can just use descend. Is this inefficient? Yes - but is this easier? Also yes
        if (pattern.Length != focus.Pattern.Length) return false; //also guarantees equality with children's length
        for (int i = 0; i < focus.Pattern.Length; i++)
        {
            var descended = focus.Children[i].Descend();
            if (descended is null || pattern[i] != descended.Type)
            {
                return false;
            }
        }
        return true;
    }
}