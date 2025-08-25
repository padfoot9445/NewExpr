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
            )(Self.Data, Self.Left, Self.Right, Driver);
        });
    }
}