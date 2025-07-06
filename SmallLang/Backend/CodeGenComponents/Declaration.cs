using Common.AST;
using SmallLang.Backend.CodeGenComponents;
using SmallLang.Metadata;

namespace SmallLang.Backend;

using Node = DynamicASTNode<ImportantASTNodeType, Attributes>;
class Declaration : BaseCodeGenComponent
{
    public Declaration(CodeGenVisitor driver) : base(driver)
    {
    }

    public override void GenerateCode(Node? parent, Node self)
    {
        bool IsModified = self.Children[0].NodeType == ImportantASTNodeType.DeclarationModifiersCombined;
        bool HasAssignment = self.Children[^1].NodeType == ImportantASTNodeType.AssignmentPrime;
        if (IsModified && HasAssignment)
        {
            GenCodeDecl(self.Children[0], self.Children[1], self.Children[2]);
        }
    }
    void GenCodeDecl(Node Modifiers, Node Type, Node AssignmentExpr)
    {

    }
}