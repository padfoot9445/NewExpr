using System.Diagnostics;
using Common.AST;
using Common.Parser;

namespace SmallLang.Parser.InternalParsers;
class SectionParser(ParserData data) : BaseInternalParser(data)
{

    public override bool Parse(out DynamicASTNode<ASTNodeType, Attributes>? Node)
    {
        if (SafeParse(Statement, out var LeftNode))
        {
            if (SafeParse(this, out var RightNode))
            {
                if (RightNode is null)
                {
                    //if this parse passes through to statement
                    Node = LeftNode;
                    return true;
                }
                if (RightNode!.NodeType == ASTNodeType.Statement)
                {
                    Node = new(null, [LeftNode, RightNode], ASTNodeType.Section);
                }
                else
                {
                    Debug.Assert(RightNode!.NodeType == ASTNodeType.Section);
                    RightNode.Children.Insert(0, LeftNode!);
                    Node = RightNode;
                }
            }
        }
        Node = null;
        return true;
    }
}