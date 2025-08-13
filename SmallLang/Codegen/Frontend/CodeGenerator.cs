using System.Diagnostics;
using System.Numerics;
using Common.Dispatchers;
using Common.LinearIR;
using SmallLang.IR.AST;
using SmallLang.IR.LinearIR;
using SmallLang.Metadata;
using static SmallLang.IR.AST.ImportantASTNodeType;

namespace SmallLang.CodeGen.Frontend;

public partial class CodeGenerator(Node RootNode)
{
    private const byte TrueValue = 0xFF;
    private const byte FalseValue = 0;
    int CurrentChunkPtr => Data.Sections.CurrentChunkPtr;
    void Emit(Operation<Opcode, BackingNumberType> Op)
    {
        Data.Sections.CurrentChunk.Add(Op);
    }
    void Emit(Opcode op, params IOperationArgument<byte>[] args)
    {
        Emit(new Operation<Opcode, byte>((OpcodeWrapper)op, args));
    }
    void NewChunk() => Data.Sections.NewChunk();
    int ParseBeginningChunk = 0;
    void SETCHUNK() => ParseBeginningChunk = CurrentChunkPtr;
    GenericNumberWrapper<int> RCHUNK(int ChunkRelOffset) => new GenericNumberWrapper<int>(CurrentChunkPtr + ChunkRelOffset);

    GenericNumberWrapper<int> ACHUNK(int ChunkRelOffset) => new GenericNumberWrapper<int>(ParseBeginningChunk + ChunkRelOffset);
    private readonly Data Data = new();
    public Data Parse()
    {
        DynamicDispatch(RootNode);
        return Data;
    }
    private void Verify(Node node, ImportantASTNodeType Expected)
    {
        Debug.Assert(node.NodeType == Expected);
    }
    private void DynamicDispatch(Node node) =>
        node.Switch(
            Accessor: x => x.NodeType,
            Comparer: (x, y) => x == y,


            (Section, ParseSection),
            //TODO: Activate (Identifier, ParsePrimary)
            (Function, ParseFunction),
            (For, ParseFor),
            (While, ParseWhile),
            (Return, ParseReturn),
            (LoopCTRL, ParseLoopCTRL),
            (ImportantASTNodeType.Switch, ParseSwitch),
            (If, ParseIf)
        );
}