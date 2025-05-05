using Common.AST;

namespace SmallLang;
public record class Attributes(List<uint>? DeclArgumentTypes, uint? FunctionID) : IMetadata
{
    public Attributes() : this(DeclArgumentTypes: null, null) { }
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