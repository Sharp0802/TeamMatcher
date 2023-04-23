using System.Numerics;
using TeamMatcher.Lib.Strategies;

namespace TeamMatcher.Lib.Algorithms;

/// <summary>
/// A extension class providing the ways to shuffle items stably.
/// </summary>
public static class ShuffleExtension
{
    /// <summary>
    /// Shuffle items by specific key and kind stably into the chunks of the specific count.
    /// </summary>
    /// <param name="items">The items to be shuffled.</param>
    /// <param name="cBlock">The count of the chunks.</param>
    /// <param name="kindSelector">The selector of the kind of data.</param>
    /// <param name="keySelector">The selector of the kind of data.</param>
    /// <typeparam name="TItem">The type of item.</typeparam>
    /// <typeparam name="TKind">The type of kind of item.</typeparam>
    /// <typeparam name="TValue">The type of key of item.</typeparam>
    /// <returns></returns>
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
                .StableConcat(g.StableShuffle(cBlock, keySelector))
                .ToArray()!);
    }
        
    
    /// <summary>
    /// Shuffle items by specific key stably into the chunks of the specific count.
    /// </summary>
    /// <param name="items">The items to be shuffled.</param>
    /// <param name="cBlock">The count of the chunks.</param>
    /// <param name="keySelector">The selector of the kind of data.</param>
    /// <typeparam name="TItem">The type of item.</typeparam>
    /// <typeparam name="TValue">The type of key of item.</typeparam>
    /// <returns></returns>
    public static TItem[][] StableShuffle<TItem, TValue>(
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