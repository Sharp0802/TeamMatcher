using System.Numerics;
using TeamMatcher.Lib.Strategies;

namespace TeamMatcher.Lib.Algorithms;

public static class ShuffleExtension
{
    public static IEnumerable<TItem>[] StableShuffle<TItem, TKind, TValue>(
        this IEnumerable<TItem> items,
        int cBlock,
        Func<TItem, TKind> kindSelector,
        Func<TItem, TValue> keySelector)
        where TItem : class?
        where TValue : struct, INumber<TValue>
    {
        return items
            .GroupBy(kindSelector)
            .Aggregate(new IEnumerable<TItem>[cBlock], (c, g) => c
                .StableConcat(g.StableShuffleInternal(cBlock, keySelector))
                .ToArray()!);
    }
        
    
    internal static TItem[][] StableShuffleInternal<TItem, TValue>(
        this IEnumerable<TItem> items, 
        int cBlock, 
        Func<TItem, TValue> keySelector)
        where TItem : class?
        where TValue : struct, INumber<TValue>
    {
        if (cBlock < 1)
        {
            throw new InvalidOperationException();
        }
        
        switch (cBlock)
        {
            case 1:
                return new[] { items.ToArray() };
            case 2:
                return new AggressiveSwappingShuffle<TItem, TValue>(items, keySelector).ToChunk();
            default:
                return new DiskSwappingShuffle<TItem, TValue>(items, cBlock, keySelector).ToChunk();
        }
    }
}