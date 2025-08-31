using SmallLang.IR.AST.Generated;

namespace SmallLang.IR.AST.AttributeVisitors;

public static partial class AttributeVisitor
{
    static void Evaluate(DeclarationNode node)
    {
        node.TypeOfExpression = node.Type.TypeLiteralType;

        DoIfNotNull(() => Assert(node.AssignmentPrime!.TypeOfExpression!.CanDeclareTo(node.TypeOfExpression!)), node.AssignmentPrime!.TypeOfExpression, node.TypeOfExpression, node.AssignmentPrime);

        DoIfNotNull(() => node.VariableName = node.Scope!.GetName(node.Data.Lexeme), node.Scope);

        DoIfNotNull(() => node.Scope!.Define(node.VariableName!, node.TypeOfExpression!), node.Scope, node.VariableName, node.TypeOfExpression);
    }
}