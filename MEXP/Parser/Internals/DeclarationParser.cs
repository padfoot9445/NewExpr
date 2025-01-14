using System.Diagnostics;
using Common.AST;
using Common.Tokens;

namespace MEXP.Parser.Internals;
class DeclarationParser : InternalParserBase
{
    public DeclarationParser(ParserData p) : base(p)
    {
    }
    private protected override string Name => "Declaration";
    private bool DeclIsTypeSafe(AnnotatedNode<Annotations> TNode, AnnotatedNode<Annotations> ANode)
    {
        return ANode!.Attributes.IsEmpty is true || TP.CanBeDeclaredTo(TNode!.Attributes.TypeDenotedByIdentifier!, ANode!.Attributes.TypeCode!);
    }
    private bool GetNodeWithAssignmentPrime(AnnotatedNode<Annotations> TNode, AnnotatedNode<Annotations> ANode, out AnnotatedNode<Annotations> Node)
    {
        Node = new(new(
            TypeCode: ANode!.Attributes.IsEmpty is true ? null : TNode!.Attributes.TypeDenotedByIdentifier, //if AssignmentPrime is empty then we cannot give any type to this declaration as an expression
            IsEmpty: false
        ), ASTNode.Binary(TNode!, CurrentToken(-1)!, ANode!, Name));
        return true;
    }
    private bool ParseAssignmentPrime(AnnotatedNode<Annotations> TNode, IToken IdentToken, out AnnotatedNode<Annotations>? Node)
    {
        Debug.Assert(IdentToken.TT == TokenType.Identifier);
        Debug.Assert(TNode!.Attributes.TypeDenotedByIdentifier is not null);
        if (!SafeParse(AssignmentPrime, out AnnotatedNode<Annotations>? ANode))
        {

            Log.Log($"Impossible path in {Name}");
            Node = null;
            return false;
        }

        //verify type safety if AssignmentPrime exists
        if (!DeclIsTypeSafe(TNode, ANode!))
        {
            //if there exists an assignmentprime and the declaration is not type-safe then we have an issue; if there does not exist an assignmentprime the declaration DNE so we don't care about types
            Log.Log($"Type mismatch at position {Position}; Cannot assign {ANode!.Attributes.TypeCode} to {TNode!.Attributes.TypeDenotedByIdentifier}"); //TODO: Reverse typecodes for better error reporting
            Node = null;
            return false;
        }
        //if type safe
        TP.StoreIdentifierType(IdentToken.Lexeme, (uint)TNode!.Attributes.TypeDenotedByIdentifier); //store identifier type in type table upon declaration
        return GetNodeWithAssignmentPrime(TNode, ANode!, out Node);


    }
    private bool ParseDecl(AnnotatedNode<Annotations> TNode, out AnnotatedNode<Annotations>? Node)
    {
        IToken IdentToken = CurrentToken(Inc: true)!;
        if (!IdentToken.TCmp(TokenType.Identifier))
        {
            Log.Log($"Expected Identifier after Type at position {Position}");
            Node = null;
            return false;
        }
        else
        {
            return ParseAssignmentPrime(TNode, IdentToken, out Node);
        }
    }


    public override bool Parse(out AnnotatedNode<Annotations>? Node)
    {
        if (SafeParse(Type, out AnnotatedNode<Annotations>? TNode))
        {
            return ParseDecl(TNode!, out Node);
        }
        else if (SafeParse(Addition, out AnnotatedNode<Annotations>? Add, Suppress: false))
        {
            Node = new(Add!.Attributes.Copy(), ASTNode.NonTerminal(Add!, Name));
            return true;
        }
        Node = null;
        Log.Log($"Expected addition or declaration (Type) at position {Position}");
        return false;
    }
}