using Common.Dispatchers;
using Common.Tokens;
using SmallLang.IR.AST;
using SmallLang.IR.AST.Generated;
namespace SmallLang.CodeGen.Frontend.CodeGeneratorFunctions;

internal static class BinaryExpressionVisitor
{
    private static void AssignmentVisitor(BinaryExpressionNode Self, CodeGenerator Driver)
    {
        var GCType = Self.GreatestCommonType!;

        Driver.Cast(Self.Right);

        var VariableBeginning = Driver.GetRegisters(1).Single();

        Driver.Emit(HighLevelOperation.LoadFromStack(VariableBeginning, GCType.Size));

        if (Self.Left is IndexNode IndexLeft)
        {
            IndexAssignmentVisitor(IndexLeft, Driver, VariableBeginning, GCType);
        }
        else if (Self.Left is IdentifierNode IDLeft)
        {
            IdentifierAssignmentVisitor(IDLeft, Driver, VariableBeginning, GCType);
        }
        else throw new Exception();
    }
    internal static void Visit(BinaryExpressionNode Self, CodeGenerator Driver)
    {
        Driver.EnteringChunk(() =>
        {
            Self.Switch
            (
                x => x.Data.TT,
                (x, y) => x == y,


                (TokenType.Equals, AssignmentVisitor),
                (TokenType.LogicalImplies, NonAssignmentOperatorVisitor),
                (TokenType.LogicalOr, NonAssignmentOperatorVisitor),
                (TokenType.LogicalXor, NonAssignmentOperatorVisitor),
                (TokenType.LogicalAnd, NonAssignmentOperatorVisitor),
                (TokenType.Addition, NonAssignmentOperatorVisitor),
                (TokenType.Subtraction, NonAssignmentOperatorVisitor),
                (TokenType.Multiplication, NonAssignmentOperatorVisitor),
                (TokenType.Division, NonAssignmentOperatorVisitor),
                (TokenType.Exponentiation, NonAssignmentOperatorVisitor),
                (TokenType.BitwiseOr, NonAssignmentOperatorVisitor),
                (TokenType.BitwiseXor, NonAssignmentOperatorVisitor),
                (TokenType.BitwiseAnd, NonAssignmentOperatorVisitor),
                (TokenType.BitwiseLeftShift, NonAssignmentOperatorVisitor),
                (TokenType.BitwiseRightShift, NonAssignmentOperatorVisitor),
                (TokenType.BitwiseNegation, NonAssignmentOperatorVisitor)
            )(Self.Data, Self, Driver);
        });
    }
}