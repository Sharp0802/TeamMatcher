using System.Numerics;

namespace TeamMatcher.Lib.Strategies;

// O(n^2)
internal class AggressiveSwappingShuffle<TItem, TValue> where TValue : struct, INumber<TValue>
{
    private IEnumerable<TItem> Items { get; }

    private Func<TItem, TValue> ValueSelector { get; }

    public AggressiveSwappingShuffle(IEnumerable<TItem> items, Func<TItem, TValue> valueSelector)
    {
        Items = items;
        ValueSelector = valueSelector;
    }

    private (TItem[], TItem[]) Divide(TItem[] dat)
    {
        var keys = Enumerable
            .Range(0, dat.Length)
            .OrderBy(_ => Random.Shared.Next())
            .ToArray();
        var l = keys
            .Take(dat.Length / 2)
            .Select(key => dat[key])
            .ToArray();
        var r = keys
            .TakeLast(dat.Length - dat.Length / 2)
            .Select(key => dat[key])
            .ToArray();

        Shuffle(l, r);

        l = l.OrderBy(ValueSelector).ToArray();
        r = r.OrderBy(ValueSelector).ToArray();
        return (l, r);
    }

    private void Shuffle(TItem[] l, TItem[] r)
    {
        var sl = l.Aggregate(default(TValue), (o, c) => o + ValueSelector(c));
        var sr = r.Aggregate(default(TValue), (o, c) => o + ValueSelector(c));
        var diff = (sl - sr) / TValue.CreateChecked(2);
        if (TValue.IsZero(diff))
            return;

        var diffMap = new Dictionary<(int, int), TValue>();
        for (var i = 0; i < l.Length; ++i)
        for (var j = 0; j < r.Length; ++j)
            diffMap.Add((i, j), ValueSelector(l[i]) - ValueSelector(r[j]));

        var diffV = diffMap
            .OrderBy(p => TValue.Abs(diff - p.Value))
            .ToArray();

        var cShfl = 0;
        var tDiff = TValue.Zero;
        foreach (var diffP in diffV)
        {
            if (TValue.Abs(diff - tDiff) < TValue.Abs(diff - (tDiff + diffP.Value)))
                break;
            tDiff += diffP.Value;
            cShfl++;
        }

        foreach (var ((iA, iB), _) in diffV.Take(cShfl))
        {
            (l[iA], r[iB]) = (r[iB], l[iA]);
        }
    }

    public TItem[][] ToChunk()
    {
        var (l, r) = Divide(Items.ToArray());
        Shuffle(l, r);
        return new[] { l, r };
    }
}