using SmallLang.Metadata;

namespace SmallLang;

public class TypeErrorException : ExpaException
{
    public TypeErrorException(string message) : base(message)
    {
    }
    public TypeErrorException(SmallLangType Expected, SmallLangType Actual, int Position, string? Message = null) : base($"Expected A value of type {Expected} but was {Actual} at {Position}." + (Message is null ? "" : $"Additional Context: {Message}"))
    {
    }
}