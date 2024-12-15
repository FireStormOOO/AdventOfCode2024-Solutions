using System.Diagnostics;
using Common;
using Day8;
using static Common.TupleMath<int>;
using static Common.ExtraMath<int>;

int Calculate(string input, bool part1=true)
{
    var grid = new CharGrid(input);
    var frequencies = input.Distinct().Where(c => !".\n".Contains(c)).ToList();
    var towersByW = new Dictionary<char, List<(int, int)>>(frequencies.Select(f=>new KeyValuePair<char, List<(int,int)>>(f,new List<(int, int)>())));
    
    for (int i = 0; i < grid.Width; i++)
    {
        for (int j = 0; j < grid.Height; j++)
        {
            var c = grid[j][i];
            if (frequencies.Contains(c)) towersByW[c].Add((i,j));
        }
    }
    var antinodes = new List<(int, int)>();
    foreach (var towersKV in towersByW)
    {
        var towers = towersKV.Value;
        for (int i = 0; i < towers.Count; i++)
            for (int j = i + 1; j < towers.Count; j++)
            {
                if (part1)
                {
                    var diff = Sub(towers[i], towers[j]);
                    antinodes.Add(Add(towers[i], diff));
                    antinodes.Add(Sub(towers[j], diff));
                }
                else
                {
                    var tempDiff = Sub(towers[i], towers[j]);
                    var gcd = Gcd(Math.Abs(tempDiff.Item1), Math.Abs(tempDiff.Item2));
                    var diff = ScalarDiv(gcd, tempDiff);
                    var steps = Math.Max(grid.Width, grid.Height);
                    for (int k = 0; k < steps; k++)
                    {
                        antinodes.Add(Add(towers[i], ScalarMult(k, diff)));
                        antinodes.Add(Add(towers[i], ScalarMult(-k, diff)));
                    }
                }
            }
    }

    var antinodesDistinctInBounds = antinodes.Distinct().Where(grid.BoundsCheck).ToList();
    return antinodesDistinctInBounds.Count;
}

Debug.Assert(14 == Calculate("............\n........0...\n.....0......\n.......0....\n....0.......\n......A.....\n............\n............\n........A...\n.........A..\n............\n............"));
Console.WriteLine($"There are {Calculate(PuzzleInput.Input)} distinct antinodes in bounds");

Debug.Assert(34 == Calculate("............\n........0...\n.....0......\n.......0....\n....0.......\n......A.....\n............\n............\n........A...\n.........A..\n............\n............",false));
Console.WriteLine($"There are {Calculate(PuzzleInput.Input,false)} distinct antinodes in bounds with the updated calculation");

Console.WriteLine($"Done!");