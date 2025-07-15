using Common.AST;
using SmallLang.Backend.CodeGenComponents;
using SmallLang.Constants;
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
        //Variables, for now, can only reside on register. Add stack support if that is ever neccesary.

        Driver.SetState(true, null, null);
        //bool IsModified = self.Children[0].NodeType == ImportantASTNodeType.DeclarationModifiersCombined;
        //doing IsModified is not necessary
        bool HasAssignment = self.Children[^1].NodeType == ImportantASTNodeType.AssignmentPrime;
        Node Type = HasAssignment ? self.Children[^2] : self.Children[^1];

        Driver.VariableNameToRegister[self.Attributes.VariableName!] = Driver.LastUsedRegister + 1;
        Driver.LastUsedRegister += Type.Attributes.TypeLiteralType!.Size;
        if (HasAssignment)
        {
            Driver.Exec(self, self.Children[^1]);
            //no need to specify dst register through the driver interface since it should be known what register to put it to via the nametoreg dict which we have just updated.
        }
    }
}