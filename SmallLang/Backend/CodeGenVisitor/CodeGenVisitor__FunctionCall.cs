using System.Diagnostics;
using Common.AST;
using Common.LinearIR;
using static SmallLang.LinearIR.Opcode;
namespace SmallLang.Backend;
partial class CodeGenVisitor
{
    protected override bool FunctionCall(DynamicASTNode<ImportantASTNodeType, Attributes>? Parent, DynamicASTNode<ImportantASTNodeType, Attributes> self)
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
            Dispatch(Function)(self, Function); //pushes the FunctionID to the top of the stack
            Emit(SCall);
        }
        return true;

    }
}