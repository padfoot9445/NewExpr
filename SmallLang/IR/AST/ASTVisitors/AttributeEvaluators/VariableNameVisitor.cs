using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Common.AST;
using SmallLang.Exceptions;
using SmallLang.IR.AST.Generated;

namespace SmallLang.IR.AST.ASTVisitors.AttributeEvaluators;

internal class VariableNameVisitor : BaseASTVisitor
{
    private const string PlaceholderVariableNameName = "__FUNCTION_NAME_TO_BE_ASSIGNED";
    protected override void PreVisit(ISmallLangNode node)
    {
        Debug.Assert(node.Flatten().All(x => x.Scope is not null));
        //TODO: Define stdlib identifiers
    }
    protected override void PostVisit(ISmallLangNode node)
    {

        Func<IdentifierNode, string> GetErrorMessage = self => $"Identifier {self.Data.Lexeme} was not defined before use at Line {self.Data.Line}, Position {self.Data.Position}.";

        StringBuilder UltimateErrorMessage = new();
        foreach (var NullIdentifierNode in node.Flatten().OfType<IHasAttributeVariableName>().Where(x => x.VariableName!.Name == PlaceholderVariableNameName))
        {
            UltimateErrorMessage.AppendLine(NullIdentifierNode is IdentifierNode IDNode ? GetErrorMessage(IDNode) : $"NodeType {NullIdentifierNode.GetType()} had an identifier which was not recognized"); //should really only be IdentifierNodes here
        }
        throw new ExpaException(UltimateErrorMessage.ToString());

    }
    private static void NotNull([NotNull] object? o1)
    {
        Debug.Assert(o1 is not null);
    }

    protected override ISmallLangNode VisitAliasExpr(ISmallLangNode? Parent, AliasExprNode self)
    {
        NotNull(self.Scope);
        //Identifier1 will handle itself
        self.Scope.DefineName(self.Identifier2.Data.Lexeme);
        //now Identifier2 will also handle itself
        return self;
    }

    protected override ISmallLangNode VisitArgListElement(ISmallLangNode? Parent, ArgListElementNode self)
    {
        //identifier's variablename won't be accurate, but it shouldn't matter
        return self;
    }

    protected override ISmallLangNode VisitAssignmentPrime(ISmallLangNode? Parent, AssignmentPrimeNode self) => self;

    protected override ISmallLangNode VisitBaseType(ISmallLangNode? Parent, BaseTypeNode self) => self;

    protected override ISmallLangNode VisitBinaryExpression(ISmallLangNode? Parent, BinaryExpressionNode self) => self;

    protected override ISmallLangNode VisitComparisonExpression(ISmallLangNode? Parent, ComparisonExpressionNode self) => self;

    protected override ISmallLangNode VisitCopyExpr(ISmallLangNode? Parent, CopyExprNode self) => self;

    protected override ISmallLangNode VisitDeclaration(ISmallLangNode? Parent, DeclarationNode self)
    {
        NotNull(self.Scope);
        self.Scope.DefineName(self.Identifier.Data.Lexeme);
        self.VariableName = self.Identifier.VariableName;
        return self;
    }

    protected override ISmallLangNode VisitDeclarationModifier(ISmallLangNode? Parent, DeclarationModifierNode self) => self;

    protected override ISmallLangNode VisitDeclarationModifiersCombined(ISmallLangNode? Parent, DeclarationModifiersCombinedNode self) => self;

    protected override ISmallLangNode VisitElse(ISmallLangNode? Parent, ElseNode self) => self;

    protected override ISmallLangNode VisitExprSectionCombined(ISmallLangNode? Parent, ExprSectionCombinedNode self) => self;

    protected override ISmallLangNode VisitFactorialExpression(ISmallLangNode? Parent, FactorialExpressionNode self) => self;

    protected override ISmallLangNode VisitFactorialSymbol(ISmallLangNode? Parent, FactorialSymbolNode self) => self;

    protected override ISmallLangNode VisitFor(ISmallLangNode? Parent, ForNode self) => self; //looplabel handles defining itself

