using SmallLang.Constants;

namespace SmallLang.Metadata;

public record class SmallLangType(uint Value, string Name, bool IsRefType = false, uint Size = 1) : BaseUIntWrapper(Value)
{
    public bool CanDeclareTo(SmallLangType other)
    {
        return TypeData.Data.CanDeclareTo(this, other);
    }
}