using Common;
using Common.Dispatchers;
using Common.Tokens;
using sly.lexer;
using SmallLang.IR.AST;
using SmallLang.IR.AST.Generated;
using SmallLang.IR.Metadata;
using SmallLang.TreeWalkInterpreter.RuntimeObjects;
using Boolean = SmallLang.TreeWalkInterpreter.RuntimeObjects.Boolean;
using String = SmallLang.TreeWalkInterpreter.RuntimeObjects.String;

namespace SmallLang.TreeWalkInterpreter;

public class TreeWalkInterpreter : ISmallLangNodeVisitor<Nothing>
{
    public InterpreterState State { get; } = new();
    private Nothing Push(IRunTimeObject o) => Nothing.DoNothing(() => State.Stack.Push(o));
    public Nothing Visit(ISmallLangNode? Parent, ReTypingAliasNode self)
    {
        throw new NotImplementedException();
    }

    public Nothing Visit(ISmallLangNode? Parent, ReTypeOriginalNode self)
    {
        throw new NotImplementedException();
    }

    public Nothing Visit(ISmallLangNode? Parent, IdentifierNode self)
    {
        throw new NotImplementedException();
    }

    public Nothing Visit(ISmallLangNode? Parent, SectionNode self)
    {
        throw new NotImplementedException();
    }

    public Nothing Visit(ISmallLangNode? Parent, FunctionNode self)
    {
        throw new NotImplementedException();
    }

    public Nothing Visit(ISmallLangNode? Parent, LoopCTRLNode self)
    {
        throw new NotImplementedException();
    }

    public Nothing Visit(ISmallLangNode? Parent, ForNode self)
    {
        throw new NotImplementedException();
    }

    public Nothing Visit(ISmallLangNode? Parent, WhileNode self)
    {
        throw new NotImplementedException();
    }

    public Nothing Visit(ISmallLangNode? Parent, LoopLabelNode self)
    {
        throw new NotImplementedException();
    }

    public Nothing Visit(ISmallLangNode? Parent, ReturnNode self)
    {
        throw new NotImplementedException();
    }

    public Nothing Visit(ISmallLangNode? Parent, BaseTypeNode self)
    {
        throw new NotImplementedException();
    }

    public Nothing Visit(ISmallLangNode? Parent, GenericTypeNode self)
    {
        throw new NotImplementedException();
    }

    public Nothing Visit(ISmallLangNode? Parent, IfNode self)
    {
        throw new NotImplementedException();
    }

    public Nothing Visit(ISmallLangNode? Parent, SwitchNode self)
    {
        throw new NotImplementedException();
    }

    public Nothing Visit(ISmallLangNode? Parent, ExprSectionCombinedNode self)
    {
        throw new NotImplementedException();
    }

    public Nothing Visit(ISmallLangNode? Parent, TypeAndIdentifierCSVElementNode self)
    {
        throw new NotImplementedException();
    }

    public Nothing Visit(ISmallLangNode? Parent, AliasExprNode self)
    {
        throw new NotImplementedException();
    }

    public Nothing Visit(ISmallLangNode? Parent, DeclarationNode self)
    {
        throw new NotImplementedException();
    }

    public Nothing Visit(ISmallLangNode? Parent, DeclarationModifiersCombinedNode self)
    {
        throw new NotImplementedException();
    }

    public Nothing Visit(ISmallLangNode? Parent, DeclarationModifierNode self)
    {
        throw new NotImplementedException();
    }

    public Nothing Visit(ISmallLangNode? Parent, FunctionArgDeclModifiersNode self)
    {
        throw new NotImplementedException();
    }

    public Nothing Visit(ISmallLangNode? Parent, FunctionArgDeclModifiersCombinedNode self)
    {
        throw new NotImplementedException();
    }

    public Nothing Visit(ISmallLangNode? Parent, AssignmentPrimeNode self)
    {
        throw new NotImplementedException();
    }

    public Nothing Visit(ISmallLangNode? Parent, FactorialExpressionNode self)
    {
        throw new NotImplementedException();
    }

    public Nothing Visit(ISmallLangNode? Parent, BinaryExpressionNode self)
    {
        throw new NotImplementedException();
    }

    public Nothing Visit(ISmallLangNode? Parent, ComparisonExpressionNode self)
    {
        throw new NotImplementedException();
    }

    public Nothing Visit(ISmallLangNode? Parent, OperatorExpressionPairNode self)
    {
        throw new NotImplementedException();
    }

    public Nothing Visit(ISmallLangNode? Parent, PrimaryNode self)
    {
        Func<IToken, IRunTimeObject> Factory = self.Data.TT switch
        {
            TokenType.Number => Number.FromToken,
            TokenType.String => String.FromToken,
            TokenType.TrueLiteral or TokenType.FalseLiteral => Boolean.FromToken,
            _ => throw new MatchNotFoundException()
        };
        return Push(Factory(self.Data));
    }

    public Nothing Visit(ISmallLangNode? Parent, CopyExprNode self)
    {
        throw new NotImplementedException();
    }

    public Nothing Visit(ISmallLangNode? Parent, NewExprNode self)
    {
        throw new NotImplementedException();
    }

    public Nothing Visit(ISmallLangNode? Parent, IndexNode self)
    {
        throw new NotImplementedException();
    }

    public Nothing Visit(ISmallLangNode? Parent, FunctionCallNode self)
    {
        throw new NotImplementedException();
    }

    public Nothing Visit(ISmallLangNode? Parent, ArgListElementNode self)
    {
        throw new NotImplementedException();
    }

    public Nothing Visit(ISmallLangNode? Parent, TypeCSVNode self)
    {
        throw new NotImplementedException();
    }

    public Nothing Visit(ISmallLangNode? Parent, UnaryExpressionNode self)
    {
        throw new NotImplementedException();
    }

    public Nothing Visit(ISmallLangNode? Parent, ElseNode self)
    {
        throw new NotImplementedException();
    }

    public Nothing Visit(ISmallLangNode? Parent, FactorialSymbolNode self)
    {
        throw new NotImplementedException();
    }
}