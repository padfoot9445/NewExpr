using SmallLang.IR.Metadata;

namespace SmallLang.Exceptions;

public class TypeErrorException : ExpaException
{
    public readonly SmallLangType Expected;
    public readonly SmallLangType Actual;
    public readonly int Position;
    public readonly string? AdditionalMessage;
    public TypeErrorException(SmallLangType Expected, SmallLangType Actual, int Position, string? Message = null) : base($"Expected A value of type {Expected} but was {Actual} at {Position}." + (Message is null ? "" : $"Additional Context: {Message}"))
    {
        this.Expected = Expected;
        this.Actual = Actual;
        this.Position = Position;
        this.AdditionalMessage = Message;
    }
}