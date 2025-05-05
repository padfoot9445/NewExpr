using System.Diagnostics;
using Common.AST;
using static SmallLang.LinearIR.Opcode;
namespace SmallLang.Backend.CodeGenComponents;
class FunctionCall(CodeGenVisitor driver) : BaseCodeGenComponent(driver)
{
    public override bool GenerateCode(DynamicASTNode<ImportantASTNodeType, Attributes>? parent, DynamicASTNode<ImportantASTNodeType, Attributes> self)
    {

        Debug.Assert(self.NodeType == ImportantASTNodeType.FunctionCall);
        var Function = self.Children[0];
        var Arguments = self.Children[1];
        Debug.Assert(Arguments.NodeType == ImportantASTNodeType.ArgList);
        Debug.Assert(Function.Attributes.DeclArgumentTypes!.Count == 0);
        Debug.Assert(Arguments.Children.Count == 0);
        uint FunctionID = Function.Attributes.FunctionID ?? throw new Exception();
        if (Function.NodeType == ImportantASTNodeType.FunctionIdentifier)
        {
            Debug.Assert(Function.Data is not null);
            Emit(ICall, FunctionID);
        }
        else
        {
            throw new Exception();
        }
        return true;


    }
}