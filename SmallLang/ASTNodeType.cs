namespace SmallLang;
public enum ASTNodeType
{
    Internal, //not exposed and should not be present in the final tree
    Section, //data: null, children: flattened array of statements
    Statement, //data: null, children: array[0] of statement(unused, passthrough, but should probably still handle)
    Block, //data: null, unused, passthrough
    Cond, //data: null, unused, passthrough to if or switch
    Expression, //unused, passthrough
    Function, //data: identifier, children: [type, statement] | [type, typeandidentifiercsv, statement]
    LoopCTRL, //data: break | continue, children: [] | [identifier: opnestedvalinloopcontrol]
    Loop, //unused, passthrough
    For, //data: null, children: [expression, expression, expression, statement]
    While, //data: null, children: [expression, statement]
    OpValInLCTRL, //unused, flatten up
    Return, //NO FLATTEN, data: null, children: [Expression]
    BaseType, //data: base type, children: []
    GenericType, //data: generic type, children: []
    Type, //passthrough to basetype if applicable, else data: generictype, children: [TypeCSV]
    If, //data: null, children: [ExprStatementCombined+, OptionalElse] where OptionalElse is not EMPTY| [ExprStatementCombined+]
    Switch, //data: null, children: [Expression, ExprStatementCombined*]
    ExprStatementCombined, //data: null, children: [Expression, Statement]
    TypeAndIdentifierCSV, //data: null, children: [TypeAndIdentifierCSVElement+]
    TypeAndIdentifierCSVElement, //data: null, children: [FunctionArgDeclModifiersCombined, Type, Identifier]
    Identifier, //data: IDENTIFIER, children: []
    AliasExpr, //data: IDENTIFIER, children: [Identifier]
    ReTypingAlias, //data: IDENTIFIER, children: [Type Identifier]
    ReTypeOriginal, //data: IDENTIFIER, children: [Type]
    Declaration, //data: "=" or null, children: [DeclarationModifiersCombined, Type, Identifier, Expression] | [Type, Identifier, Expression]
    DeclarationModifiersCombined, //data: null, children: [DeclarationModifier+]
    DeclarationModifier, //data: "ref" | "readonly" | "frozen" | "immut", children: []
    FunctionArgDeclModifiers, //data: *DeclarationModifier | "copy", children: []
    FunctionArgDeclModifiersCombined, //data: null, children: [FunctionArgDeclModifiers+]
    AssignmentPrime, //unused, data: "=", children: [Expression]
    AssignmentExpr, //data: "=", children: [Identifier, Expression]
    BinaryExpression, //data: "implies" | "or" | "xor" | "and" | "==" | "!=" | ">" | ">=" | "<" | "<=" | "+" | "-" | "*" | "/" | "**" | "|" | "^" | "&", children: [Expression, Expression] // Deconstruct syntactic sugar of x < y > z and x == y == z etc into binary and and ops in parser
    UnaryExpression, //data: "not" | "-" | "~" | "!", children: [Expression] //x++ x-- etc if implemented can also be deconstructed in the parser
    Primary, //data: IDENTIFIER | NUMBER | STRING | BOOL, children: [] //paren expr is passthrough expr
    CopyExpr, //data: null, children: [Expression] //NO UP FLATTEN
    NewExpr, //data: null, children: [Type, ArgList] | [Type]
    Index, //data: null, children: [Expression, Expression]
    FunctionCall, //data: null | ".", children: [Expression, ArgList] | [Expression]
    ArgList, //data: null, children: [ArgListElement+]
    ArgListElement, //data: null, children: [Expression] | [ArgListLabel, Expression]
    ArgListLabel, //data: IDENTIFIER
    TypeCSV, //data: null, children: [Type+]
}