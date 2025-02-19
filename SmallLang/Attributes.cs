using Common.AST;

namespace SmallLang;
record class Attributes : IMetadata
{
    public void ForcedMerge(IMetadata other, bool PrioritizeOther = false)
    {
        throw new NotImplementedException();
    }

    public bool IsEquivalentTo(IMetadata other)
    {
        throw new NotImplementedException();
    }

    public void Merge(IMetadata other)
    {
        throw new NotImplementedException();
    }
}