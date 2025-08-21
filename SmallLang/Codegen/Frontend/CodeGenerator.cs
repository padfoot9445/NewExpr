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
    internal void Verify(SmallLangNode node, ImportantASTNodeType Expected)
    {
        Debug.Assert(node.NodeType == Expected);
    }
    internal void Exec(SmallLangNode node) =>
        DynamicDispatch(node)(node, this);
    static Action<SmallLangNode, CodeGenerator> DynamicDispatch(SmallLangNode node) =>
        node.Switch(
                Accessor: x => x.NodeType,
                Comparer: (x, y) => x == y,


                (NodeType.Section, SectionVisitor.Visit),
                (NodeType.Identifier, PrimaryVisitor.Visit),
                (NodeType.Function, FunctionVisitor.Visit),
                (NodeType.For, ForVisitor.Visit),
                (NodeType.While, WhileVisitor.Visit),
                (NodeType.Return, ReturnVisitor.Visit),
                (NodeType.LoopCTRL, LoopCtrlVisitor.Visit),
                (NodeType.Switch, SwitchVisitor.Visit),
                (NodeType.If, IfVisitor.Visit),
                (NodeType.Primary, PrimaryVisitor.Visit),
                (NodeType.Declaration, DeclarationVisitor.Visit),
                (NodeType.FactorialExpression, FactorialExpressionVisitor.Visit)

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