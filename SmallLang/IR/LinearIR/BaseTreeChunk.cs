namespace SmallLang.IR.LinearIR;

public abstract record class BaseTreeChunk(Chunk Self)
{
    protected abstract IEnumerable<BaseTreeChunk> Children { get; }
}