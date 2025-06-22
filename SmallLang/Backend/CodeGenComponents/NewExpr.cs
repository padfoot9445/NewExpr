using Common.AST;
using SmallLang.Backend.CodeGenComponents;
using SmallLang.Metadata;
namespace SmallLang.Backend.CodeGenComponents;

class NewExpr : BaseCodeGenComponent
{
    public NewExpr(CodeGenVisitor driver) : base(driver)
    {
    }

    public override void GenerateCode(DynamicASTNode<ImportantASTNodeType, Attributes>? parent, DynamicASTNode<ImportantASTNodeType, Attributes> self)
    {
        throw new NotImplementedException();
    }
}