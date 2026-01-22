using JetBrains.Annotations;

namespace Common;

public sealed class Nothing
{
    private Nothing() { }

    [UsedImplicitly]
    public static Nothing GetNothing { get; } = new();

    public static Nothing DoNothing(Action _) => GetNothing;
}