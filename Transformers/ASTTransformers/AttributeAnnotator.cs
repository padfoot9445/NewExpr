using System.Data.Common;
using System.Diagnostics;
using Common.AST;
using Common.Tokens;
using IRs.ParseTree;
using static Common.AST.ASTLeafType;
using Common.Logger;
using Common.Transformers.ASTTransformers;
namespace Transformers.ASTTransformers;
public class AttributeAnnotator
{
    public IValidASTLeaf BaseNode { get; init; }
    public AnnotatedNode<AttributeRecord> AnnotatedNode { get; init; }
    public ILogger Logger { get; init; }
    public AttributeAnnotator(IValidASTLeaf leaf, ILogger? logger = null)
    {
        Logger = logger ?? new Logger();
        ITM = new(Logger);
        BaseNode = leaf;
        TOP = new(TCP);
        AnnotatedNode = AnnotatedNode<AttributeRecord>.FromNodeRecursive(new(), leaf.MinimizeRemoveUnnecessaryNodes());
    }
    private MEXPTypeCodeProvider TCP { get; } = new();
    /// <summary>
    /// Annotates the AnnotatedNode property in-place and returns it.
    /// </summary>
    public AnnotatedNode<AttributeRecord> Annotate()
    {
        AnnotateNode(AnnotatedNode);
        return AnnotatedNode;
    }
    /// <summary>
    /// Annotates a single node
    /// </summary>
    /// <param name="node"></param>
    private void AnnotateNode(AnnotatedNode<AttributeRecord> node)
    {
        if (node.Descend() is not IValidASTLeaf leaf) return; //a blank node gets a blank annotation
        if (node.Children.Length == 0) return; //should literally be the same thing as above lol
        //We'll only handle the cases where the node has syntatic meaning (e.g. matches a single production); where we can't derive meaning from the node, we'll just let it flow through without doing anything
        if (leaf is IToken token)
        {
            //Primary Productions
            if (node.MatchDescendEnd([Terminal]) && IsLiteral(token))
            {
                //[0-9]+
                //node is terminal & literal
                Debug.Assert(node.Children.Length == 1); //should be redundant since matchdescenend is tested, but eh
                node.Attributes.TypeCode = GetTypeFromLiteral(token);
            }
            else if (node.MatchDescendEnd([Terminal, null, Terminal]) && token.TT == TokenType.OpenParen)
            {
                //"(" Expr ")"
                //Propagate the type of Expr upwards into this node
                AnnotatedNode<AttributeRecord> anode = (AnnotatedNode<AttributeRecord>)node.Children[1];
                AnnotateNode(anode);
                node.Attributes.TypeCode = anode.Attributes.TypeCode;
            }
            else if (token.TT == TokenType.Identifier)
            {
                //Identifier AssignmentPrime
                if (!node.MatchDescendEnd([Terminal]))
                {
                    //Case where AssignmentPrime is epsilon
                    Debug.Assert(node.Children.Length == 1);
                    node.Attributes.TypeCode = ITM.Get(token);
                }
                else
                {
                    //Case where AssignmentPrime is not epsilon (thus it is another node)

                    Debug.Assert(node.Children.Length == 2);
                    AnnotatedNode<AttributeRecord> AssignmentPrime = (AnnotatedNode<AttributeRecord>)node.Children[1];
                    AnnotateNode(AssignmentPrime);
                    node.Attributes.TypeCode = AssignmentPrime.Attributes.TypeCode;
                    // Debug.Assert(node.Children[1] is AnnotatedNode<AttributeRecord> anode && anode.Children.Length >= 2 && anode.Children[0].Descend() is IToken anodeT && anodeT.TT == TokenType.Equals);
                }
            }
            //< as input
            else if (token.TT == TokenType.LessThan)
            {
#pragma warning disable CS0162
                throw new NotImplementedException("Not yet supported");
                node.Attributes.TypeCode = TCP.NumberTypeCode;
#pragma warning restore CS0162
            }
            //Negation
            else if (token.TT == TokenType.Subtraction)
            {
                Debug.Assert(node.Children.Length == 2);
                node.Attributes.TypeCode = TOP.Negation((uint)(AnnotateAndGet(node.Children[1]).TypeCode!));
            }
            //PowerPrime
            else if (token.TT == TokenType.Exponentiation)
            {
                //"**" Negation PowerPrime
                Debug.Assert(node.Children.Length >= 2 && node.Children.Length <= 3);
                var NegationAttr = AnnotateAndGet(node.Children[1]);
                if (node.Children.Length == 2)
                {
                    //case where PowerPrime is empty
                    node.Attributes.TypeCode = NegationAttr.TypeCode;
                }
                else
                {
                    //length must be 3; i.e. not empty
                    var ExpPAttr = AnnotateAndGet(node.Children[2]);
                    node.Attributes.TypeCode = TOP.Exponentiation((uint)(NegationAttr.TypeCode!), (uint)(ExpPAttr.TypeCode!));
                }
            }
        }
    }
    private AttributeRecord AnnotateAndGet(IValidASTLeaf leaf)
    {
        if (leaf is not AnnotatedNode<AttributeRecord> annnode) throw new InvalidOperationException("Cannot annotate a leaf node");
        AnnotateNode(annnode);
        return annnode.Attributes;
    }
    private TypeOperationProvider TOP;
    class TypeOperationProvider
    {
        private MEXPTypeCodeProvider TCP;
        public TypeOperationProvider(MEXPTypeCodeProvider tcp)
        {
            TCP = tcp;
            uint[] IntegerTypes = [TCP.ByteTypeCode, TCP.IntTypeCode, TCP.LongTypeCode, TCP.LongintTypeCode];
            uint[] DecimalTypes = [TCP.FloatTypeCode, TCP.DoubleTypeCode, TCP.NumberTypeCode];
            void AddInSameList(uint[] TypeList)
            {

                for (int i = 0; i < TypeList.Length; i++)
                {
                    uint FirstType = TypeList[i];
                    AddUnary(FirstType, FirstType); //numeric types negated are themselves
                    for (int j = i; j < TypeList.Length; j++)
                    {
                        uint SecondType = TypeList[j];
                        AddBinary(FirstType, SecondType, SecondType);
                        AddBinary(SecondType, FirstType, SecondType);
                        //result must be secondtype since j is always >= i
                    }
                }
            }
            AddInSameList(IntegerTypes);
            AddInSameList(DecimalTypes);
            for (int i = 0; i < IntegerTypes.Length; i++)
            {
                for (int j = 0; j < DecimalTypes.Length; j++)
                {
                    int DecimalTypeIndex = i == 3 ? 2 : Math.Max(i, j);
                    //Since we have the lesser-precision byte in the integers, this works.
                    //obviously in the case j > i,a double is able to represent a byte; and a infinite-precision number is able to represent a long
                    //in the case j < i, a float of precision-position i is able to represent an integer of precision-position i, as shown in the case j = i (this works because increasing the required precision of one of the numbers will only increase the result required precision, thus we can say that a precision sufficient for i = j will also work for i > j)
                    //in the case i = j, since we shifted the ints down by the inclusion of a byte, float can represent byte, double can represent int, and infinite precision can represent long
                    //in the edge case that i is an infinite-precision integer, in which case we can represent it with an infinite-precision number as well
                    AddBinary(IntegerTypes[i], DecimalTypes[j], DecimalTypes[DecimalTypeIndex]);
                    AddBinary(DecimalTypes[j], IntegerTypes[i], DecimalTypes[DecimalTypeIndex]);
                }
            }
        }
        private Dictionary<(uint, uint), uint> BinaryTable = new();
        private Dictionary<uint, uint> UnaryTable = new();
        public uint Exponentiation(uint Type1, uint Type2) => Binary(Type1, Type2) ?? throw new Exception();
        public uint Negation(uint Type1) => Unary(Type1) ?? throw new Exception();
        public uint? Binary(uint Type1, uint Type2) => TryGetResultOfOperation(BinaryTable, (Type1, Type2));
        public uint? Unary(uint Type1) => TryGetResultOfOperation(UnaryTable, Type1);
        private uint? TryGetResultOfOperation<T>(Dictionary<T, uint> Table, T operands) where T : notnull
        {
            //For numeric types, get the lowest common maximum; otherwise it's going to be user-defined so we don't really care
            if (Table.TryGetValue(operands, out uint ResultingType))
            {
                return ResultingType;
            }
            return null;
        }
        public void AddBinary(uint Type1, uint Type2, uint Result) => AddMapping(BinaryTable, (Type1, Type2), Result);
        public void AddUnary(uint Type1, uint Result) => AddMapping(UnaryTable, Type1, Result);
        private void AddMapping<T>(Dictionary<T, uint> Table, T operands, uint Result) where T : notnull
        {
            if (Table.ContainsKey(operands)) throw new Exception("Relationship already defined");
            Table[operands] = Result;
        }

    }

