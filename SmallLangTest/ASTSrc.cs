using Common.AST;
using Common.Tokens;
using SmallLang;
using SNode = Common.AST.DynamicASTNode<SmallLang.ImportantASTNodeType, SmallLang.Attributes>;
namespace SmallLangTest;
static class ASTSrc
{
    public static SNode Addition =>
    new SNode(
        TokenSrc.Addition, [Number, NumberTwo], ImportantASTNodeType.BinaryExpression


    );
    public static SNode Number => new SNode(
        TokenSrc.Number, [], ImportantASTNodeType.Primary
    );
    public static SNode NumberTwo => new SNode(
        TokenSrc.Number2, [], ImportantASTNodeType.Primary
    );
    public static SNode Multiplication => new SNode(
        TokenSrc.Multiplication, [NumberTwo, Number], ImportantASTNodeType.BinaryExpression
    );
}