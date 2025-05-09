using System.Diagnostics;
using Common.AST;
using static SmallLang.LinearIR.Opcode;
namespace SmallLang.Backend.CodeGenComponents;
class FunctionCall(CodeGenVisitor driver) : BaseCodeGenComponent(driver)
{
    public override void GenerateCode(DynamicASTNode<ImportantASTNodeType, Attributes>? parent, DynamicASTNode<ImportantASTNodeType, Attributes> self)
    {

        Debug.Assert(self.NodeType == ImportantASTNodeType.FunctionCall);
        var Function = self.Children[0];
        DynamicASTNode<ImportantASTNodeType, Attributes>? Arguments = self.Children.Count == 2 ? self.Children[1] : null;
        Debug.Assert(self.Attributes.DeclArgumentTypes!.Count == 0);
        Debug.Assert(Arguments is null);
        uint FunctionID = self.Attributes.FunctionID ?? throw new Exception();
        if (Function.NodeType == ImportantASTNodeType.FunctionIdentifier)
        {
            Debug.Assert(Function.Data is not null);
            if (Driver.OutputToRegister)
            {
                Emit(ICallR, FunctionID, GetDestRegister());
            }
            Emit(ICallS, FunctionID);
        }
        else
        {
            throw new Exception();
        }


    }
}