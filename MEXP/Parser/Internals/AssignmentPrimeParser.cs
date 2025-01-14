using System.Diagnostics;
using Common.AST;
using Common.Tokens;

namespace MEXP.Parser.Internals;
class AssignmentPrimeParser : BinaryPrimeParserBase
{
    public AssignmentPrimeParser(ParserData p) : base(p)
    {
    }
    private protected override AnnotatedNode<Annotations> Action(ASTNode ASNode)
    {
        if (ASNode.Children.Length == 0) //if assignmentprime is empty
        {
            return new(new(IsEmpty: true), ASNode);
        }
        else
        {
            Debug.Assert(ASNode.Children.Length == 3);
            Debug.Assert(ASNode.Children[2] is AnnotatedNode<Annotations>);
            Annotations NestedAssignmentPrimeAnnotations = ParserData.GetFromChildIndex(ASNode, 2);
            Annotations AdditionAnnotations = ParserData.GetFromChildIndex(ASNode, 1);
            if (NestedAssignmentPrimeAnnotations.IsEmpty is true)
            {
                return new(
                    new(IsEmpty: false,
                    TypeCode: AdditionAnnotations.TypeCode
                    ), //= x, TypeCode <- x.TypeCode
                    ASNode
                );
            }
            else
            {
                Debug.Assert(NestedAssignmentPrimeAnnotations.IsEmpty is false);
                if (!AdditionAnnotations.CanBeResolvedToAssignable)
                {
                    throw new InvalidOperationException($"Cannot assign to non-variable at position {Position}");
                    //TODO: Add handling for exceptions in actions in binaryparse methods
                }
                return new(
                    new(TypeCode: NestedAssignmentPrimeAnnotations.TypeCode), // = x (= y), TypeCode <= (= y).TypeCode
                    ASNode
                );
            }
        }
    }

    private protected override InternalParserBase NextInPriority => Addition;

    private protected override ICollection<TokenType> Operators => [TokenType.Equals];

    private protected override string Name => "AssignmentPrime";
}