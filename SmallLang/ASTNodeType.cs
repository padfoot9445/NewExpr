namespace SmallLang;
public enum ASTNodeType
{
    Terminal,
    Any,
    Internal, //not exposed and should not be present in the final tree
    Section, //data: null, children: flattened array of statements
    Statement, //data: null, children: array[0] of statement(unused, passthrough, but should probably still handle)
    Block, //data: null, unused, passthrough
    Cond, //data: null, unused, passthrough to if or switch
    Expression, //unused, passthrough
    Function, //data: identifier, children: [type, statement] | [type, typeandidentifiercsv, statement]
    LoopCTRL, //data: break | continue, children: [] | [identifier: opnestedvalinloopcontrol]
    Loop, //unused, passthrough
    For, //data: null, children: [expression, expression, expression, Label, statement, else as Statement]
    While, //data: null, children: [expression, statement, Label, else as Statement]
    ValInLCTRL, //data: identifier
    LoopLabel, //data: identifier
    Return, //NO FLATTEN, data: null, children: [Expression]
    BaseType, //data: base type, children: []
    GenericType, //data: generic type, children: []
    Type, //unused, passthrough
    If, //data: null, children: [ExprStatementCombined+, OptionalElse as Statement] where OptionalElse is not EMPTY| [ExprStatementCombined+]
    Switch, //data: null, children: [Expression, ExprStatementCombined*]
    ExprStatementCombined, //data: null, children: [Expression, Statement]
    TypeAndIdentifierCSV, //data: null, children: [TypeAndIdentifierCSVElement+]
    TypeAndIdentifierCSVElement, //data: Identifier, children: [FunctionArgDeclModifiersCombined, Type]
    Identifier, //data: IDENTIFIER, children: []
    AliasExpr, //data: IDENTIFIER, children: [Identifier]
    ReTypingAlias, //data: IDENTIFIER, children: [Type Identifier]
    ReTypeOriginal, //data: IDENTIFIER, children: [Type]
    Declaration, //data: Identifier, children: [DeclarationModifiersCombined, Type, AssignmentPrime] | [Type, AssignmentPrime] | [Type]
    DeclarationModifiersCombined, //data: null, children: [DeclarationModifier*]
    DeclarationModifier, //data: "ref" | "readonly" | "frozen" | "immut", children: []
    FunctionArgDeclModifiers, //data: *DeclarationModifier | "copy", children: []
    FunctionArgDeclModifiersCombined, //data: null, children: [FunctionArgDeclModifiers+]
    AssignmentPrime, //data: "=", children: [Expression]
    AssignmentExpr, //data: "=", children: [Identifier, Expression]
    PrefixExpression, //data: operator, children: [Expression]
    FactorialExpression, //data: null, children: [Expression, !+]
    BinaryExpression, //data: "implies" | "or" | "xor" | "and" | "==" | "!=" | ">" | ">=" | "<" | "<=" | "+" | "-" | "*" | "/" | "**" | "|" | "^" | "&", children: [Expression, Expression] // Deconstruct syntactic sugar of x < y > z and x == y == z etc into binary and and ops in parser
    ComparisionExpression, //data: null, children: [Expression, OperatorExpressionPair+] // x < y > z > a -> [x, (< y), (> z), (> a)] via 
    OperatorExpressionPair, //data: cmpOperator, children: [Expression]
    UnaryExpression, //data: "not" | "-" | "~" | "!", children: [Expression] //x++ x-- etc if implemented can also be deconstructed in the parser
    Primary, //data: IDENTIFIER | NUMBER | STRING | BOOL, children: [] //paren expr is passthrough expr
    CopyExpr, //data: null, children: [Expression] //NO UP FLATTEN
    NewExpr, //data: null, children: [Type, ArgList] | [Type]
    Index, //data: null, children: [Expression, Expression] where Expression1 is Primary or subset (parens etc)
    FunctionCall, //data: null | ".", children: [Expression, ArgList] | [Expression] where Expression1 i
    ArgList, //data: null, children: [ArgListElement+]
    ArgListElement, //data: null, children: [Expression] | [ArgListLabel, Expression]
    ArgListLabel, //data: IDENTIFIER
    TypeCSV, //data: null, children: [Type+]
}