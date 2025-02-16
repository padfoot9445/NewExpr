using Common.AST;
using Common.Parser;
using Common.Tokens;

namespace SmallLang.Parser.InternalParsers;
class ReturnStatementParser(ParserData data) : BaseInternalParser(data)
{
    public override bool Parse(out DynamicASTNode<ASTNodeType, Attributes>? Node)
    {
        IToken ReturnToken;
        if (!((ReturnToken = Data.Advance()).TT == TokenType.Return) || !SafeParse(Expression, out var Expr) || !(Data.Advance().TT == TokenType.Semicolon))
        {
            Node = null;
            return false;
        }
        Node = new(
            ReturnToken,
            [Expr],
            ASTNodeType.Return
        );
        return true;
    }
}