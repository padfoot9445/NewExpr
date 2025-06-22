namespace SmallLang;

public record class BaseUIntWrapper(uint Value)
{
    public static implicit operator uint(BaseUIntWrapper value) => value.Value;
    public static explicit operator BaseUIntWrapper(uint value) => new(value);
}