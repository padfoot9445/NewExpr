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
    internal void NewChunk() => Data.NewChunk();
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
        while (Data.CurrentChunk != CurrentChunk)
        {
            Data.Rewind();
        }
    }
    static Action<SmallLangNode, CodeGenerator> VisitFunctionWrapper<T>(Action<T, CodeGenerator> visitor)
    where T : SmallLangNode =>
        (x, y) =>
        {
            Verify<T>(x);
            visitor((T)x, y);
        };

    static Action<SmallLangNode, CodeGenerator> DynamicDispatch(SmallLangNode node) =>
        node.Switch(
                Accessor: x => x.NodeType,
                Comparer: (x, y) => x == y,


                (NodeType.Section, VisitFunctionWrapper<SmallLangNode>(SectionVisitor.Visit)),
                (NodeType.Identifier, VisitFunctionWrapper<SmallLangNode>(PrimaryVisitor.Visit)),
                (NodeType.Function, VisitFunctionWrapper<SmallLangNode>(FunctionVisitor.Visit)),
                (NodeType.For, VisitFunctionWrapper<SmallLangNode>(ForVisitor.Visit)),
                (NodeType.While, VisitFunctionWrapper<SmallLangNode>(WhileVisitor.Visit)),
                (NodeType.Return, VisitFunctionWrapper<SmallLangNode>(ReturnVisitor.Visit)),
                (NodeType.LoopCTRL, VisitFunctionWrapper<LoopCTRLNode>(LoopCtrlVisitor.Visit)),
                (NodeType.Switch, VisitFunctionWrapper<SmallLangNode>(SwitchVisitor.Visit)),
                (NodeType.If, VisitFunctionWrapper<SmallLangNode>(IfVisitor.Visit)),
                (NodeType.Primary, VisitFunctionWrapper<SmallLangNode>(PrimaryVisitor.Visit)),
                (NodeType.Declaration, VisitFunctionWrapper<SmallLangNode>(DeclarationVisitor.Visit)),
                (NodeType.FactorialExpression, VisitFunctionWrapper<SmallLangNode>(FactorialExpressionVisitor.Visit))

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