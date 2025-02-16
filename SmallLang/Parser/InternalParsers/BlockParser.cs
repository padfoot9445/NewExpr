using Common.AST;
using Common.Parser;
using Common.Tokens;

namespace SmallLang.Parser.InternalParsers;
class BlockParser(ParserData data) : BaseInternalParser(data)
{
    public override bool Parse(out DynamicASTNode<ASTNodeType, Attributes>? Node)
    {
        if (Data.Advance().TT != TokenType.OpenCurly || !SafeParse(Section, out Node) || Data.Advance().TT != TokenType.CloseCurly)
        {
            Node = null;
            return false;
        }
        return true;
    }
}