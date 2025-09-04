using Common.Metadata;
using SmallLang.IR.LinearIR;

namespace SmallLang.IR.Metadata;

public record class SmallLangType(
    uint BaseValue,
    string Name,
    bool IsRefType = false,
    uint Size = 1,
    bool IsNum = false,
    NumberType NumberType = NumberType.None,
    bool IsCollection = false,
    int? ValMaxSize = null) : GenericNumberWrapper<BackingNumberType>((BackingNumberType)BaseValue),
    IMetadataTypes<SmallLangType>, ISmallLangType
{
    public bool CanDeclareTo(SmallLangType other)
    {
        return TypeData.CanDeclareTo(this, other);
    }

    public bool ImplicitCast(SmallLangType other)
    {
        return TypeData.ImplicitCastTo(this, other);
    }
}