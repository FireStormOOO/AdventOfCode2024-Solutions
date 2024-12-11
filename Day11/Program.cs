using System.Diagnostics;
using Common;
using Day11;

long Calculate(string input, int blinks = 25)
{
    var stones = new NumArgsByLine<long>(input).Lines[0];
    Dictionary<long, long> stoneCounts = new Dictionary<long, long>();
    stones.Distinct().Select(stone => (stone, (long)stones.Count(s => s == stone)))
        .ToList().ForEach(stonePair=> stoneCounts[stonePair.stone]=stonePair.Item2);
    
    Dictionary<long, List<(long stone, long count)>> resultCache = new();
    while (blinks > 0)
    {
        var cappedBlinks = int.Min(blinks, 5);
        Dictionary<long, long> stoneCountsNext = new Dictionary<long, long>();
        foreach (var stone in stoneCounts.Keys)
        {
            List<(long stone, long count)> stepResult;
            if (resultCache.TryGetValue(stone, out var cachedResult))
                stepResult = cachedResult;
            else
            {
                stepResult = CalculateInner([stone], cappedBlinks);
                resultCache[stone] = stepResult;
            }

            foreach (var stoneRes in stepResult)
            {
                stoneCountsNext.TryAdd(stoneRes.stone, 0);
                stoneCountsNext[stoneRes.stone] += stoneCounts[stone] * stoneRes.count;
            }
        }

        stoneCounts = stoneCountsNext;
        blinks -= cappedBlinks;
    }

    return stoneCounts.Values.Sum();
}
List<(long stone, long count)> CalculateInner(List<long> stones, int blinks)
{
    for (int blink = 0; blink < blinks; blink++)
    {
        var toAppend = new List<long>();
        for (int iStone = 0; iStone < stones.Count; iStone++)
        {
            var digits = (int)Math.Log10(stones[iStone]) + 1;
            if (stones[iStone] == 0)
                stones[iStone] = 1;
            else if (digits % 2 == 0)
            {
                var split = (int)Math.Pow(10, (digits >> 1));
                var left = stones[iStone] / split;
                var right = stones[iStone] % split;
                stones[iStone] = left;
                toAppend.Add(right);
            }
            else
                stones[iStone] *= 2024;
        }
        stones.AddRange(toAppend);

        if (stones.Count > int.MaxValue >> 2)
            throw new ArgumentOutOfRangeException();
    }

    return stones.Distinct().Select(stone => (stone, (long)stones.Count(s => s == stone))).ToList();
}

string testInput = "125 17";
Debug.Assert(55312 == Calculate(testInput));
Console.WriteLine($"There are {Calculate(PuzzleInput.Input)} stones after 25 blinks");

Console.WriteLine($"There are {Calculate(PuzzleInput.Input,75)} stones after 75 blinks");

Console.WriteLine($"Done!");