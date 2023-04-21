using System.Numerics;

namespace TeamMatcher.Lib.Strategies;

internal class DiskSwappingShuffle<TItem, TValue> where TItem : class? where TValue : struct, INumber<TValue>
{
    private int CTeam { get; }

    private Func<TItem, TValue> ValueSelector { get; }

    private TItem[][] VDisk { get; }

    public DiskSwappingShuffle(IEnumerable<TItem> items, int cTeam, Func<TItem, TValue> valueSelector)
    {
        CTeam = cTeam;
        ValueSelector = valueSelector;
        VDisk = items.OrderByDescending(ValueSelector).Chunk(cTeam).ToArray();
        if (VDisk.Last().Length != cTeam)
        {
            var last = VDisk.Last();
            VDisk[^1] = last.Concat(Enumerable.Repeat<TItem>(null!, cTeam - last.Length)).ToArray();
        }
    }

    private TValue GetValue(TItem? item)
    {
        return item is null ? TValue.Zero : ValueSelector(item);
    }

    private void Shuffle()
    {
        foreach (var disk in VDisk)
            Random.Shared.Shuffle(disk);
    }
    
    private TValue[] GetSum()
    {
        var vSum = new TValue[CTeam];
        for (var i = 0; i < CTeam; ++i)
            vSum[i] = VDisk.Aggregate(TValue.Zero, (o, c) => o + GetValue(c[i]));
        return vSum;
    }
    
    private void Stable()
    {
        foreach (var disk in VDisk)
        {
            var vSum = GetSum();
            
            var alnDisk = disk
                .OrderByDescending(GetValue)
                .ToArray();
            
            var alnSum = vSum
                .Select((v, j) => (Value: v, Idx: j))
                .OrderBy(p => p.Value)
                .Select(p => p.Idx)
                .ToArray();

            for (var j = 0; j < CTeam; ++j)
                disk[alnSum[j]] = alnDisk[j];
        }
    }

    public TItem[][] ToChunk()
    {
        Shuffle();
        Stable();
        
        var vArr = new TItem[CTeam][];
        for (var i = 0; i < CTeam; ++i)
        {
            var lst = new Queue<TItem>();
            foreach (var disk in VDisk)
                if (disk[i] is not null)
                    lst.Enqueue(disk[i]);
            vArr[i] = lst.ToArray();
        }

        return vArr;
    }
}