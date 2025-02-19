using Common.AST;
using Common.Parser;
using Common.Tokens;

namespace SmallLang.Parser.InternalParsers;
class LoopControlStatementParser(SmallLangParserData data) : BaseInternalParser(data)
{
    private ICollection<TokenType> KWs = [TokenType.Break, TokenType.Continue];
    public override bool Parse(out DynamicASTNode<ASTNodeType, Attributes>? Node)
    {
        if (KWs.Contains(Data.Peek().TT))
        {
            if (SafeParse(Data.OpValInLCTRL, out var OpVal))
            {
                Node = new(
                    Data.Advance(),
                    OpVal is null ? [] : [OpVal],
                    ASTNodeType.LoopCTRL
                );
                return true;
            }
        }
        Node = null;
        return false;
    }
}