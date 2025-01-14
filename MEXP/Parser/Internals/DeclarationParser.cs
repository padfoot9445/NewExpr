using System.Diagnostics;
using Common.AST;
using Common.Tokens;

namespace MEXP.Parser.Internals;
class DeclarationParser : InternalParserBase
{
    public DeclarationParser(Parser p) : base(p)
    {
    }
    public bool Declaration(out AnnotatedNode<Annotations>? Node)
    {
        if (_Parser.SP.SafeParse(_Parser.Type, out AnnotatedNode<Annotations>? TNode, Current: ref _Parser.Current))
        {
            IToken IdentToken = CurrentToken(Inc: true)!;
            if (!IdentToken.TCmp(TokenType.Identifier))
            {
                Log.Log($"Expected Identifier after Type at position {Position}");
                Node = null;
                return false;
            }
            if (_Parser.SP.SafeParse(_Parser.AssignmentPrime, out AnnotatedNode<Annotations>? ANode, Current: ref _Parser.Current))
            {
                Debug.Assert(IdentToken.TT == TokenType.Identifier);
                Debug.Assert(TNode!.Attributes.TypeDenotedByIdentifier is not null);
                //verify type safety if AssignmentPrime exists
                if (ANode!.Attributes.IsEmpty is false && !_Parser.TP.CanBeDeclaredTo(TNode!.Attributes.TypeDenotedByIdentifier!, ANode!.Attributes.TypeCode!))
                {
                    //if there exists an assignmentprime and the declaration is not type-safe then we have an issue; if there does not exist an assignmentprime the declaration DNE so we don't care about types
                    Log.Log($"Type mismatch at position {Position}; Cannot assign {ANode!.Attributes.TypeCode} to {TNode!.Attributes.TypeDenotedByIdentifier}"); //TODO: Reverse typecodes for better error reporting
                    Node = null;
                    return false;
                }
                else
                {
                    _Parser.TP.StoreIdentifierType(IdentToken.Lexeme, (uint)TNode!.Attributes.TypeDenotedByIdentifier); //store identifier type in type table upon declaration
                    Node = new(new(
                        TypeCode: ANode!.Attributes.IsEmpty is true ? null : TNode!.Attributes.TypeDenotedByIdentifier, //if AssignmentPrime is empty then we cannot give any type to this declaration as an expression
                        IsEmpty: false
                    ), ASTNode.Binary(TNode!, CurrentToken(-1)!, ANode!, nameof(Declaration)));
                    return true;
                }
            }
            else
            {
                Log.Log($"Impossible path in {nameof(Declaration)}");
                Node = null;
                return false;
            }
        }
        else if (_Parser.SP.SafeParse(_Parser.Addition, out AnnotatedNode<Annotations>? Add, Suppress: false, Current: ref _Parser.Current))
        {
            Node = new(Add!.Attributes.Copy(), ASTNode.NonTerminal(Add!, nameof(Declaration)));
            return true;
        }
        Node = null;
        Log.Log($"Expected addition or declaration (Type) at position {Position}");
        return false;
    }

    private protected override string Name => "Declaration";

    public override bool Parse(out AnnotatedNode<Annotations>? Node)
    {
        return Declaration(out Node);
    }
}