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

        Driver.Cast(Self.Right, Self.Left.TypeOfExpression!);

        var VariableBeginning = Driver.GetRegisters(1).Single();

        Driver.Emit(HighLevelOperation.LoadFromStack(VariableBeginning, Self.Left.TypeOfExpression!.Size));

        if (Self.Left is IndexNode IndexLeft)
        {
            IndexAssignmentVisitor(IndexLeft, Driver, VariableBeginning, Self.Left.TypeOfExpression);
        }
        else if (Self.Left is IdentifierNode IDLeft)
        {
            IdentifierAssignmentVisitor(IDLeft, Driver, VariableBeginning, Self.Left.TypeOfExpression);
        }
        else throw new Exception();
    }

    private static void IdentifierAssignmentVisitor<TLeft>(TLeft Left, CodeGenerator Driver, int VariableBeginning, SmallLangType TypeOfVariable)
    where TLeft : ISmallLangNode, IHasAttributeTypeOfExpression, IHasAttributeVariableName
    {
        var startReg = Driver.Data.GetVariableStartRegister(Left.VariableName!);
        var width = TypeOfVariable.Size;

        Driver.Emit(HighLevelOperation.MoveRegister(VariableBeginning, startReg, width));
    }
    private static void IndexAssignmentVisitor<TLeft>(TLeft Left, CodeGenerator Driver, int VariableBeginning, SmallLangType RightType)
    where TLeft : IndexNode
    {
        var Pointer = Driver.GetRegisters((int)Left.Expression1.TypeOfExpression!.Size).First();


        Driver.Exec(Left.Expression1);
        Driver.Emit(HighLevelOperation.LoadFromStack(Pointer, Left.Expression1.TypeOfExpression!.Size));
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