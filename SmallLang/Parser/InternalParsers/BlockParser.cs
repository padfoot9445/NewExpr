using Common.AST;
using Common.Parser;
using Common.Tokens;

namespace SmallLang.Parser.InternalParsers;
class BlockParser(SmallLangParserData data) : BaseInternalParser(data)
{
    public override bool Parse(out DynamicASTNode<ASTNodeType, Attributes>? Node)
    {
        if (Data.Advance().TT != TokenType.OpenCurly || !SafeParse(Data.Section, out Node) || Data.Advance().TT != TokenType.CloseCurly)
        {
            Node = null;
            return false;
        }
        return true;
    }
}