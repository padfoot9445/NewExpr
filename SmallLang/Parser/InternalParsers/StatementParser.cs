using Common.AST;
using Common.Parser;
using Common.Tokens;

namespace SmallLang.Parser.InternalParsers;
class StatementParser(ParserData data) : BaseInternalParser(data)
{
    private BaseInternalParser[] Branches => [Loop, Cond, Function, Block, ReturnStatement, LCtrlStatement];
    public override bool Parse(out DynamicASTNode<ASTNodeType, Attributes>? Node)
    {
        if (SafeParse(Expression, out Node))
        {
            if (Data.TCmp(TokenType.Semicolon))
            {
                Data.Advance();
                return true;
            }
            else return false;
        }
        foreach (var branch in Branches)
        {
            if (SafeParse(branch, out Node))
            {
                return true;
            }
        }

        return false;
    }
}