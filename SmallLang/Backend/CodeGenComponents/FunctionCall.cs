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
        Debug.Assert(Function.Attributes.DeclArguments!.Count == 0);
        Debug.Assert(Arguments.Children.Count == 0);
        uint FunctionID;
        if (Function.Attributes.IsActualFunctionName is true)
        {
            Debug.Assert(Function.NodeType == ImportantASTNodeType.Primary);
            Debug.Assert(Function.Data is not null);
            FunctionID = FunctionNameToID[Function.Data!.Literal];
            Emit(ICall, FunctionID);
        }
        else
        {
            Driver.Dispatch(Function)(self, Function); //pushes the FunctionID to the top of the stack
            Emit(SCall);
        }
        return true;


    }
}