    protected override ISmallLangNode VisitFunction(ISmallLangNode? Parent, FunctionNode self)
    {
        NotNull(self.Scope);

        self.Scope.DefineName(self.FunctionName.Data.Lexeme);

        return self;
    }

    protected override ISmallLangNode VisitFunctionArgDeclModifiers(ISmallLangNode? Parent, FunctionArgDeclModifiersNode self) => self;

    protected override ISmallLangNode VisitFunctionArgDeclModifiersCombined(ISmallLangNode? Parent, FunctionArgDeclModifiersCombinedNode self) => self;

    protected override ISmallLangNode VisitFunctionCall(ISmallLangNode? Parent, FunctionCallNode self)
    {
        self.Identifier.VariableName = new(PlaceholderVariableNameName); //hack to signal that we are going to assign the function name later. This should never clash with an actual variablename because all Variablenames begin with Global.
        return self;
    }

    protected override ISmallLangNode VisitGenericType(ISmallLangNode? Parent, GenericTypeNode self) => self;

    protected override ISmallLangNode VisitIdentifier(ISmallLangNode? Parent, IdentifierNode self)
    {
        NotNull(self.Scope);

        if (self.VariableName is null && !self.Scope.IsDefined(self.Data.Lexeme)) throw new ExpaException($"Identifier {self.Data.Lexeme} was not defined before use at Line {self.Data.Line}, Position {self.Data.Position}.");

        else if (self.Scope.IsDefined(self.Data.Lexeme))
        {
            self.VariableName = self.Scope.SearchName(self.Data.Lexeme);
        }
        else
        {
            Debug.Assert(self.VariableName is not null, message: self.Data.Lexeme);
            Debug.Assert(self.VariableName.Name == PlaceholderVariableNameName);
        }
        return self;
    }

    protected override ISmallLangNode VisitIf(ISmallLangNode? Parent, IfNode self) => self;

    protected override ISmallLangNode VisitIndex(ISmallLangNode? Parent, IndexNode self) => self;

    protected override ISmallLangNode VisitLoopCTRL(ISmallLangNode? Parent, LoopCTRLNode self) => self;

    protected override ISmallLangNode VisitLoopLabel(ISmallLangNode? Parent, LoopLabelNode self)
    {
        NotNull(self.Scope);

        self.Scope.DefineName(self.Identifier.Data.Lexeme);
        return self;
    }

    protected override ISmallLangNode VisitNewExpr(ISmallLangNode? Parent, NewExprNode self) => self;

    protected override ISmallLangNode VisitOperatorExpressionPair(ISmallLangNode? Parent, OperatorExpressionPairNode self) => self;

    protected override ISmallLangNode VisitPrimary(ISmallLangNode? Parent, PrimaryNode self) => self;

    protected override ISmallLangNode VisitReturn(ISmallLangNode? Parent, ReturnNode self) => self;

    protected override ISmallLangNode VisitReTypeOriginal(ISmallLangNode? Parent, ReTypeOriginalNode self) => self;

    protected override ISmallLangNode VisitReTypingAlias(ISmallLangNode? Parent, ReTypingAliasNode self)
    {
        self.Scope!.DefineName(self.Identifier2.Data.Lexeme);
        return self;
    }

    protected override ISmallLangNode VisitSection(ISmallLangNode? Parent, SectionNode self) => self;

    protected override ISmallLangNode VisitSwitch(ISmallLangNode? Parent, SwitchNode self) => self;

    protected override ISmallLangNode VisitTypeAndIdentifierCSVElement(ISmallLangNode? Parent, TypeAndIdentifierCSVElementNode self)
    {
        NotNull(self.Scope);

        self.Scope.DefineName(self.Identifier.Data.Lexeme);

        return self;
    }

    protected override ISmallLangNode VisitTypeCSV(ISmallLangNode? Parent, TypeCSVNode self) => self;

    protected override ISmallLangNode VisitUnaryExpression(ISmallLangNode? Parent, UnaryExpressionNode self) => self;

    protected override ISmallLangNode VisitWhile(ISmallLangNode? Parent, WhileNode self) => self;
}