using Common.Metadata;
using SmallLang.IR.LinearIR;
using SmallLang.IR.Metadata;

namespace SmallLang.CodeGen.Frontend;

public record class Data
{
    public Dictionary<LoopGUID, (int, int, int, int, int)> LoopData = new();

    private int NextRegister = 1;
    public StaticallyAllocatedDataArea<VariableName, BackingNumberType> StaticDataArea = new();

    public Data()
    {
        ChunkStack.Push(Sections);
    }

    public TreeChunk Sections { get; init; } = new(new Chunk(), []);
    public TreeChunk CurrentChunk => ChunkStack.Peek();
    private Stack<TreeChunk> ChunkStack { get; } = new();

    internal void NewChunk()
    {
        var chunk = new TreeChunk(new Chunk(), []);
        CurrentChunk.Children.Add(chunk);
        ChunkStack.Push(chunk);
    }

    internal void Rewind()
    {
        ChunkStack.Pop();
    }

    internal void Emit(HighLevelOperation Op)
    {
        CurrentChunk.Self.Add(Op);
    }

    internal int GetVariableStartRegister(VariableName Variable)
    {
        throw new NotImplementedException();
    }

    internal int GetVariableWidth(VariableName Variable)
    {
        throw new NotImplementedException();
    }

    internal int AllocateRegisters(VariableName Variable, int width)
    {
        throw new NotImplementedException();
    }

    public int GetRegister()
    {
        return NextRegister++;
    }
}