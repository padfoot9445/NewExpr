using System.Diagnostics;
using System.Numerics;
using Common.Dispatchers;
using Common.LinearIR;
using Common.Metadata;
using SmallLang.CodeGen.Frontend.CodeGeneratorFunctions;
using SmallLang.IR.AST;
using SmallLang.IR.AST.Generated;
using SmallLang.IR.LinearIR;
using SmallLang.IR.Metadata;
namespace SmallLang.CodeGen.Frontend;

public partial class CodeGenerator(SmallLangNode RootNode)
{
    internal const BackingNumberType TrueValue = BackingNumberType.MaxValue;
    internal const BackingNumberType FalseValue = BackingNumberType.MinValue;
    internal void Cast<T>(T self, SmallLangType dstType)
    where T : IHasAttributeTypeOfExpression, ISmallLangNode
    {
        if (self.TypeOfExpression! == dstType) Exec(self);
        throw new NotImplementedException();
    }
    internal void Emit(HighLevelOperation Op)
    {
        if (!IsInChunkFlag) throw new InvalidOperationException("Must be in a chunk to call Driver.Emit. Wrap the emit call in a suitable chunk.");
        Data.Emit(Op);
    }
    internal void EnteringChunk(Action code)
    {
        IsInChunkFlag = true;
        code();
        IsInChunkFlag = false;
    }
    private bool IsInChunkFlag { get; set; } = false;
    internal void NewChunk(int chunkID, Action code)
    {
        Debug.Assert(Data.CurrentChunk.Children.Count == (chunkID - 1));
        IsNextFlag = false;
        Data.NewChunk();
        IsInChunkFlag = true;
        code();
        IsInChunkFlag = false;
        if (!IsNextFlag)
        {
            Data.Rewind();
        }
    }
    private bool IsNextFlag { get; set; }
    private bool NextWasCalledFlag { get; set; }
    internal void Next()
    {
        NextWasCalledFlag = true;
        IsNextFlag = true;
    }
    internal RelativeChunkPointer ACHUNK(int v) => new(v);
    internal Data Data { get; init; } = new();
    public Data Parse()
    {
        Exec(RootNode);
        return Data;
    }
    static internal void Verify<T>(ISmallLangNode node) where T : ISmallLangNode
    {
        Debug.Assert(node is T);
    }
    static internal void CastNode<T>(ISmallLangNode node, out T OutNode)
    {
        OutNode = (T)node;
    }

    static internal bool TryCastNode<T>(ISmallLangNode node, out T? OutNode)
    {
        if (node is T)
        {
            CastNode(node, out OutNode);
            return true;
        }
        OutNode = default;
        return false;
    }
    internal void Exec(ISmallLangNode node)
    {
        var CurrentChunk = Data.CurrentChunk;
        DynamicDispatch(node)(node, this);
    }
    internal static Action<ISmallLangNode, CodeGenerator> VisitFunctionWrapper<T>(Action<T, CodeGenerator> visitor)
    where T : ISmallLangNode =>
        (x, y) =>
        {
            y.NextWasCalledFlag = false;
            Verify<T>(x);
            visitor((T)x, y);
            if (y.NextWasCalledFlag is false) throw new Exception($"{typeof(T)}: No exit point was set. Set an exit point by calling Next(); in a chunk.");
        };
    internal int[] GetRegisters(int Width = 1) => Enumerable.Range(0, Width).Select(_ => Data.GetRegister()).ToArray();
    internal int[] GetRegisters<T>(T Width) where T : INumber<T>
    {
        return GetRegisters(int.CreateTruncating(Width));
    }
    internal int[] GetRegisters(SmallLangType Type) => GetRegisters(Type.Size);
    internal int[] GetRegisters(IHasAttributeTypeOfExpression Node) => GetRegisters((int)Node.TypeOfExpression!.Size);
    internal TreeChunk GetChild(int ChunkID) => Data.CurrentChunk.Children[ChunkID - 1];

    static (Func<ISmallLangNode, bool>, Action<ISmallLangNode, CodeGenerator>) GetCase<T>(Action<T, CodeGenerator> Visitor)
    where T : ISmallLangNode
    {
        return (x => x is T, VisitFunctionWrapper(Visitor));
    }

    static Action<ISmallLangNode, CodeGenerator> DynamicDispatch(ISmallLangNode node) =>
        node.Dispatch(
                Accessor: x => x,

                GetCase<SectionNode>(SectionVisitor.Visit),
                GetCase<IdentifierNode>(PrimaryVisitor.VisitIdentifier),
                GetCase<FunctionNode>(FunctionVisitor.Visit),
                GetCase<ForNode>(ForVisitor.Visit),
                GetCase<WhileNode>(WhileVisitor.Visit),
                GetCase<ReturnNode>(ReturnVisitor.Visit),
                GetCase<LoopCTRLNode>(LoopCtrlVisitor.Visit),
                GetCase<SwitchNode>(SwitchVisitor.Visit),
                GetCase<IfNode>(IfVisitor.Visit),
                GetCase<PrimaryNode>(PrimaryVisitor.Visit),
                GetCase<DeclarationNode>(DeclarationVisitor.Visit),
                GetCase<FactorialExpressionNode>(FactorialExpressionVisitor.Visit),
                GetCase<ElseNode>(ElseVisitor.Visit),
                GetCase<BinaryExpressionNode>(BinaryExpressionVisitor.Visit),
                GetCase<ComparisonExpressionNode>(ComparisonExpressionVisitor.Visit),
                GetCase<CopyExprNode>(CopyExpressionVisitor.Visit),
                GetCase<IndexNode>(IndexVisitor.Visit),
                GetCase<FunctionCallNode>(FunctionCallVisitor.Visit),
                GetCase<UnaryExpressionNode>(UnaryExpressionVisitor.Visit),
                GetCase<NewExprNode>(NewExpressionVisitor.Visit)

            );
    internal Pointer<BackingNumberType> AddStaticData(IEnumerable<BackingNumberType> Area)
    {
        var ptr = Data.StaticDataArea.Allocate(Area.Count());
        foreach (var i in Area)
        {
            Data.StaticDataArea.Store.Add(i);
        }
        return ptr;
    }
}