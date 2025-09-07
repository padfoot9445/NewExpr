using System.Diagnostics;
using Common.Dispatchers;
using Common.LinearIR;
using Common.Tokens;
using SmallLang.IR.AST;
using SmallLang.IR.AST.Generated;
using SmallLang.IR.LinearIR;
using SmallLang.IR.Metadata;

namespace SmallLang.CodeGen.Frontend.CodeGeneratorFunctions;

using ArgumentType = NumberWrapper<int, BackingNumberType>;
using TypeType = NumberWrapper<byte, BackingNumberType>;

internal static class BinaryExpressionVisitor
{
    private static void AssignmentVisitor(BinaryExpressionNode Self, CodeGenerator Driver)
    {
        Driver.Cast(Self.Right, Self.Left.TypeOfExpression!);

        var VariableBeginning = Driver.GetRegisters().Single();

        Driver.Emit(HighLevelOperation.LoadFromStack(VariableBeginning, Self.Left.TypeOfExpression!.Size));

        Driver.Emit(
            HighLevelOperation.PushFromRegister(VariableBeginning,
                Self.Left.TypeOfExpression
                    .Size)); //push the value back on the register because this is an expression still and expressions put values on stack

        if (Self.Left is IndexNode IndexLeft)
            IndexAssignmentVisitor(IndexLeft, Driver, VariableBeginning, Self.Left.GenericSLType!);
        else if (Self.Left is IdentifierNode IDLeft)
            IdentifierAssignmentVisitor(IDLeft, Driver, VariableBeginning, Self.Left.GenericSLType!);
        else throw new Exception();
    }

    private static void IdentifierAssignmentVisitor<TLeft>(TLeft Left, CodeGenerator Driver, int VariableBeginning,
        GenericSmallLangType TypeOfVariable)
        where TLeft : ISmallLangNode, IHasAttributeTypeOfExpression, IHasAttributeVariableName
    {
        var startReg = Driver.Data.GetVariableStartRegister(Left.VariableName!);
        var width = TypeOfVariable.Size;

        Driver.Emit(HighLevelOperation.MoveRegister(VariableBeginning, startReg, width));
    }

    private static void IndexAssignmentVisitor<TLeft>(TLeft Left, CodeGenerator Driver, int VariableBeginning,
        GenericSmallLangType RightType)
        where TLeft : IndexNode
    {
        var Pointer = Driver.GetRegisters((int)Left.Expression1.TypeOfExpression!.Size).First();


        Driver.Exec(Left.Expression1);
        Driver.Emit(HighLevelOperation.LoadFromStack(Pointer, Left.Expression1.TypeOfExpression!.Size));
        if (Left.Expression1.TypeOfExpression == TypeData.Array ||
            Left.Expression1.TypeOfExpression == TypeData.List) //TODO: Generalize this to SmallLangType.IsVectorLike
        {
            var Indexer = Driver.GetRegisters((int)TypeData.Int.Size).First();
            var ItemPtr = Driver.GetRegisters((int)Left.TypeOfExpression!.Size).First();

            Debug.Assert(Left.GenericSLType == RightType, $"{Left.TypeOfExpression}, {RightType}");

            //Push the indexer value as an int onto the stack
            Driver.Cast(Left.Expression2, TypeData.Int);

            //indexer -> r@Indexer
            Driver.Emit(HighLevelOperation.LoadFromStack(Indexer, (int)TypeData.Int.Size));

            //Get the pointer to the memory location storing the relevant value
            //Ptr -> r@ItemPtr
            Driver.Emit(HighLevelOperation.IndexVectorLike(Pointer, Indexer, ItemPtr));

            Driver.Emit(HighLevelOperation.StoreToMemory(VariableBeginning, ItemPtr, RightType.Size));
        }
        else if (Left.Expression1.TypeOfExpression == TypeData.Dict)
        {
            var KeyType =
                Left.Expression1.GenericSLType!.ChildNodes.First()
                    .OutmostType; //the expected Type of Expression of a in x[a]. This is correct, as validated in analyser.

            var Indexer = Driver.GetRegisters((int)KeyType!.Size).First();

            Driver.Cast(Left.Expression2, KeyType);
            Driver.Emit(HighLevelOperation.LoadFromStack(Indexer, KeyType.Size));

            Driver.Emit(HighLevelOperation.LoadHashMap<int, int, int, byte, byte>(Indexer, Pointer, VariableBeginning,
                KeyType, RightType));
        }
    }

    private static void NonAssignmentOperatorVisitor(BinaryExpressionNode Self, CodeGenerator Driver)
    {
        Debug.Assert(Self.Left.GenericSLType is not null && Self.Right.GenericSLType is not null);
        var GreatestCommonType = Self.Left.GenericSLType.GreatestCommonType(Self.Right.GenericSLType);

        var LeftRegister = Driver.GetRegisters(GreatestCommonType).Single();
        var RightRegister = Driver.GetRegisters(GreatestCommonType).Single();
        var DstRegister = Driver.GetRegisters(GreatestCommonType).Single();

        Driver.Cast(Self.Left, GreatestCommonType);
        Driver.Emit(HighLevelOperation.LoadFromStack(LeftRegister, GreatestCommonType.Size));

        Driver.Cast(Self.Right, GreatestCommonType);
        Driver.Emit(HighLevelOperation.LoadFromStack(RightRegister, GreatestCommonType.Size));

        Driver.Emit(Self.Data.TT
            .Map<TokenType, Func<ArgumentType, ArgumentType, ArgumentType, TypeType, HighLevelOperation>>(
                (TokenType.LogicalImplies, HighLevelOperation.LogicalImplies),
                (TokenType.LogicalOr, HighLevelOperation.LogicalOr),
                (TokenType.LogicalXor, HighLevelOperation.LogicalXor),
                (TokenType.LogicalAnd, HighLevelOperation.LogicalAnd),
                (TokenType.Addition, HighLevelOperation.Addition),
                (TokenType.Subtraction, HighLevelOperation.Subtraction),
                (TokenType.Multiplication, HighLevelOperation.Multiplication),
                (TokenType.Division, HighLevelOperation.Division),
                (TokenType.Exponentiation, HighLevelOperation.Exponentiation),
                (TokenType.BitwiseOr, HighLevelOperation.BitwiseOr),
                (TokenType.BitwiseXor, HighLevelOperation.BitwiseXor),
                (TokenType.BitwiseAnd, HighLevelOperation.BitwiseAnd),
                (TokenType.BitwiseLeftShift, HighLevelOperation.BitwiseLeftShift),
                (TokenType.BitwiseRightShift, HighLevelOperation.BitwiseRightShift)
            )(LeftRegister, RightRegister, DstRegister, GreatestCommonType));

        Driver.Emit(HighLevelOperation.PushFromRegister(DstRegister, GreatestCommonType.Size));
    }

    internal static void Visit(BinaryExpressionNode Self, CodeGenerator Driver)
    {
        Driver.EnteringChunk(() =>
        {
            Self.Switch<BinaryExpressionNode, TokenType, Action<BinaryExpressionNode, CodeGenerator>>
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
            )(Self, Driver);
            Driver.Next();
        });
    }
}