using Common.AST;
using Common.Parser;
using Common.Tokens;

namespace SmallLang.Parser.InternalParsers;
class FunctionParser(SmallLangParserData data) : BaseInternalParser(data)
{
    public override bool Parse(out DynamicASTNode<ASTNodeType, Attributes>? Node)
    {
        if (!SafeParse(Data.Type, out var TypeNode) || !Data.TryConsumeToken(TokenType.OpenParen) || !SafeParse(Data.TypeAndIdentifierCSVOrEmpty, out var ArgList) || !Data.TryConsumeToken(TokenType.CloseParen) || !SafeParse(Data.Statement, out var MethodBodyStatement))
        {
            Node = null;
            return false;
        }

        Node = new(
            null,
            ArgList is not null ? [TypeNode, ArgList, MethodBodyStatement] : [TypeNode, MethodBodyStatement],
            ASTNodeType.Function
        );
        return true;
    }
}