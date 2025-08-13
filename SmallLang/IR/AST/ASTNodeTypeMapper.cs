namespace SmallLang.IR.AST;

public static class ASTNodeTypeMapper
{
    public static ImportantASTNodeType ToImportant(this ASTNodeType NodeType)
    {
        switch (NodeType)
        {
            case ASTNodeType.ReTypingAlias: return ImportantASTNodeType.ReTypingAlias;
            case ASTNodeType.ReTypeOriginal: return ImportantASTNodeType.ReTypeOriginal;
            case ASTNodeType.Identifier: return ImportantASTNodeType.Identifier;
            case ASTNodeType.Section: return ImportantASTNodeType.Section;
            case ASTNodeType.Function: return ImportantASTNodeType.Function;
            case ASTNodeType.LoopCTRL: return ImportantASTNodeType.LoopCTRL;
            case ASTNodeType.For: return ImportantASTNodeType.For;
            case ASTNodeType.While: return ImportantASTNodeType.While;
            case ASTNodeType.ValInLCTRL: return ImportantASTNodeType.ValInLCTRL;
            case ASTNodeType.LoopLabel: return ImportantASTNodeType.LoopLabel;
            case ASTNodeType.Return: return ImportantASTNodeType.Return;
            case ASTNodeType.BaseType: return ImportantASTNodeType.BaseType;
            case ASTNodeType.GenericType: return ImportantASTNodeType.GenericType;
            case ASTNodeType.If: return ImportantASTNodeType.If;
            case ASTNodeType.Switch: return ImportantASTNodeType.Switch;
            case ASTNodeType.ExprStatementCombined: return ImportantASTNodeType.ExprStatementCombined;
            case ASTNodeType.TypeAndIdentifierCSV: return ImportantASTNodeType.TypeAndIdentifierCSV;
            case ASTNodeType.TypeAndIdentifierCSVElement: return ImportantASTNodeType.TypeAndIdentifierCSVElement;
            case ASTNodeType.AliasExpr: return ImportantASTNodeType.AliasExpr;
            case ASTNodeType.Declaration: return ImportantASTNodeType.Declaration;
            case ASTNodeType.DeclarationModifiersCombined: return ImportantASTNodeType.DeclarationModifiersCombined;
            case ASTNodeType.DeclarationModifier: return ImportantASTNodeType.DeclarationModifier;
            case ASTNodeType.FunctionArgDeclModifiers: return ImportantASTNodeType.FunctionArgDeclModifiers;
            case ASTNodeType.FunctionArgDeclModifiersCombined: return ImportantASTNodeType.FunctionArgDeclModifiersCombined;
            case ASTNodeType.AssignmentPrime: return ImportantASTNodeType.AssignmentPrime;
            case ASTNodeType.FactorialExpression: return ImportantASTNodeType.FactorialExpression;
            case ASTNodeType.BinaryExpression: return ImportantASTNodeType.BinaryExpression;
            case ASTNodeType.ComparisionExpression: return ImportantASTNodeType.ComparisionExpression;
            case ASTNodeType.OperatorExpressionPair: return ImportantASTNodeType.OperatorExpressionPair;
            case ASTNodeType.Primary: return ImportantASTNodeType.Primary;
            case ASTNodeType.CopyExpr: return ImportantASTNodeType.CopyExpr;
            case ASTNodeType.NewExpr: return ImportantASTNodeType.NewExpr;
            case ASTNodeType.Index: return ImportantASTNodeType.Index;
            case ASTNodeType.FunctionCall: return ImportantASTNodeType.FunctionCall;
            case ASTNodeType.ArgList: return ImportantASTNodeType.ArgList;
            case ASTNodeType.ArgListElement: return ImportantASTNodeType.ArgListElement;
            case ASTNodeType.TypeCSV: return ImportantASTNodeType.TypeCSV;
            case ASTNodeType.UnaryExpression: return ImportantASTNodeType.UnaryExpression;
            default: throw new ArgumentException($"Nodetype {NodeType} is not important; failed to convert");
        }
    }
}