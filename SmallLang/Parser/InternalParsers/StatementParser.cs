using Common.AST;
using Common.Parser;
using Common.Tokens;

namespace SmallLang.Parser.InternalParsers;
class StatementParser(SmallLangParserData data) : BaseInternalParser(data)
{
    private BaseInternalParser[] Branches => [Data.Loop, Data.Cond, Data.Function, Data.Block, Data.ReturnStatement, Data.LCtrlStatement];
    public override bool Parse(out DynamicASTNode<ASTNodeType, Attributes>? Node)
    {
        if (SafeParse(Data.Expression, out Node) && Data.TryConsumeToken(TokenType.Semicolon))
        {
            return true;
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