using Common.Metadata;
using SmallLang.Constants;
using SmallLang.LinearIR;

namespace SmallLang.Metadata;

public record class SmallLangType(uint BaseValue, string Name, bool IsRefType = false, uint Size = 1, bool IsNum = false) : GenericNumberWrapper<uint>(BaseValue), IMetadataTypes<SmallLangType>
{
    public bool CanDeclareTo(SmallLangType other)
    {
        return TypeData.Data.CanDeclareTo(this, other);
    }
    public bool ImplicitCast(SmallLangType other)
    {
        return TypeData.Data.ImplicitCastTo(this, other);
    }
}