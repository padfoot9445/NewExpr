namespace Common.Tokens;
public enum TokenType
{
    //should be shared between all sets of valid tokens, since invalid ones will simply be unused
    Number,
    Identifier,
    String,

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
    Exponentiation,
    Factorial,
    #endregion
    #region logical operator
    LogicalAnd,
    LogicalOr,
    LogicalNot,
    LogicalXor,
    LogicalImplies,
    #endregion
    #region Brackets
    OpenParen,
    CloseParen,
    OpenCurly,
    CloseCurly,
    OpenSquare,
    CloseSquare,
    OpenAngleSquare,
    CloseAngleSquare,
    #endregion
    #region Symbols
    Comma,
    Colon,
    Dot,
    #endregion
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
    #region Types
    TypeFloat,
    TypeInt,
    TypeDouble,
    TypeNumber,
    TypeLong,
    TypeLongInt,
    TypeByte,
    TypeArray,
    TypeList,
    TypeSet,
    TypeDict,
    TypeShort,
    TypeRational,
    TypeString,
    TypeChar,
    TypeVoid,
    #endregion

    #region control-flow
    Return,
    Break,
    Continue,
    If,
    Else,
    Switch,
    #endregion
    #region Loops
    For,
    While,
    #endregion
    As,
    #region FunctionMod
    Cascading,
    Copy,
    Readonly,
    Frozen,
    Immut,
    Ref,
    New,
    #endregion
    TrueLiteral,
    FalseLiteral,
    EOF,

}