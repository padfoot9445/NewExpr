using System.Diagnostics;
using Common.AST;
using static SmallLang.LinearIR.Opcode;
namespace SmallLang.Backend.CodeGenComponents;
class Section(CodeGenVisitor driver) : BaseCodeGenComponent(driver)
{
    public override bool GenerateCode(DynamicASTNode<ImportantASTNodeType, Attributes>? parent, DynamicASTNode<ImportantASTNodeType, Attributes> self)
    {
        foreach (var child in self.Children)
        {
            Driver.Exec(self, child);
        }
        return true;
    }
}