using System.Diagnostics;
using Common.AST;
using Common.Tokens;
using MEXP.IRs.ParseTree;
namespace MEXP.Parser.Internals;
class PrimaryParser : InternalParserBase
{
    public PrimaryParser(ParserData p) : base(p)
    {
    }

    private protected override string Name => "Primary";

    private bool ParseBracket(out AnnotatedNode<Annotations>? Node)
    {
        IToken OpenBracketToken = CurrentToken(Inc: true)!;
        //get expr
        if (!SafeParse(Data.Expression, out AnnotatedNode<Annotations>? Expr, Suppress: false))
        {
            //no message as not suppressed
            Node = null;
            return false;
        }
        if (CurrentToken().TCmp(TokenType.CloseParen))
        {
            Annotations annotations = new Annotations(CanBeResolvedToAssignable: false);
            annotations.ForcedMerge(Expr!.Attributes); //we propagate empty since empty brackets are still empty, but the addition of brackets means that we can no-longer resolve to an identifier literal
            Node = new(
                Attributes: annotations,
                node: ASTNode.Parenthesized(Open: OpenBracketToken, Center: Expr!, Close: CurrentToken()!, Name)
            );
            Advance();
            return true;
        }
        else
        {
            Log.Log($"Expected Close Parenthesis at position {Position}");
            Node = null;
            return false;
        }
    }
    private bool GetAndValidateIdentifierType(IToken IdentifierToken, out uint IdentifierType)
    {
        uint? _IdentifierType = TP.GetTypeFromIdentifierLiteral(IdentifierToken.Lexeme);
        if (_IdentifierType is null)
        {
            Log.Log($"Identifier {IdentifierToken.Lexeme} was used before declaration at {Position}");
            IdentifierType = uint.MaxValue;
            return false;
        }
        IdentifierType = (uint)_IdentifierType!;
        return true;
    }
    private bool ParseRemainderOfAssignment(AnnotatedNode<Annotations>? AssP, uint IdentifierType, out AnnotatedNode<Annotations>? Node)
    {
        Debug.Assert(AssP!.Attributes.TypeCode is not null);
        //typecheck
        if (TP.CanBeAssignedTo(IdentifierType, (uint)AssP!.Attributes.TypeCode))
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
    private bool ParseIdentifierOrAssignment(out AnnotatedNode<Annotations>? Node)
    {
        IToken IdentifierToken = CurrentToken(Inc: true)!;
        //get the assignment prime node
        if (!SafeParse(Data.AssignmentPrime, out AnnotatedNode<Annotations>? AssP))
        {
            Log.Log($"Impossible path in ParseIdentifierAndAssignment");
            Node = null;
            return false;
        }

        //check if the identifier is properly declared
        if (!GetAndValidateIdentifierType(IdentifierToken, out uint IdentifierType))
        {
            Node = null;
            return false;
        }

        //check if there is an assignment
        if (AssP!.Attributes.IsEmpty is false)
        {
            return ParseRemainderOfAssignment(AssP, IdentifierType, out Node);

        }
        else
        {
            Node = new(
                Attributes: new(
                    TypeCode: IdentifierType,
                    CanBeResolvedToAssignable: true,
                    IsEmpty: false
                ),
                node: AssP
            );
            return true;
        }
    }
    private bool ParseNumber(out AnnotatedNode<Annotations>? Node)
    {
        IToken NumberToken = CurrentToken(Inc: true)!;
        ASTNode NumberNode = ASTNode.Terminal(NumberToken, Name);
        Node = new(
            new(TypeCode: TP.GetTypeFromNumberLiteral(NumberToken.Lexeme)),
            NumberNode
        );
        return true;
    }
    public override bool Parse(out AnnotatedNode<Annotations>? Node)
    {
        if (CurrentToken().TCmp(TokenType.OpenParen))
        //brackets
        {
            return ParseBracket(out Node);
        }
        else if (CurrentToken().TCmp(TokenType.Identifier))
        {
            return ParseIdentifierOrAssignment(out Node);
        }
        else if (CurrentToken().TCmp(TokenType.Number))
        {
            return ParseNumber(out Node);
        }
        Log.Log($"Expected Open Parenthesis ( or Number or identifier at token position {Position}, but got \"{CurrentToken()!.Lexeme}\"");
        Node = null;
        return false;
    }
}