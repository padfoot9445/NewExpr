using Common.Metadata;
using SmallLang.IR.LinearIR;
using SmallLang.IR.Metadata;

namespace SmallLang.CodeGen.Frontend;

public record class Data
{
    public Data()
    {
        CurrentChunk = Sections;
    }
    public TreeChunk Sections { get; init; } = new(new Chunk(), []);
    public TreeChunk CurrentChunk { get; private set; }
    public StaticallyAllocatedDataArea<VariableName, BackingNumberType> VariableSlots = new();
    public StaticallyAllocatedDataArea<VariableName, BackingNumberType> StaticDataArea = new();
    public Dictionary<LoopGUID, (GenericNumberWrapper<int> ContinueDSTChunk, GenericNumberWrapper<int> BreakDSTChunk)> LoopData = new();

    internal void NewChunk()
    {
        var chunk = new TreeChunk(new Chunk(), []);
        CurrentChunk.Children.Add(chunk);
        CurrentChunk = chunk;
    }
    internal void Emit(HighLevelOperation Op)
    {
        CurrentChunk.Self.Add(Op);
    }
}