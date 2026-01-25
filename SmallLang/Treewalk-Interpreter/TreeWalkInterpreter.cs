using System.Diagnostics;
using Common;
using Common.Dispatchers;
using Common.LinqExtensions;
using Common.Tokens;
using sly.lexer;
using SmallLang.Exceptions;
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
    public Nothing Interpret(ISmallLangNode? self)
    {
        if (self is null) return Nothing.GetNothing;
        return self.AcceptVisitor(null, this);
    }
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
        self.Statements.Select(x => Interpret(x)).Evaluate();
        return Nothing.GetNothing;
    }

    public Nothing Visit(ISmallLangNode? Parent, FunctionNode self)
    {
        return Nothing.GetNothing; //we should be able to call a function via flatten and searching, I think
    }

    public Nothing Visit(ISmallLangNode? Parent, LoopCTRLNode self)
    {
        throw new NotImplementedException();
    }

    public Nothing Visit(ISmallLangNode? Parent, ForNode self)
    {
        throw new NotImplementedException();
    }


    bool InterpretConditionalExpression(IExpressionNode Cond)
    {

        //TODO: Fix Bodge
        if (Cond.GenericSLType?.OutmostType != TypeData.Bool)
        {
            throw new TypeErrorException(new GenericSmallLangType(TypeData.Bool), Cond.GenericSLType!, -1);
        }

        var top = State.Stack.Peek();

        Interpret(Cond);

        Debug.Assert(State.Stack.Peek() is Boolean boolean && ReferenceEquals(boolean, top));
        return (bool)State.Stack.Pop().AsVal<bool>()!;
    }

    public Nothing Visit(ISmallLangNode? Parent, WhileNode self)
    {


        bool BreakFlag = false;

        while (InterpretConditionalExpression(self.ConditionExpression))
        {
            try
            {
                Interpret(self.LoopBody);
            }
            catch (ControlException e)
            {
                if (e.TryGetDst(out var variableName) && variableName != self.LoopLabel?.Identifier.VariableName)
                {
                    throw;
                }
                else if (e.ContinueFlag)
                {
                    continue;
                }
                else if (e.BreakFlag)
                {
                    BreakFlag = true;
                    break;
                }
                else
                {
                    throw new Exception("Impossible Case");
                }
            }
        }
        if (!BreakFlag)
        {
            Interpret(self.Else);
        }

        return Nothing.GetNothing;
    }

    public Nothing Visit(ISmallLangNode? Parent, LoopLabelNode self)
    {
        return Nothing.GetNothing;
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