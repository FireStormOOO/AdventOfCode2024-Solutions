using System.Diagnostics;
using Day19;

long Calculate(string input, bool part1=true)
{
    var patterns = input.Split('\n')[0].Split(", ");
    var designs = input.Split('\n')[2..];
    
    var count = 0L;
    foreach (var design in designs)
    {
        var lPatterns = patterns.Where(design.Contains).ToList();
        List<(string partial, long count)> remaining = lPatterns
            .Where(pattern => design.StartsWith(pattern))
            .Select(pattern => (design.Substring(pattern.Length), 1L))
            .ToList();
        while (remaining.Count > 0)
        {
            if (remaining[0].partial.Length == 0)
            {
                if (part1)
                {
                    count++;
                    break;
                }
                count += remaining[0].count;
                remaining.RemoveAt(0);
                continue;
            }
            List<(string partial, long count)> query = lPatterns
                .Where(remaining[0].partial.StartsWith)
                .Select(pattern => (remaining[0].partial.Substring(pattern.Length), remaining[0].count))
                .ToList();
            foreach (var q in query)
            {
                var match = remaining.FindIndex(r => r.partial == q.partial);
                if (match != -1)
                {
                    var temp = remaining[match];
                    temp.count += q.count;
                    remaining[match] = temp;
                }
                else remaining.Add(q);
            }
            remaining.RemoveAt(0);
            remaining = remaining.OrderByDescending(s => s.partial.Length).ToList();
            lPatterns = lPatterns.Where(p => remaining.Any(s => s.partial.Contains(p))).ToList();
        }
    }

    return count;
}

string testInput = "r, wr, b, g, bwu, rb, gb, br\n\nbrwrr\nbggr\ngbbr\nrrbgbr\nubwu\nbwurrg\nbrgr\nbbrgwb";
Debug.Assert(6 == Calculate(testInput));

Console.WriteLine($"Achievable designs: {Calculate(PuzzleInput.Input)}");

Debug.Assert(16 == Calculate(testInput, false));

Console.WriteLine($"Achievable solutions: {Calculate(PuzzleInput.Input, false)}");

Console.WriteLine($"Done!");