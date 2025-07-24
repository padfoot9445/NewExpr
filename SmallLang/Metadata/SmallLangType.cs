using SmallLang.Constants;

namespace SmallLang.Metadata;

public record class SmallLangType(uint Value, string Name, bool IsRefType = false, uint Size = 1, bool IsNum = false) : BaseUIntWrapper(Value)
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