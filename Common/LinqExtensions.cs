namespace Common.LinqExtensions;

public static class LinqExtensions
{
    public static IEnumerable<T> Evaluate<T>(this IEnumerable<T> Source)
    {
        return Source.ToArray();
    }
}