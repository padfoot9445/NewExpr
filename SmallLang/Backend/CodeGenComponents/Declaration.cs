using Common.AST;
using SmallLang.Backend.CodeGenComponents;
using SmallLang.Metadata;

namespace SmallLang.Backend;

class Declaration : BaseCodeGenComponent
{
    public Declaration(CodeGenVisitor driver) : base(driver)
    {
    }

    public override void GenerateCode(DynamicASTNode<ImportantASTNodeType, Attributes>? parent, DynamicASTNode<ImportantASTNodeType, Attributes> self)
    {
        throw new NotImplementedException();
    }
}