using System.Diagnostics;
using Day8;


(int, int) ToCoordByWidth(int index, int width) => (index % (width + 1), index / (width + 1));
(int, int) Add((int, int) A, (int, int) B) => (A.Item1 + B.Item1, A.Item2 + B.Item2);
(int, int) Sub((int, int) A, (int, int) B) => (A.Item1 - B.Item1, A.Item2 - B.Item2);

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
                var diff = Sub(towers[i], towers[j]);
                antinodes.Add(Add(towers[i], diff));
                antinodes.Add(Sub(towers[j], diff));
            }
    }

    var antinodesDistinctInBounds = antinodes.Distinct().Where(BoundsCheck).ToList();
    return antinodesDistinctInBounds.Count;
}

Debug.Assert(14 == Calculate("............\n........0...\n.....0......\n.......0....\n....0.......\n......A.....\n............\n............\n........A...\n.........A..\n............\n............"));
Console.WriteLine($"There are {Calculate(PuzzleInput.Input)} distinct antinodes in bounds");

//Debug.Assert(11387 == Calculate("............\n........0...\n.....0......\n.......0....\n....0.......\n......A.....\n............\n............\n........A...\n.........A..\n............\n............",false));
//Console.WriteLine($"Corrected calibration sum is  {Calculate(PuzzleInput.Input,false)}");

Console.WriteLine($"Done!");