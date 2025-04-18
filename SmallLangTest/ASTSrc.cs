using Common.AST;
using Common.Tokens;
using SmallLang;
namespace SmallLangTest;
static class ASTSrc
{
    public static DynamicASTNode<ImportantASTNodeType, T> Addition<T>() where T : IMetadata, new() =>
    new DynamicASTNode<ImportantASTNodeType, T>(
        TokenSrc.Addition, [Number<T>(), NumberTwo<T>()], ImportantASTNodeType.BinaryExpression
    );
    public static DynamicASTNode<ImportantASTNodeType, T> Number<T>() where T : IMetadata, new() => new DynamicASTNode<ImportantASTNodeType, T>(
        TokenSrc.Number, [], ImportantASTNodeType.Primary
    );
    public static DynamicASTNode<ImportantASTNodeType, T> NumberTwo<T>() where T : IMetadata, new() => new DynamicASTNode<ImportantASTNodeType, T>(
        TokenSrc.Number2, [], ImportantASTNodeType.Primary
    );
    public static DynamicASTNode<ImportantASTNodeType, T> Multiplication<T>() where T : IMetadata, new() => new DynamicASTNode<ImportantASTNodeType, T>(
        TokenSrc.Multiplication, [NumberTwo<T>(), Number<T>()], ImportantASTNodeType.BinaryExpression
    );
    public static DynamicASTNode<ImportantASTNodeType, T> AddMul<T>() where T : IMetadata, new() => new DynamicASTNode<ImportantASTNodeType, T>(TokenSrc.Addition, [Multiplication<T>(), NumberTwo<T>()], ImportantASTNodeType.BinaryExpression);
}