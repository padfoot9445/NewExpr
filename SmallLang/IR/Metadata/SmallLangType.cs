using Common.Metadata;
using SmallLang.IR.LinearIR;

namespace SmallLang.IR.Metadata;

public record class SmallLangType(uint BaseValue, string Name, bool IsRefType = false, uint Size = 1, bool IsNum = false) : GenericNumberWrapper<BackingNumberType>((BackingNumberType)BaseValue), IMetadataTypes<SmallLangType>
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