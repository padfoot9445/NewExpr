namespace Common.Tokens;
public enum TokenType
{
    //should be shared between all sets of valid tokens, since invalid ones will simply be unused
    Number,
    Identifier,

    //Operators
    #region bitwise operator
    BitwiseAnd,
    BitwiseOr,
    BitwiseXor,
    BitwiseLeftShift,
    BitwiseRightShift,
    BitwiseNegation,
    #endregion
    #region arithmetic operator
    Addition,
    Subtraction,
    Multiplication,
    Division,
    #endregion
    #region logical operator
    LogicalAnd,
    LogicalOr,
    LogicalNot,
    LogicalXor,
    LogicalImplies,
    #endregion
    OpenParen,
    CloseParen,
    Comma,
    #region Comparison_Operator
    EqualTo,
    NotEqualTo,
    GreaterThan,
    LessThan,
    GreaterThanOrEqualTo,
    LessThanOrEqualTo,
    ComparisonIs,
    In,
    Semicolon,
    #endregion
    Equals,
    Float,
    Int,
    EOF,

}