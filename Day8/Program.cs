using System.Diagnostics;
using Day8;


(int, int) ToCoordByWidth(int index, int width) => (index % (width + 1), index / (width + 1));
(int, int) Add((int, int) a, (int, int) b) => (a.Item1 + b.Item1, a.Item2 + b.Item2);
(int, int) Sub((int, int) a, (int, int) b) => (a.Item1 - b.Item1, a.Item2 - b.Item2);
(int, int) ScalarMult(int a, (int, int) b) => (a * b.Item1, a * b.Item2);

int Gcd(int a, int b)
{
    if (b == 0) return a;
    return Gcd(b, a % b);
}

int Calculate(string input, bool part1=true)
{
    var freqencies = input.Distinct().Where(c => !".\n".Contains(c)).ToList();
    var towersByW = new Dictionary<char, List<(int, int)>>(freqencies.Select(f=>new KeyValuePair<char, List<(int,int)>>(f,new List<(int, int)>())));
    int width = input.IndexOf('\n');
    int height = input.Count(c => c == '\n') + 1;
    bool BoundsCheck((int X, int Y) coord) => coord is { X: >= 0, Y: >= 0} && coord.X < width && coord.Y < height;
    (int, int) ToCoord(int index) => ToCoordByWidth(index, width);
    for (int i = 0; i < input.Length; i++)
    {
        var c = input[i];
        if (freqencies.Contains(c)) towersByW[c].Add(ToCoord(i));
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
                    var diff = (tempDiff.Item1 / gcd, tempDiff.Item2 / gcd);
                    var steps = Math.Max(width, height);
                    for (int k = 0; k < steps; k++)
                    {
                        antinodes.Add(Add(towers[i], ScalarMult(k, diff)));
                        antinodes.Add(Add(towers[i], ScalarMult(-k, diff)));
                    }
                }
            }
    }

    var antinodesDistinctInBounds = antinodes.Distinct().Where(BoundsCheck).ToList();
    return antinodesDistinctInBounds.Count;
}

Debug.Assert(14 == Calculate("............\n........0...\n.....0......\n.......0....\n....0.......\n......A.....\n............\n............\n........A...\n.........A..\n............\n............"));
Console.WriteLine($"There are {Calculate(PuzzleInput.Input)} distinct antinodes in bounds");

Debug.Assert(Gcd(24,6) == 6);
Debug.Assert(Gcd(24,9) == 3);
Debug.Assert(Gcd(9,24) == 3);
Debug.Assert(Gcd(48,18) == 6);
Debug.Assert(Gcd(7,13) == 1);

Debug.Assert(34 == Calculate("............\n........0...\n.....0......\n.......0....\n....0.......\n......A.....\n............\n............\n........A...\n.........A..\n............\n............",false));
Console.WriteLine($"There are {Calculate(PuzzleInput.Input,false)} distinct antinodes in bounds");

Console.WriteLine($"Done!");