using System.Diagnostics;
using Common.AST;

namespace MEXP.Parser.Internals;
abstract class PrimedBinaryParserBase : InternalParserBase
{
    protected PrimedBinaryParserBase(ParserData p) : base(p)
    {
    }
    private protected abstract InternalParserBase NextInPriority { get; }
    private protected abstract BinaryPrimeParserBase BinaryPrime { get; }
    private protected virtual string ErrorMessage => $"PrimedBinary Error at {Position}";
    private protected virtual string TypeMismatchErrorMessage(uint Type1, uint Type2) => $"Type Mismatch between {Type1} and {Type2} at {Position}";
    private protected virtual AnnotatedNode<Annotations> Action(ASTNode ASNode)
    {
        Debug.Assert(ASNode.Children.Length == 2);
        Annotations NextInPriorityAnno = GetFromChildIndex(ASNode, 0);
        Annotations ProductionPrimeAnno = GetFromChildIndex(ASNode, 1);
        Debug.Assert(NextInPriorityAnno.IsEmpty is false && NextInPriorityAnno.TypeCode is not null);
        if (!ProductionPrimeAnno.IsEmpty)
        {
            Debug.Assert(ProductionPrimeAnno.TypeCode is not null);
            //if this is actually a binary operation

            return new(
                new(
                    IsEmpty: false,
                    TypeCode: TP.BinOpResultantType((uint)NextInPriorityAnno.TypeCode!, (uint)ProductionPrimeAnno.TypeCode!) ?? throw new InvalidOperationException(TypeMismatchErrorMessage((uint)NextInPriorityAnno.TypeCode!, (uint)ProductionPrimeAnno.TypeCode!))
                ),
                ASNode
            );

        }
        else
        {
            //if not, treat it as unary and just return the annotation of the nested type
            Debug.Assert(NextInPriorityAnno.IsEmpty is false);
            return new(
                NextInPriorityAnno.Copy(),
                ASNode
            );
        }
    }
    bool PrimedBinary(out AnnotatedNode<Annotations>? Node)
    {
        if (SafeParse(NextInPriority, out AnnotatedNode<Annotations>? Neg, Suppress: false) && SafeParse(BinaryPrime, out AnnotatedNode<Annotations>? MulP, Suppress: false))
        {
            Node = Action(ASTNode.PrimedBinary(Neg!, MulP!, Name));
            return true;
        }
        Log.Log(ErrorMessage);
        Node = null;
        return false;
        //no need for error message here as the explanation of why NextInPriority or BinaryPrime failed to parse will be handled by those methods
    }
    public override bool Parse(out AnnotatedNode<Annotations>? Node)
    {
        return PrimedBinary(out Node);
    }
}