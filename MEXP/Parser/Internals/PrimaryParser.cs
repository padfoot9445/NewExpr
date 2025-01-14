using System.Diagnostics;
using Common.AST;
using Common.Tokens;
namespace MEXP.Parser.Internals;
class PrimaryParser : InternalParserBase
{
    public PrimaryParser(Parser p) : base(p)
    {
    }

    private protected override string Name => "Primary";

    public override bool Parse(out AnnotatedNode<Annotations>? Node)

    {
        if (CurrentToken()!.TT == TokenType.OpenParen)
        {
            IToken Operator = CurrentToken(Inc: true)!;
            if (_Parser.SP.SafeParse(_Parser.Expression, out AnnotatedNode<Annotations>? Expr, Suppress: false, Current: ref _Parser.Current))
            {
                if (CurrentToken()!.TT == TokenType.CloseParen)
                {
                    Annotations annotations = new Annotations(CanBeResolvedToAssignable: false);
                    annotations.ForcedMerge(Expr!.Attributes); //we propagate empty since empty brackets are still empty, but the addition of brackets means that we can no-longer resolve to an identifier literal
                    Node = new(
                        Attributes: annotations,
                        node: ASTNode.Parenthesized(Open: Operator, Center: Expr!, Close: CurrentToken()!, Name)
                    );
                    _Parser.Advance();
                    return true;
                }
                else
                {
                    Log.Log($"Expected Close Parenthesis at position {Position}");
                    Node = null;
                    return false;
                }
            }
            else
            {
                //no message as not suppressed
                Node = null;
                return false;
            }
        }
        else if (CurrentToken().TCmp(TokenType.Identifier))
        {
            IToken IdentifierToken = CurrentToken(Inc: true)!;
            ASTNode IdentifierNode = ASTNode.Terminal(IdentifierToken, Name);
            if (_Parser.SP.SafeParse(_Parser.AssignmentPrime, out AnnotatedNode<Annotations>? AssP, Current: ref _Parser.Current))
            {
                ASTNode ASNode = ASTNode.PrimedBinary(IdentifierNode, AssP!, Name);
                uint? IdentifierType = _Parser.TP.GetTypeFromIdentifierLiteral(IdentifierToken.Lexeme);
                if (IdentifierType is null)
                {
                    Log.Log($"Identifier {IdentifierToken.Lexeme} was used before declaration at {Position}");
                    Node = null;
                    return false;
                }
                if (AssP!.Attributes.IsEmpty is true)
                {
                    Node = new(
                        Attributes: new(
                            TypeCode: IdentifierType,
                            CanBeResolvedToAssignable: true,
                            IsEmpty: false),
                        node: AssP
                    );
                    return true;
                }
                else
                {
                    Debug.Assert(AssP!.Attributes.TypeCode is not null);
                    //typecheck
                    if (_Parser.TP.CanBeAssignedTo((uint)IdentifierType, (uint)AssP!.Attributes.TypeCode))
                    {
                        Node = new(
                            Attributes: new(TypeCode: IdentifierType, IsEmpty: false, TypeDenotedByIdentifier: null), //TypeDenotedByIdentifier would need a lookup if we had custom types but we don't
                            node: AssP
                        );
                        return true;
                    }
                    else
                    {
                        Log.Log($"Could not assign {AssP!.Attributes.TypeCode} to {IdentifierType} at {Position}. If you meant to cast, declare the variable in-line; if you do not want to bind, use _ as the identifier.");
                        Node = null;
                        return false;
                    }
                }
            }
            Node = null;
            return false;
        }
        else if (CurrentToken().TCmp(TokenType.Number))
        {
            IToken NumberToken = CurrentToken(Inc: true)!;
            ASTNode NumberNode = ASTNode.Terminal(NumberToken, Name);
            Node = new(
                new(TypeCode: _Parser.TP.GetTypeFromNumberLiteral(NumberToken.Lexeme)),
                NumberNode
            );
            return true;
        }
        Log.Log($"Expected Open Parenthesis ( or Number or identifier at token position {Position}, but got \"{CurrentToken()!.Lexeme}\"");
        Node = null;
        return false;
    }



}