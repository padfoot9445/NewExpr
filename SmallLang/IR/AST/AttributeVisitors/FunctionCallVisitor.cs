using System.Diagnostics;
using SmallLang.IR.AST.Generated;

namespace SmallLang.IR.AST.AttributeVisitors;

public static partial class AttributeVisitor
{
    static void Evaluate(FunctionCallNode node)
    {
        node.VariableName = node.Identifier.VariableName;
        node.TypeOfExpression = GetIfNotNullRefReturn(FunctionSignatures, node.VariableName, (x, y) => x[y].RetVal);
        node.ExpectedTypeOfExpression = node.TypeOfExpression;
        node.FunctionID = GetIfNotNullRefReturn(FunctionSignatures, node.VariableName, (x, y) => x[y].ID);
    }
}