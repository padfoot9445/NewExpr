using System.Diagnostics;
using Common.AST;
using Common.Tokens;

namespace MEXP.Parser.Internals;
abstract class BinaryPrimeParserBase : InternalParserBase
{
    protected BinaryPrimeParserBase(ParserData p) : base(p)
    {
    }
    public override bool Parse(out AnnotatedNode<Annotations>? Node)
    {
        return BinaryPrime(out Node);
    }

    private protected abstract InternalParserBase NextInPriority { get; }
    private protected abstract ICollection<TokenType> Operators { get; }
    private protected virtual string ErrorMessage => $"PrimedBinary Error at {Position}";
    private protected virtual string TypeMismatchErrorMessage(uint Type1, uint Type2) => $"Type Mismatch between {Type1} and {Type2} at {Position}";
    private protected virtual AnnotatedNode<Annotations> Action(ASTNode ASNode)
    {
        Debug.Assert(ASNode.Children.Length == 3 || ASNode.Children.Length == 0);
        if (ASNode.Children.Length == 0)
        {
            return new(new(IsEmpty: true), ASNode);
        }
        Annotations NextInPriorityAnnotations = GetFromChildIndex(ASNode, 1);
        Annotations BinaryPrimeAnnotations = GetFromChildIndex(ASNode, 2);
        Debug.Assert(NextInPriorityAnnotations.TypeCode is not null);
        if (BinaryPrimeAnnotations.IsEmpty)
        {
            return new(new(TypeCode: NextInPriorityAnnotations.TypeCode), ASNode);
        }
        else
        {
            Debug.Assert(BinaryPrimeAnnotations.TypeCode is not null);
            return new(
                new(
                    TypeCode: TP.BinOpResultantType((uint)NextInPriorityAnnotations.TypeCode!, (uint)BinaryPrimeAnnotations.TypeCode!) ?? throw new InvalidOperationException(TypeMismatchErrorMessage((uint)NextInPriorityAnnotations.TypeCode, (uint)BinaryPrimeAnnotations.TypeCode))
                ),
                ASNode
            );
        }
    }

    bool BinaryPrime(out AnnotatedNode<Annotations>? Node)
    {
        if (Operators.Contains(CurrentToken()!.TT))
        {
            IToken Operator = CurrentToken(Inc: true)!;
            if (SafeParse(NextInPriority, out AnnotatedNode<Annotations>? ParentPrimedNode, Suppress: false) && SafeParse(this, out AnnotatedNode<Annotations>? PrimeNode, Suppress: false))
            {
                Node = Action(ASTNode.BinaryPrime(Operator: Operator, Right: ParentPrimedNode!, Repeat: PrimeNode!, Name));
                return true;
            }
            else
            {
                Log.Log(ErrorMessage);
                Node = null;
                return false;
                //similarly, faliure to parse ParentPrimedNode or NextInPriority is handled by those methods
            }
        }

        //if not **  must be empty
        Node = Action(ASTNode.Empty(Name)); //isempty is true
        return true;
    }
}