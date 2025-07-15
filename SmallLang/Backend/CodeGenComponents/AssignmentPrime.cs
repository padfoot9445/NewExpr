using Common.AST;
using SmallLang.Metadata;

namespace SmallLang.Backend.CodeGenComponents;

class AssignmentPrime(CodeGenVisitor driver) : BaseCodeGenComponent(driver)
{
    public override void GenerateCode(Node? parent, Node self)
    {
        uint register = Driver.VariableNameToRegister[parent!.Attributes.VariableName!];
        Driver.SetState(true, register, null);
        Driver.Exec(self, self.Children[0]);
    }
}