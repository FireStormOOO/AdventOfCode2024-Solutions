using System.Diagnostics;
using Common;
using Day12;
using static Common.TupleMath<int>;

var offsets = new[] { (0, 1), (0, -1), (-1, 0), (1, 0) };
long Calculate(string input, bool part1=true)
{
    var grid = new CharGrid(input);
    List<List<int>> regionMap = [];
    Dictionary<int, long> costs = new(); 
    for (int i = 0; i < grid.Width; i++)
    {
        regionMap.Add(new());
        for (int j = 0; j < grid.Height; j++)
            regionMap[i].Add(-1);
    }

    (int X, int Y) NextRegion()
    {
        for (int i = 0; i < grid.Width; i++)
            for (int j = 0; j < grid.Height; j++)
                if (regionMap[i][j]==-1)
                    return (i, j);
        return (-1, -1);
    }

    for (var nextRegion = NextRegion(); nextRegion != (-1,-1); nextRegion=NextRegion())
    {
        List<(int, int)> GetAdjacent((int X, int Y) index)
        {
            return offsets.Select(offset => Add(index, offset))
                .ToList();
        }
        var searchHeads = new List<(int X, int Y)>{nextRegion};
        var included = new List<(int X, int Y)>();
        var perimeter = 0;
        while (searchHeads.Count>0)
        {
            var adj = GetAdjacent(searchHeads[0])
                .Where(xy=>!included.Union(searchHeads).Contains(xy)).ToList();
            var searchHeads0Val = grid.Index(searchHeads[0]);
            foreach (var tile in adj)
                if (!grid.BoundsCheck(tile))
                    perimeter++;
                else if (grid.Index(tile) == searchHeads0Val)
                    searchHeads.Add(tile);
                else
                    perimeter++;
            included.Add(searchHeads[0]);
            searchHeads.RemoveAt(0);
        }

        var id = regionMap.SelectMany(list => list).Distinct().Order().Last() + 1;
        foreach (var tile in included) regionMap[tile.X][tile.Y] = id;
        
        if(part1)
            costs[id] = included.Count * perimeter;
        else
        {
            var corners = 0;//corners==sides
            foreach (var tile in included)
            {
                foreach (var diagonal in new (int X,int Y)[]{(1, 1),(1, -1),(-1, -1),(-1, 1)})
                {
                    if(included.Contains(Add(tile, (diagonal.X,0))) &&
                       included.Contains(Add(tile, (0,diagonal.Y))) &&
                        !included.Contains(Add(tile, diagonal)))
                        corners++;
                    
                    if(!included.Contains(Add(tile, (diagonal.X,0))) &&
                       !included.Contains(Add(tile, (0,diagonal.Y))))
                        corners++;
                }
            }
            costs[id] = included.Count * corners;
        }
    }

    return costs.Values.Sum();
}

string testInput = "AAAA\nBBCD\nBBCC\nEEEC";
Debug.Assert(140 == Calculate(testInput));
string testInput2 = "OOOOO\nOXOXO\nOOOOO\nOXOXO\nOOOOO";
Debug.Assert(772 == Calculate(testInput2));
string testInput3 = "RRRRIICCFF\nRRRRIICCCF\nVVRRRCCFFF\nVVRCCCJFFF\nVVVVCJJCFE\nVVIVCCJJEE\nVVIIICJJEE\nMIIIIIJJEE\nMIIISIJEEE\nMMMISSJEEE";
Debug.Assert(1930 == Calculate(testInput3));
Console.WriteLine($"Total fence cost is {Calculate(PuzzleInput.Input)}");

Debug.Assert(80==Calculate(testInput, false));
Debug.Assert(436==Calculate(testInput2, false));
string testInput4 = "EEEEE\nEXXXX\nEEEEE\nEXXXX\nEEEEE";
Debug.Assert(236==Calculate(testInput4, false));
string testInput5 = "AAAAAA\nAAABBA\nAAABBA\nABBAAA\nABBAAA\nAAAAAA";
Debug.Assert(368==Calculate(testInput5, false));
Debug.Assert(1206 == Calculate(testInput3, false));
Console.WriteLine($"Total fence cost with bulk discount is {Calculate(PuzzleInput.Input,false)}");

Console.WriteLine($"Done!");