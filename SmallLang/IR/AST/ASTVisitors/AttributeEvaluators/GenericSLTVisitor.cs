using Common.Tokens;
using SmallLang.Exceptions;
using SmallLang.IR.AST.Generated;
using SmallLang.IR.Metadata;

namespace SmallLang.IR.AST.ASTVisitors.AttributeEvaluators;

internal class GenericSLTypeVisitor : BaseASTVisitor
{
    protected override void PreVisit(ISmallLangNode node)
    {
        AssertPropertyIsNotNull<IHasAttributeTypeLiteralType>(x => x.TypeLiteralType is not null);
        AssertPropertyIsNotNull<IHasAttributeVariableName>(x => x.VariableName is not null);
        AssertPropertyIsNotNull<IHasAttributeScope>(x => x.Scope is not null);
        base.PreVisit(node);
    }

    private static GenericSmallLangType GetGenericSLTFromLiteralType(ITypeNode type)
    {
        NotNull(type.TypeLiteralType);
        return GenericSmallLangType.ParseType(type);
    }

    protected override ISmallLangNode VisitReTypingAlias(ISmallLangNode? Parent, ReTypingAliasNode self)
    {
        NotNull(self.Identifier2.Scope, self.Identifier2.VariableName, self.Type.TypeLiteralType);
        self.GenericSLType = GetGenericSLTFromLiteralType(self.Type);
        self.Identifier2.Scope.DefineTypeOfName(self.Identifier2.VariableName, GetGenericSLTFromLiteralType(self.Type));
        return base.VisitReTypingAlias(Parent, self);
    }

    protected override ISmallLangNode VisitReTypeOriginal(ISmallLangNode? Parent, ReTypeOriginalNode self)
    {
        self.GenericSLType = GetGenericSLTFromLiteralType(self.Type);
        return base.VisitReTypeOriginal(Parent, self);
    }

    protected override ISmallLangNode VisitIdentifier(ISmallLangNode? Parent, IdentifierNode self)
    {
        NotNull(self.VariableName, self.Scope);
        self.Scope.TryGetTypeOfVariable(self.VariableName, out var VarType);

        self.GenericSLType ??= VarType;

        return base.VisitIdentifier(Parent, self);
    }

    protected override ISmallLangNode VisitAliasExpr(ISmallLangNode? Parent, AliasExprNode self)
    {
        NotNull(self.Identifier2.Scope, self.Identifier2.VariableName);
        self.GenericSLType = self.Identifier1.GenericSLType;
        if (self.GenericSLType is not null)
            self.Identifier2.Scope.DefineTypeOfName(self.Identifier2.VariableName, self.GenericSLType);

        return base.VisitAliasExpr(Parent, self);
    }

    protected override ISmallLangNode VisitDeclaration(ISmallLangNode? Parent, DeclarationNode self)
    {
        NotNull(self.Type.TypeLiteralType, self.Identifier.Scope, self.Identifier.VariableName);
        self.GenericSLType = GetGenericSLTFromLiteralType(self.Type);

        self.Identifier.Scope.DefineTypeOfName(self.Identifier.VariableName, self.GenericSLType);
        return base.VisitDeclaration(Parent, self);
    }

    protected override ISmallLangNode VisitFactorialExpression(ISmallLangNode? Parent, FactorialExpressionNode self)
    {
        self.GenericSLType = self.Expression.GenericSLType;
        return base.VisitFactorialExpression(Parent, self);
    }

    protected override ISmallLangNode VisitBinaryExpression(ISmallLangNode? Parent, BinaryExpressionNode self)
    {
        if (self.Data.TT == TokenType.Equals)
        {
            if (self.Right is PrimaryNode
                {
                    GenericSLType:
                    {
                        IsLeafNode: true, OutmostType.IsNum: true
                    }
                } rightPrimaryNode)
                rightPrimaryNode.GenericSLType = self.Left.GenericSLType;

            self.GenericSLType = self.Left.GenericSLType;
        }
        else if (self.Left.GenericSLType is not null && self.Right.GenericSLType is not null)
        {
            self.GenericSLType = self.Left.GenericSLType.GreatestCommonType(self.Right.GenericSLType);
        }

        return base.VisitBinaryExpression(Parent, self);
    }

    protected override ISmallLangNode VisitComparisonExpression(ISmallLangNode? Parent, ComparisonExpressionNode self)
    {
        self.GenericSLType = new GenericSmallLangType(TypeData.Bool);
        return base.VisitComparisonExpression(Parent, self);
    }

