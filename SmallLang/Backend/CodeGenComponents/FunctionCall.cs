using System.Diagnostics;
using Common.AST;
using SmallLang.Metadata;
using static SmallLang.LinearIR.Opcode;
namespace SmallLang.Backend.CodeGenComponents;

using Node = DynamicASTNode<ImportantASTNodeType, Attributes>;
class FunctionCall(CodeGenVisitor driver) : BaseCodeGenComponent(driver)
{
    void CheckArgTypes(Node? Arguments, Node self)
    {
        if (Arguments is null)
        {
            if (self.Attributes.DeclArgumentTypes!.Count != 0) throw new ExpaException("Expected arguments but got none");
            else return;
        }
        if (self.Attributes.DeclArgumentTypes!.Count != Arguments.Children.Count ||
            (self.Attributes.DeclArgumentTypes!.Zip(Arguments.Children.Select(x => x.Attributes.TypeOfExpression!), (x, y) => x == y).All(x => x) is false))
        {
            throw new ExpaException("Expected argument types and actual did not match.");
        }
    }
    public override void GenerateCode(Node? parent, Node self)
    {

        Debug.Assert(self.NodeType == ImportantASTNodeType.FunctionCall);
        var Function = self.Children[0];
        Node? Arguments = self.Children.Count == 2 ? self.Children[1] : null;
        CheckArgTypes(Arguments, self);
        PushArgsToStack(Arguments, self);
        FunctionID FunctionID = self.Attributes.FunctionID ?? throw new Exception();
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