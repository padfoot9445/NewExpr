using Common.AST;

namespace SmallLang;
public record class Attributes(List<object>? DeclArguments, bool? IsActualFunctionName) : IMetadata
{
    public Attributes() : this(DeclArguments: null, null) { }
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