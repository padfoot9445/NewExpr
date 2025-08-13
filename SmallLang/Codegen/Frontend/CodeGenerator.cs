using System.Diagnostics;
using Common.Dispatchers;
using Common.LinearIR;
using SmallLang.IR.AST;
using SmallLang.IR.LinearIR;
using SmallLang.IR.Metadata;
using NodeType = SmallLang.IR.AST.ImportantASTNodeType;

namespace SmallLang.CodeGen.Frontend;

public partial class CodeGenerator(Node RootNode)
{
    private const byte TrueValue = 0xFF;
    private const byte FalseValue = 0;
    int CurrentChunkPtr => Data.Sections.CurrentChunkPtr;
    internal void Cast(Node self, SmallLangType dstType)
    {
        if (self.Attributes.TypeLiteralType! == dstType) DynamicDispatch(self);
        throw new NotImplementedException();
    }
    internal void Emit(Operation<Opcode, BackingNumberType> Op)
    {
        Data.Sections.CurrentChunk.Add(Op);
    }
    internal void Emit(Opcode op, params IOperationArgument<byte>[] args)
    {
        Emit(new Operation<Opcode, byte>((OpcodeWrapper)op, args));
    }
    internal void NewChunk() => Data.Sections.NewChunk();
    int ParseBeginningChunk = 0;
    internal void SETCHUNK() => ParseBeginningChunk = CurrentChunkPtr;
    internal GenericNumberWrapper<int> RCHUNK(int ChunkRelOffset) => new GenericNumberWrapper<int>(CurrentChunkPtr + ChunkRelOffset);

    internal GenericNumberWrapper<int> ACHUNK(int ChunkRelOffset) => new GenericNumberWrapper<int>(ParseBeginningChunk + ChunkRelOffset);
    internal Data Data { get; init; } = new();
    public Data Parse()
    {
        DynamicDispatch(RootNode);
        return Data;
    }
    internal void Verify(Node node, ImportantASTNodeType Expected)
    {
        Debug.Assert(node.NodeType == Expected);
    }
    internal void DynamicDispatch(Node node) =>
        node.Switch(
            Accessor: x => x.NodeType,
            Comparer: (x, y) => x == y,


            (NodeType.Section, SectionVisitor.Visit),
            //TODO: Activate (Identifier, ParsePrimary)
            (NodeType.Function, FunctionVisitor.Visit),
            (NodeType.For, ForVisitor.Visit),
            (NodeType.While, WhileVisitor.Visit),
            (NodeType.Return, ReturnVisitor.Visit),
            (NodeType.LoopCTRL, LoopCtrlVisitor.Visit),
            (NodeType.Switch, SwitchVisitor.Visit),
            (NodeType.If, IfVisitor.Visit)
        )(node, this);
}