using JetBrains.Annotations;

namespace Common.LinqExtensions;

public static class LinqExtensions
{
    public static void Evaluate<T>([InstantHandle] this IEnumerable<T> Source)
    {
        foreach (var _ in Source) ;
    }
}