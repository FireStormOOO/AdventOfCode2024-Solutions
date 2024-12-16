using System.Diagnostics;
using Common;
using Day16;
using static Common.TupleMath<int>;



long Calculate(string input, out int pathTileCount)
{
    var map = new CharGrid(input);
    var startPos = map.FindFirst('S');
    var endPos = map.FindFirst('E');

    long Hint(SearchEntry action)
    {
        var delta = Sub(endPos, action.Pos);
        var hint = 0L;
        var turnPenalty = 1000L;
        if (delta.X > 0 && action.Facing != Directions.West)
            hint += turnPenalty;
        if (delta.X < 0 && action.Facing != Directions.East)
            hint += turnPenalty;
        if (delta.Y > 0 && action.Facing != Directions.South)
            hint += turnPenalty;
        if (delta.Y < 0 && action.Facing != Directions.North)
            hint += turnPenalty;
        return hint + Math.Abs(delta.X) * Math.Abs(delta.Y);
    }

    SearchEntry candidate = new(Directions.East, startPos, 0, false, Hint, null);
    candidate.Hint = Hint(candidate);
    List<SearchEntry> candidates = [candidate];
    Dictionary<(Directions Facing, (int X, int Y) Pos), List<SearchEntry>> visited = [];
    List<SearchEntry> solutions = new();
    var solutionCost = long.MaxValue;
    while (candidates.Count > 0)
    {
        candidates = candidates.Where(c=>c.Cost < solutionCost).Distinct().OrderBy(t => t.Cost + t.Hint).ToList();
        
        candidate = candidates[0];
        Debug.Assert(map.BoundsCheck(candidate.Pos));
        candidates.RemoveAt(0);
        
        var visit = candidate.ToVisited();
        if (visited.TryGetValue(visit, out var tileVisits))
        {
            var cost = tileVisits.Min(se => se.Cost);
            if(cost < candidate.Cost)
                continue;
            if (cost > candidate.Cost)
                visited[visit] = [candidate];
            else
            {
                visited[visit].Add(candidate);
                continue;
            }
        }
        else
            visited[visit]=[candidate];

        if (candidate.Pos == endPos && candidate.Previous?.Pos != endPos)
        {
            solutions.Add(candidate);
            solutionCost = Math.Min(candidate.Cost, solutionCost);
        }
        
        if (!candidate.JustTurned || candidate.Pos == startPos)
        {
            var left = candidate.LeftSuccessor();
            if (!visited.TryGetValue(left.ToVisited(), out var ll) 
                || ll.All(se => se.Cost >= left.Cost))
                candidates.Add(left);
            var right = candidate.RightSuccessor();
            if (!visited.TryGetValue(right.ToVisited(), out var lr) 
                || lr.All(se => se.Cost >= right.Cost)) 
                candidates.Add(right);
        }

        var forward = candidate.FwdSuccessor();
        if(map.Index(forward.Pos) != '#')
            candidates.Add(forward);
        Debug.Assert(candidates.Count < 100 * map.Width * map.Height);
    }

    var tilesToExpand = solutions.SelectMany(sln => sln.GetFullPath()).Distinct().ToList();
    List<(int X, int Y)> allPathTiles = [];
    while (tilesToExpand.Count > 0)
    {
        var visit = tilesToExpand[0];
        tilesToExpand.RemoveAt(0);
        allPathTiles.Add(visit.Pos);
        if (visited[visit].Count > 1)
        {
            var path = visited[visit].Select(se=>se.Previous).SelectMany(se => se.GetFullPath());
            tilesToExpand.AddRange(path.Except(tilesToExpand));
        }
    }
    
    pathTileCount = allPathTiles.Distinct().Count();
    foreach (var tile in allPathTiles) map.Index(tile, 'O');
    Debug.Print(map.Print());
    return solutionCost;
}

string testInput = "###############\n#.......#....E#\n#.#.###.#.###.#\n#.....#.#...#.#\n#.###.#####.#.#\n#.#.#.......#.#\n#.#.#####.###.#\n#...........#.#\n###.#.#####.#.#\n#...#.....#.#.#\n#.#.#.###.#.#.#\n#.....#...#.#.#\n#.###.#.#.#.#.#\n#S..#.....#...#\n###############";
string testInput2 =
    "#################\n#...#...#...#..E#\n#.#.#.#.#.#.#.#.#\n#.#.#.#...#...#.#\n#.#.#.#.###.#.#.#\n#...#.#.#.....#.#\n#.#.#.#.#.#####.#\n#.#...#.#.#.....#\n#.#.#####.#.###.#\n#.#.#.......#...#\n#.#.###.#####.###\n#.#.#...#.....#.#\n#.#.#.#####.###.#\n#.#.#.........#.#\n#.#.#.#########.#\n#S#.............#\n#################";

var pathTileCount = 0;
Debug.Assert(7036 == Calculate(testInput, out pathTileCount));
Debug.Assert(pathTileCount == 45);
Debug.Assert(11048 == Calculate(testInput2, out pathTileCount));
Debug.Assert(pathTileCount == 64);

Console.WriteLine($"Best paths have cost {Calculate(PuzzleInput.Input, out pathTileCount)} and cover {pathTileCount} distinct tiles.");

Console.WriteLine($"Done!");