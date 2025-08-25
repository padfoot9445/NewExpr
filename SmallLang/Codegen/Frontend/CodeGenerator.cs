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
        Data.Emit(Op);
    }
    internal void EnteringChunk(Action code)
    {
        code();
    }
    internal void NewChunk(int chunkID, Action code)
    {
        Debug.Assert(Data.CurrentChunk.Children.Count == (chunkID - 1));
        IsNextFlag = false;
        Data.NewChunk();
        code();
        if (!IsNextFlag)
        {
            Data.Rewind();
        }
    }
    private bool IsNextFlag { get; set; }
    internal void Next()
    {
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
            Verify<T>(x);
            visitor((T)x, y);
        };
    internal int[] GetRegisters(int Width = 1) => Enumerable.Range(0, Width).Select(_ => Data.GetRegister()).ToArray();
    internal int[] GetRegisters<T>(T Width) where T : INumber<T>
    {
        return GetRegisters(int.CreateTruncating(Width));
    }
    internal int[] GetRegisters(IHasAttributeTypeOfExpression Node) => GetRegisters((int)Node.TypeOfExpression!.Size);
    internal TreeChunk GetChild(int ChunkID) => Data.CurrentChunk.Children[ChunkID - 1];
    static Action<ISmallLangNode, CodeGenerator> DynamicDispatch(ISmallLangNode node) =>
        node.Dispatch(
                Accessor: x => x,

                (x => x is SectionNode, VisitFunctionWrapper<SectionNode>(SectionVisitor.Visit)),
                (x => x is IdentifierNode, VisitFunctionWrapper<IdentifierNode>(PrimaryVisitor.VisitIdentifier)),
                (x => x is FunctionNode, VisitFunctionWrapper<FunctionNode>(FunctionVisitor.Visit)),
                (x => x is ForNode, VisitFunctionWrapper<ForNode>(ForVisitor.Visit)),
                (x => x is WhileNode, VisitFunctionWrapper<WhileNode>(WhileVisitor.Visit)),
                (x => x is ReturnNode, VisitFunctionWrapper<ReturnNode>(ReturnVisitor.Visit)),
                (x => x is LoopCTRLNode, VisitFunctionWrapper<LoopCTRLNode>(LoopCtrlVisitor.Visit)),
                (x => x is SwitchNode, VisitFunctionWrapper<SwitchNode>(SwitchVisitor.Visit)),
                (x => x is IfNode, VisitFunctionWrapper<IfNode>(IfVisitor.Visit)),
                (x => x is PrimaryNode, VisitFunctionWrapper<PrimaryNode>(PrimaryVisitor.Visit)),
                (x => x is DeclarationNode, VisitFunctionWrapper<DeclarationNode>(DeclarationVisitor.Visit)),
                (x => x is FactorialExpressionNode, VisitFunctionWrapper<FactorialExpressionNode>(FactorialExpressionVisitor.Visit)),
                (x => x is ElseNode, VisitFunctionWrapper<ElseNode>(ElseVisitor.Visit))

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