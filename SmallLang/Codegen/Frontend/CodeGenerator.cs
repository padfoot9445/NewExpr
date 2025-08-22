using System.Diagnostics;
using Common.Dispatchers;
using Common.LinearIR;
using Common.Metadata;
using SmallLang.CodeGen.Frontend.CodeGeneratorFunctions;
using SmallLang.IR.AST;
using SmallLang.IR.AST.Generated;
using SmallLang.IR.LinearIR;
using SmallLang.IR.Metadata;
using NodeType = SmallLang.IR.AST.ImportantASTNodeType;
namespace SmallLang.CodeGen.Frontend;

public partial class CodeGenerator(SmallLangNode RootNode)
{
    internal const BackingNumberType TrueValue = BackingNumberType.MaxValue;
    internal const BackingNumberType FalseValue = BackingNumberType.MinValue;
    internal void Cast(SmallLangNode self, SmallLangType dstType)
    {
        if (self.Attributes.TypeLiteralType! == dstType) Exec(self);
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
    static internal void Verify(SmallLangNode node, ImportantASTNodeType Expected)
    {
        Debug.Assert(node.NodeType == Expected);
    }
    static internal void Verify<T>(SmallLangNode node) where T : SmallLangNode
    {
        Debug.Assert(node is T);
    }
    internal void Exec(SmallLangNode node)
    {
        var CurrentChunk = Data.CurrentChunk;
        DynamicDispatch(node)(node, this);
    }
    static Action<SmallLangNode, CodeGenerator> VisitFunctionWrapper<T>(Action<T, CodeGenerator> visitor)
    where T : SmallLangNode =>
        (x, y) =>
        {
            Verify<T>(x);
            visitor((T)x, y);
        };
    internal int[] GetRegisters(int Width = 1) => Enumerable.Range(0, Width).Select(_ => Data.GetRegister()).ToArray();
    internal int[] GetRegisters(SmallLangNode Node) => GetRegisters((int)Node.Attributes.TypeOfExpression!.Size);
    internal TreeChunk GetChild(int ChunkID) => Data.CurrentChunk.Children[ChunkID - 1];
    static Action<SmallLangNode, CodeGenerator> DynamicDispatch(SmallLangNode node) =>
        node.Switch(
                Accessor: x => x.NodeType,
                Comparer: (x, y) => x == y,


                (NodeType.Section, VisitFunctionWrapper<SectionNode>(SectionVisitor.Visit)),
                (NodeType.Identifier, VisitFunctionWrapper<SmallLangNode>(PrimaryVisitor.Visit)),
                (NodeType.Function, VisitFunctionWrapper<FunctionNode>(FunctionVisitor.Visit)),
                (NodeType.For, VisitFunctionWrapper<ForNode>(ForVisitor.Visit)),
                (NodeType.While, VisitFunctionWrapper<WhileNode>(WhileVisitor.Visit)),
                (NodeType.Return, VisitFunctionWrapper<SmallLangNode>(ReturnVisitor.Visit)),
                (NodeType.LoopCTRL, VisitFunctionWrapper<LoopCTRLNode>(LoopCtrlVisitor.Visit)),
                (NodeType.Switch, VisitFunctionWrapper<SwitchNode>(SwitchVisitor.Visit)),
                (NodeType.If, VisitFunctionWrapper<IfNode>(IfVisitor.Visit)),
                (NodeType.Primary, VisitFunctionWrapper<SmallLangNode>(PrimaryVisitor.Visit)),
                (NodeType.Declaration, VisitFunctionWrapper<DeclarationNode>(DeclarationVisitor.Visit)),
                (NodeType.FactorialExpression, VisitFunctionWrapper<FactorialExpressionNode>(FactorialExpressionVisitor.Visit))

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