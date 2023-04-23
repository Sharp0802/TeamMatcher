namespace TeamMatcher.Lib.Algorithms;

internal static class StableConcatExtension
{
    internal static IEnumerable<IEnumerable<TItem>?> StableConcat<TItem>(
        this IEnumerable<IEnumerable<TItem>?> self, 
        IEnumerable<IEnumerable<TItem>?> other)
    {
        return self
            .OrderBy(i => i?.Count() ?? -1)
            .Zip(other.OrderByDescending(i => i?.Count() ?? -1))
            .Select(i => i.First is null 
                ? i.Second
                : i.Second is null 
                    ? i.First
                    : i.First.Concat(i.Second));
    }
}