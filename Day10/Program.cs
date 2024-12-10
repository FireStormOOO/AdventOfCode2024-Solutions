using System.Diagnostics;
using Common;
using Day10;
using static Common.TupleMath<int>;

var offsets = new[] { (0, 1), (0, -1), (-1, 0), (1, 0) };
int Calculate(string input, bool part1=true)
{
    var grid = new IntGrid(input);
    var trailheads = new List<(int, int)>();
    for (int i = 0; i < grid.Width; i++)
        for (int j = 0; j < grid.Height; j++)
                if(grid[i][j] == 0)
                    trailheads.Add((i,j));

    var tally = 0;
    foreach (var trailhead in trailheads)
    {
        List<(int, int)> GetAdjacent((int X, int Y) index)
        {
            return offsets.Select(offset => Add(index, offset))
                .Where(grid.BoundsCheck).ToList();
        }

        var searchHeads = new List<(int, int)>{trailhead};
        var path = new List<(int, int)>();
        var goals = new List<(int, int)>();
        while (searchHeads.Count>0)
        {
            var adj = GetAdjacent(searchHeads[0])
                .Where(xy=>!path.Union(goals).Union(searchHeads).Contains(xy)).ToList();
            var searchHeads0Val = grid.Index(searchHeads[0]);
            foreach (var tile in adj)
                if (grid.Index(tile) == searchHeads0Val + 1)
                    searchHeads.Add(tile);
            path.Add(searchHeads[0]);
            if(searchHeads0Val==9)
                goals.Add(searchHeads[0]);
            searchHeads.RemoveAt(0);
        }

        tally += goals.Distinct().Count();
    }

    return tally;
}

string testInput = "89010123\n78121874\n87430965\n96549874\n45678903\n32019012\n01329801\n10456732";
Debug.Assert(36 == Calculate(testInput));
Console.WriteLine($"Sum of trailhead scores is {Calculate(PuzzleInput.Input)}");

//Debug.Assert( == Calculate(testInput,false));
//Console.WriteLine($"{Calculate(PuzzleInput.Input,false)}");

Console.WriteLine($"Done!");