    protected override ISmallLangNode VisitPrimary(ISmallLangNode? Parent, PrimaryNode self)
    {
        self.GenericSLType ??= new GenericSmallLangType(self.Data.TT switch
        {
            TokenType.String => self.Data.Lexeme.Length == 1 ? TypeData.Char : TypeData.String,
            TokenType.Number => self.Data.Lexeme.Contains('.') ? TypeData.Number : TypeData.Longint,
            TokenType.TrueLiteral or TokenType.FalseLiteral => TypeData.Bool,
            _ => throw new ExpaException($"Token {self.Data} was not recognized when attempting to determine its type")
        });
        return base.VisitPrimary(Parent, self);
    }

    protected override ISmallLangNode VisitCopyExpr(ISmallLangNode? Parent, CopyExprNode self)
    {
        self.GenericSLType = self.Expression.GenericSLType;
        return base.VisitCopyExpr(Parent, self);
    }

    protected override ISmallLangNode VisitNewExpr(ISmallLangNode? Parent, NewExprNode self)
    {
        self.GenericSLType = GetGenericSLTFromLiteralType(self.Type);
        return base.VisitNewExpr(Parent, self);
    }

    protected override ISmallLangNode VisitIndex(ISmallLangNode? Parent, IndexNode self)
    {
        self.GenericSLType = self.Expression1.GenericSLType?.OutmostType == TypeData.Dict
            ? self.Expression1.GenericSLType.ChildNodes.ElementAt(1)
            : self.Expression1.GenericSLType?.ChildNodes.ElementAt(0);
        return base.VisitIndex(Parent, self);
    }

    protected override ISmallLangNode VisitFunctionCall(ISmallLangNode? Parent, FunctionCallNode self)
    {
        self.GenericSLType = self.Scope?.GetSignature(self.Identifier.Data.Lexeme).RetVal;
        return base.VisitFunctionCall(Parent, self);
    }

    protected override ISmallLangNode VisitUnaryExpression(ISmallLangNode? Parent, UnaryExpressionNode self)
    {
        self.GenericSLType = self.Expression.GenericSLType;
        return base.VisitUnaryExpression(Parent, self);
    }

    protected override ISmallLangNode VisitLoopCTRL(ISmallLangNode? Parent, LoopCTRLNode self)
    {
        if (self.Identifier is not null)
        {
            NotNull(self.Scope, self.Identifier?.VariableName);
            self.Scope.DefineTypeOfName(self.Identifier.VariableName, new GenericSmallLangType(TypeData.Void));
        }

        return base.VisitLoopCTRL(Parent, self);
    }

    protected override ISmallLangNode VisitLoopLabel(ISmallLangNode? Parent, LoopLabelNode self)
    {
        if (self.Identifier is not null)
        {
            NotNull(self.Scope, self.Identifier?.VariableName);
            self.Scope.DefineTypeOfName(self.Identifier.VariableName, new GenericSmallLangType(TypeData.Void));
        }

        return base.VisitLoopLabel(Parent, self);
    }

    protected override ISmallLangNode VisitFunction(ISmallLangNode? Parent, FunctionNode self)
    {
        NotNull(self.Scope, self.FunctionName.VariableName);

        self.Scope.DefineTypeOfName(self.FunctionName.VariableName, new GenericSmallLangType(TypeData.Void));

        foreach (var node in self.TypeAndIdentifierCSV)
            self.FunctionBody.Scope!.DefineTypeOfName(node.Identifier.VariableName!, node.Type.TypeLiteralType!);
        return base.VisitFunction(Parent, self);
    }

    protected override ISmallLangNode VisitTypeAndIdentifierCSVElement(ISmallLangNode? Parent,
        TypeAndIdentifierCSVElementNode self)
    {
        self.Identifier.GenericSLType = self.Type.TypeLiteralType;
        return base.VisitTypeAndIdentifierCSVElement(Parent, self);
    }

    protected override ISmallLangNode VisitArgListElement(ISmallLangNode? Parent, ArgListElementNode self)
    {
        if (self.Identifier is not null) self.Identifier.GenericSLType = new GenericSmallLangType(TypeData.Void);

        return base.VisitArgListElement(Parent, self);
    }

    protected override ISmallLangNode VisitAssignmentPrime(ISmallLangNode? Parent, AssignmentPrimeNode self)
    {
        self.GenericSLType = self.Expression.GenericSLType;
        return base.VisitAssignmentPrime(Parent, self);
    }
}