    private IdentifierTypeMapper ITM;
    class IdentifierTypeMapper
    {
        private ILogger Logger { get; }
        public IdentifierTypeMapper(ILogger logger)
        {
            Logger = logger;
        }
        private Dictionary<string, uint> IdentToTypecode = new();
        public void Store(IToken ident, uint TypeCode)
        {
            if (IdentToTypecode.ContainsKey(ident.Lexeme))
            {
                Logger.Log($"Shadowing identifier {ident.Lexeme}");
                //TODO: add position to the token
            }
            IdentToTypecode[ident.Lexeme] = TypeCode;
        }
        public uint Get(IToken ident)
        {
            if (!IdentToTypecode.TryGetValue(ident.Lexeme, out uint TypeCode))
            {
                Logger.Log($"Undefined identifier {ident.Lexeme}");
                throw new Exception($"Undefined identifier {ident.Lexeme}");
            }
            return TypeCode;
        }
    }
    /// <summary>
    /// Wrapper around AnnotateNode(AnnotatedNode node) that takes IValidASTLeaf instead, and throws if the argument passed is not AnnotateNode
    /// </summary>
    /// <param name="node"></param>
    /// <exception cref="Exception"></exception>
    private void AnnotateNode(IValidASTLeaf node)
    {
        if (node is not AnnotatedNode<AttributeRecord> anode) throw new Exception($"Invalid type of node; cannot annotate node of type {node.GetType().Name}");
        AnnotateNode(anode);
    }
    private bool IsLiteral(IToken token)
    {
        return token.TT == TokenType.Number;
    }
    private uint GetTypeFromLiteral(IToken token)
    {
        Debug.Assert(token.TT == TokenType.Number);
        if (token.Lexeme.Contains('.'))
        {
            return TCP.FloatTypeCode;
        }
        return TCP.IntTypeCode;
    }
}