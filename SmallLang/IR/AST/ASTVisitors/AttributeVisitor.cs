using Common.AST;
using Common.Evaluator;
using Common.Metadata;
using Common.Tokens;
using SmallLang.Exceptions;
using SmallLang.IR.Metadata;
namespace SmallLang.IR.AST.ASTVisitors;

using Node = DynamicASTNode<ImportantASTNodeType, Attributes>;
public class AttributeVisitor : IDynamicASTVisitor<ImportantASTNodeType, Attributes>
{
    Dictionary<VariableName, SmallLangType> VariableNameToType = Functions.Values.FunctionNameToFunctionID.Select(x => TypeData.Void).Zip(Functions.Values.FunctionNameToFunctionID.Keys).Select(x => (new VariableName(x.Second), x.First)).ToDictionary();
    public Func<Node?, Node, bool> Dispatch(Node node)
    {
        return Combine(node.NodeType switch
        {
            ImportantASTNodeType.FunctionCall => FunctionCall,
            ImportantASTNodeType.Section => (x, y) => false,
            ImportantASTNodeType.Identifier => Primary,
            ImportantASTNodeType.Primary => Primary,
            ImportantASTNodeType.ArgList => (x, y) => false,
            ImportantASTNodeType.ArgListElement => ArgListElement,
            ImportantASTNodeType.NewExpr => NewExpr,
            ImportantASTNodeType.GenericType => GenericType,
            ImportantASTNodeType.TypeCSV => (x, y) => false,
            ImportantASTNodeType.BaseType => BaseType,
            ImportantASTNodeType.Declaration => Declaration,
            ImportantASTNodeType.AssignmentPrime => AssignmentPrime,
            ImportantASTNodeType.DeclarationModifiersCombined => (x, y) => false,
            ImportantASTNodeType.DeclarationModifier => (x, y) => false,
            _ => throw new Exception(node.NodeType.ToString())
        });
    }
    bool Identity(Node? parent, Node self)
    {
        var oldattr = self.Attributes;
        if (self.Attributes.VariablesInScope is null)
        {
            self.Attributes = self.Attributes with { VariablesInScope = new() };
            return true;
        }
        if (parent is not Node nnp) return false;
        self.Attributes = self.Attributes with { VariablesInScope = ((Scope)nnp.Attributes.VariablesInScope!).ScopeUnion((Scope)self.Attributes.VariablesInScope!) };
        return Changed(oldattr, self.Attributes);
    }
    Func<Node?, Node, bool> Combine(Func<Node?, Node, bool> inner)
    {
        bool Inner(Node? parent, Node self)
        {
            var c1 = Identity(parent, self);
            var c2 = inner(parent, self);
            return c1 || c2;
        }
        return Inner;
    }
    bool Changed(Attributes oldattr, Attributes newattr) => (oldattr == newattr) is false;
    private bool GenericType(Node? parent, Node self) => BaseType(parent, self);
    private bool AssignmentPrime(Node? parent, Node self)
    {
        var oldattr = self.Attributes;
        self.Attributes = self.Attributes with { TypeOfExpression = self.Children[0].Attributes.TypeOfExpression };
        return Changed(oldattr, self.Attributes);
    }
    bool NewExpr(Node? parent, Node self)
    {
        if (self.Children.Count == 1) return false;
        var args = self.Children[1];
        var types = self.Children[0].Children[0].Children;
        if (types.Any(x => x.Attributes.TypeLiteralType is null)) return false;
        var TTypes = types.Select(x => x.Attributes.TypeLiteralType!).ToArray();
        var j = args.Children.Where(x => x.Attributes.TypeOfExpression is not null).Select((x, i) => (Node: x, IsValid: x.Attributes.TypeOfExpression!.ImplicitCast(TTypes[i % TTypes.Length]), Expected: TTypes[i % TTypes.Length])).Where(x => x.IsValid is false).Select(x => (x.Node, x.Expected)).Select(x => new TypeErrorException(x.Expected, x.Node.Attributes.TypeOfExpression!, x.Node.GetLine()));
        if (!j.Any()) return false;
        throw j.First();
    }
    private bool ArgListElement(Node? parent, Node self)
    {
        var oldattr = self.Attributes;
        self.Attributes = self.Attributes with { TypeOfExpression = self.Children[0].Attributes.TypeOfExpression };
        return Changed(oldattr, self.Attributes);
    }
    private bool Declaration(Node? parent, Node self)
    {
        var oldattr = self.Attributes;
        self.Attributes = self.Attributes with { VariableName = new(self.Data!.Lexeme), TypeOfExpression = self.Children[0].Attributes.TypeLiteralType };
        bool Changed2 = !(((Scope)parent!.Attributes.VariablesInScope!).Contains(self.Attributes.VariableName));
        if (Changed2)
        {
            parent!.Attributes = parent.Attributes with { VariablesInScope = ((Scope)parent.Attributes.VariablesInScope).Append(self.Attributes.VariableName) };
        }
        bool Changed3 = false;
        if (!VariableNameToType.ContainsKey(self.Attributes.VariableName) || VariableNameToType[self.Attributes.VariableName] != self.Attributes.TypeOfExpression!)
        {
            VariableNameToType[self.Attributes.VariableName] = self.Attributes.TypeOfExpression!;
            Changed3 = true;
        }
        if (self.Children[^1].NodeType == ImportantASTNodeType.AssignmentPrime
        &&
        self.Children[0].Attributes.TypeLiteralType is not null && self.Children[0].Attributes.TypeLiteralType is SmallLangType type
        &&
        self.Children[^1].Attributes.TypeOfExpression is not null && self.Children[^1].Attributes.TypeOfExpression is SmallLangType srctype
        &&
        !srctype.CanDeclareTo(type)
        )
        {
            throw new TypeErrorException(type, srctype, self.Children[^1].GetLine());
        }
        return Changed(oldattr, self.Attributes) || Changed2 || Changed3;
    }
    private bool BaseType(Node? parent, Node self)
    {
        var oldattr = self.Attributes;
        var typename = self.Data!.Lexeme;
        var Typecode = TypeData.GetType(typename);
        self.Attributes = self.Attributes with { TypeLiteralType = Typecode };
        return Changed(oldattr, self.Attributes);
    }
    private bool FunctionCall(Node? parent, Node self)
    {
        FunctionID<BackingNumberType> ID = Functions.Values.FunctionNameToFunctionID[self.Children[0].Data!.Lexeme];
        SmallLangType RetType = Functions.Values.FunctionToRetType[ID];
        var oldattr = self.Attributes;
        self.Attributes = self.Attributes with { FunctionID = ID, DeclArgumentTypes = Functions.Values.FunctionToFunctionArgs[ID], TypeOfExpression = RetType };
        if (self.Children.Count == 2 && self.Children[^1].NodeType == ImportantASTNodeType.ArgList && self.Attributes.DeclArgumentTypes is not null && self.Attributes.DeclArgumentTypes is List<SmallLangType> NN)
        {
            var x = self.Children[^1].Children.Zip(NN).Where(x => x.First.Attributes.TypeOfExpression is not null).Where(x => x.First.Attributes.TypeOfExpression != x.Second).Select(x => new TypeErrorException(Expected: x.Second, Actual: x.First.Attributes.TypeOfExpression!, x.First.GetLine()));
            //TODO: Log all of x
            if (x.Any())
            {
                throw x.First();
            }
        }
        return Changed(oldattr, self.Attributes);
    }
    private bool Primary(Node? parent, Node self)
    {
        var oldattr = self.Attributes;
        self.Attributes = self.Attributes with
        {
            TypeOfExpression =

        self.Data!.TT switch
        {
            TokenType.String => self.Data.Literal.Length > 3 ? TypeData.String : TypeData.Char,
            TokenType.TrueLiteral => TypeData.Bool,
            TokenType.FalseLiteral => TypeData.Bool,
            TokenType.Number => self.Data.Literal.Contains('.') ? TypeData.Float : TypeData.Int,
            TokenType.Identifier => self.Attributes.VariableName is null ? null : VariableNameToType[self.Attributes.VariableName],
            _ => throw new Exception($"Unknown primary type {self.Data.TT}"),
        },
            VariableName = new(self.Data.Lexeme),

        };
        return Changed(oldattr, self.Attributes);
    }
}
