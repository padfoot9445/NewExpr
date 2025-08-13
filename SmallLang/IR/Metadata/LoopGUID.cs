namespace SmallLang.IR.Metadata;

public readonly record struct LoopGUID
{
    public LoopGUID()
    {
        ID = new Guid();
    }
    public readonly Guid ID;

}