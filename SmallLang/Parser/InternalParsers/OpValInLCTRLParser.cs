using Common.AST;
using Common.Parser;
using Common.Tokens;

namespace SmallLang.Parser.InternalParsers;
class OpValInLCTRLParser(ParserData data) : BaseInternalParser(data)
{
    public override bool Parse(out DynamicASTNode<ASTNodeType, Attributes>? Node)
    {
        Node = null;
        if (Data.TCmp(TokenType.Identifier))
        {
            Node = new(
                Data.Advance(),
                [],
                ASTNodeType.OpValInLCTRL
            );
        }
        return true;
    }
}