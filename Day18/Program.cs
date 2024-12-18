using System.Diagnostics;
using Common;
using Day18;
using static Common.TupleMath<int>;



long Calculate(CharGrid map)
{
    var startPos = (0, 0);
    var endPos = (map.Width - 1, map.Height - 1);

    long Hint(SearchEntry action)
    {
        var delta = Sub(endPos, action.Pos);
        return Math.Abs(delta.X) + Math.Abs(delta.Y);
    }

    SearchEntry candidate = new(Directions.East, startPos, 0, Hint, null);
    candidate.Hint = Hint(candidate);
    List<SearchEntry> candidates = [candidate];
    Dictionary<(int X, int Y), List<SearchEntry>> visited = [];
    List<SearchEntry> solutions = new();
    var solutionCost = long.MaxValue;
    while (candidates.Count > 0)
    {
        candidates = candidates.Where(c=>c.Cost < solutionCost).Distinct().OrderBy(t => t.Cost + t.Hint).ToList();
        
        candidate = candidates[0];
        Debug.Assert(map.BoundsCheck(candidate.Pos));
        candidates.RemoveAt(0);
        
        var visit = candidate.Pos;
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

        bool IsValidCandidate(SearchEntry toCheck) =>
            map.BoundsCheck(toCheck.Pos) &&
            map.Index(toCheck.Pos) != '#' &&
            (!visited.TryGetValue(toCheck.Pos, out var toCheckVisited)
             || toCheckVisited.All(se => se.Cost >= toCheck.Cost));
        
        var left = candidate.LeftSuccessor();
        if(IsValidCandidate(left))
            candidates.Add(left);
        var right = candidate.RightSuccessor();
        if(IsValidCandidate(right))
            candidates.Add(right);
        var forward = candidate.FwdSuccessor();
        if(IsValidCandidate(forward))
            candidates.Add(forward);
        
        Debug.Assert(candidates.Count < 100 * map.Width * map.Height);
    }

    foreach (var tile in solutions.First().GetFullPath()) map.Index(tile, 'O');
    Debug.Print(map.Print());
    return solutionCost;
}

CharGrid GetMaze(string input, int size, int takeFirstN)
{
    List<List<char>> grid = new();
    for (int i = 0; i < size; i++)
    {
        var row = new List<char>();
        grid.Add(row);
        for (int j = 0; j < size; j++) row.Add('.');
    }

    foreach (var pair in new NumArgsByLine<int>(input).Lines.Take(takeFirstN)) 
        grid[pair[1]][pair[0]] = '#';
    return new CharGrid(grid.Select(row => row.ToArray()).ToArray());
}

string testInput = "5,4\n4,2\n4,5\n3,0\n2,1\n6,3\n2,4\n1,5\n0,6\n3,3\n2,6\n5,1\n1,2\n5,5\n2,5\n6,5\n1,4\n0,4\n6,4\n1,1\n6,1\n1,0\n0,5\n1,6\n2,0";
var testGrid = GetMaze(testInput, 7, 12);
Debug.Print(testGrid.Print());

Debug.Assert(22 == Calculate(testGrid));

var mainPuzzle = GetMaze(PuzzleInput.Input, 71, 1024);
Debug.Print(mainPuzzle.Print());
//Console.WriteLine($"Best path has cost {Calculate(mainPuzzle)}");

Debug.Assert((6,1)==GetFirstBlocker(testInput, 7));
Console.WriteLine($"First blocker is {GetFirstBlocker(PuzzleInput.Input, 71)}");

(int X, int Y)  GetFirstBlocker(string input, int size)
{
    List<List<int>> tempGrid = new();
    for (int i = 0; i < size; i++)
    {
        var row = new List<int>();
        tempGrid.Add(row);
        for (int j = 0; j < size; j++) row.Add(-1);
    }
    var map = new IntGrid(tempGrid.Select(l => l.ToArray()).ToArray());

    var nextGroupId = 1;
    foreach (var pair in new NumArgsByLine<int>(input).Lines)
    {
        (int X, int Y) pos = (pair[0], pair[1]);

        List<((int X, int Y) Pos, int group)> neighbors = new();
        for (int i = -1; i < 2; i++) for (int j = -1; j < 2; j++)
        {
            var nPos = Add((i,j), pos);
            if (map.BoundsCheck(nPos) && map.Index(nPos) > -1) 
                neighbors.Add((nPos, map.Index(nPos)));
        }

        var neighboringGroups = neighbors.Select(n => n.group).Distinct().ToList();
        int mainGroup = -1;
        if (neighboringGroups.Count == 0)
        {
            map.Index(pos, nextGroupId++);
            continue;
        }
        if (neighboringGroups.Count == 1)
        {
            mainGroup = neighbors[0].group;
            map.Index(pos, mainGroup);
        }
        if (neighboringGroups.Count > 1)
        {
            mainGroup = neighboringGroups.Order().First();
            foreach (var group in neighboringGroups.Where(n=>n!=mainGroup))
                while (map.TryFindFirst(group, out var match)) 
                    map.Index(match!.Value, mainGroup);
            map.Index(pos, mainGroup);
        }

        var region = map.Where(xy => map.Index(xy) == mainGroup).ToList();
        if (region.Any(xy => xy.X == 0 || xy.Y == map.Height - 1) &&
            region.Any(xy => xy.X == map.Width - 1 || xy.Y == 0))
            return pos;
    }

    return (-1, -1);
}



Console.WriteLine($"Done!");