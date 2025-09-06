namespace SmallLang.IR.Metadata;

public readonly record struct LoopGUID
{
    public readonly Guid ID;

    public LoopGUID()
    {
        ID = Guid.NewGuid();
    }
}