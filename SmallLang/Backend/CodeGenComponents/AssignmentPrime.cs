using Common.AST;
using SmallLang.Metadata;

namespace SmallLang.Backend.CodeGenComponents;

class AssignmentPrime(CodeGenVisitor driver) : BaseCodeGenComponent(driver)
{
    public override void GenerateCode(DynamicASTNode<ImportantASTNodeType, Attributes>? parent, DynamicASTNode<ImportantASTNodeType, Attributes> self)
    {
        throw new NotImplementedException();
    }
}