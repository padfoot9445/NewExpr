namespace SmallLang;

public abstract record class BaseUIntWrapper(uint Value)
{
    public static implicit operator uint(BaseUIntWrapper value) => value.Value;
}