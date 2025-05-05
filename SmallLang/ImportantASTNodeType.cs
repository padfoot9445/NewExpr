namespace SmallLang;
public enum ImportantASTNodeType
{
    ReTypingAlias, //data: IDENTIFIER, children: [Type Identifier]
    ReTypeOriginal, //data: IDENTIFIER, children: [Type]
    Identifier, //data: IDENTIFIER, children: []
    Section, //data: null, children: flattened array of statements
    Function, //data: identifier, children: [type, statement] | [type, typeandidentifiercsv, statement]
    LoopCTRL, //data: break | continue, children: [] | [identifier: opnestedvalinloopcontrol]
    For, //data: null, children: [expression, expression, expression, Label, statement, else as Statement]
    While, //data: null, children: [expression, statement, Label, else as Statement]
    ValInLCTRL, //data: identifier
    LoopLabel, //data: identifier
    Return, //NO FLATTEN, data: null, children: [Expression]
    BaseType, //data: base type, children: []
    GenericType, //data: generic type, children: []
    If, //data: null, children: [ExprStatementCombined+, OptionalElse as Statement] where OptionalElse is not EMPTY| [ExprStatementCombined+]
    Switch, //data: null, children: [Expression, ExprStatementCombined*]
    ExprStatementCombined, //data: null, children: [Expression, Statement]
    TypeAndIdentifierCSV, //data: null, children: [TypeAndIdentifierCSVElement+]
    TypeAndIdentifierCSVElement, //data: Identifier, children: [FunctionArgDeclModifiersCombined, Type]
    AliasExpr, //data: IDENTIFIER, children: [Identifier]
    Declaration, //data: Identifier, children: [DeclarationModifiersCombined, Type, AssignmentPrime] | [Type, AssignmentPrime] | [Type]
    DeclarationModifiersCombined, //data: null, children: [DeclarationModifier*]
    DeclarationModifier, //data: "ref" | "readonly" | "frozen" | "immut", children: []
    FunctionArgDeclModifiers, //data: *DeclarationModifier | "copy", children: []
    FunctionArgDeclModifiersCombined, //data: null, children: [FunctionArgDeclModifiers+]
    AssignmentPrime, //data: "=", children: [Expression]
    //AssignmentExpr, //data: "=", children: [Identifier, Expression] 
    FactorialExpression, //data: null, children: [Expression, !+]
    BinaryExpression, //data: "implies" | "or" | "xor" | "and" | "==" | "!=" | ">" | ">=" | "<" | "<=" | "+" | "-" | "*" | "/" | "**" | "|" | "^" | "&", children: [Expression, Expression] // Deconstruct syntactic sugar of x < y > z and x == y == z etc into binary and and ops in parser
    ComparisionExpression, //data: null, children: [Expression, OperatorExpressionPair+] // x < y > z > a -> [x, (< y), (> z), (> a)] via 
    OperatorExpressionPair, //data: cmpOperator, children: [Expression]
    Primary, //data: IDENTIFIER | NUMBER | STRING | BOOL, children: [] //paren expr is passthrough expr
    FunctionIdentifier, //data: IDENTIFIER, children: []
    CopyExpr, //data: null, children: [Expression] //NO UP FLATTEN
    NewExpr, //data: null, children: [Type, ArgList] | [Type]
    Index, //data: null, children: [Expression, Expression] where Expression1 is Primary or subset (parens etc)
    FunctionCall, //data: null | ".", children: [Expression, ArgList] | [Expression] where Expression1 i
    ArgList, //data: null, children: [ArgListElement+]
    ArgListElement, //data: null, children: [Expression] | [ArgListLabel, Expression]
    TypeCSV, //data: null, children: [Type+]
    UnaryExpression,
}