using System.Diagnostics;
using System.Numerics;
using Common.Dispatchers;
using Common.LinearIR;
using SmallLang.LinearIR;
using SmallLang.Metadata;
using static SmallLang.ImportantASTNodeType;

namespace SmallLang.Frontend.CodeGen;

public partial class CodeGenerator(Node RootNode)
{
    private const byte TrueValue = 0xFF;
    private const byte FalseValue = 0;
    int CurrentChunkPtr => data.Sections.CurrentChunkPtr;
    void Emit(Operation<Opcode, BackingNumberType> Op)
    {
        data.Sections.CurrentChunk.Add(Op);
    }
    void Emit(Opcode op, params IOperationArgument<byte>[] args)
    {
        Emit(new Operation<Opcode, byte>((OpcodeWrapper)op, args));
    }
    void NewChunk() => data.Sections.NewChunk();
    int ParseBeginningChunk = 0;
    void SETCHUNK() => ParseBeginningChunk = CurrentChunkPtr;
    GenericNumberWrapper<int> RCHUNK(int ChunkRelOffset) => new GenericNumberWrapper<int>(CurrentChunkPtr + ChunkRelOffset);

    GenericNumberWrapper<int> ACHUNK(int ChunkRelOffset) => new GenericNumberWrapper<int>(ParseBeginningChunk + ChunkRelOffset);
    private readonly Data data = new();
    public Data Parse()
    {
        DynamicDispatch(RootNode);
        return data;
    }
    private void Verify(Node node, ImportantASTNodeType Expected)
    {
        Debug.Assert(node.NodeType == Expected);
    }
    private void DynamicDispatch(Node node) =>
    node.DispatchNodeType(

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
    private void DispatchExpressionType(Node node, params (SmallLangType, Action<Node>)[] Cases)
    {
        node.DispatchGeneric(Cases.Select<(SmallLangType, Action<Node>), (Func<Node, bool>, Action<Node>)>(x => (y => y.Attributes.TypeOfExpression == x.Item1, x.Item2)).ToArray());
    }
}