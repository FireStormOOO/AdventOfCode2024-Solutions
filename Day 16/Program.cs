using System.Diagnostics;
using Common;
using Day16;
using static Common.TupleMath<int>;



long Calculate(string input, bool part1=true)
{
    var map = new CharGrid(input);
    var startPos = map.FindFirst('S');
    var endPos = map.FindFirst('E');
    Dictionary<Directions, (int X, int Y)> offsets = new() { 
        { Directions.East, (1, 0) }, { Directions.North, (0, -1)},
        { Directions.West, (-1, 0)}, { Directions.South, (0, 1)}};
    long Hint((Directions Facing, (int X, int Y) Pos, long cost, bool justTurned, long hint) action)
    {
        var delta = Sub(endPos, action.Pos);
        var hint = 0L;
        if (delta.X > 0 && action.Facing != Directions.West)
            hint += 1000L;
        if (delta.X < 0 && action.Facing != Directions.East)
            hint += 1000L;
        if (delta.Y > 0 && action.Facing != Directions.South)
            hint += 1000L;
        if (delta.Y < 0 && action.Facing != Directions.North)
            hint += 1000L;
        return hint + Math.Abs(delta.X) * Math.Abs(delta.Y);
    }

    (Directions Facing, (int X, int Y) Pos, long cost, bool justTurned, long hint) candidate = (Directions.East, startPos, 0, false, 0);
    candidate.hint = Hint(candidate);
    List<(Directions Facing, (int X, int Y) Pos, long cost, bool justTurned, long hint)> candidates = [candidate];
    List<(Directions Facing, (int X, int Y) Pos)> visited = [];
    while (candidates.Count > 0)
    {
        candidates = candidates.Distinct().OrderBy(t => t.cost + t.hint).ToList();
        
        candidate = candidates[0];
        Debug.Assert(map.BoundsCheck(candidate.Pos));
        candidates.RemoveAt(0);
        var visit = (candidate.Facing, candidate.Pos);
        if (visited.Contains(visit))
            continue;
        visited.Add(visit);
        
        if(candidate.Pos == endPos)
            break;
        if (!candidate.justTurned || candidate.Pos == startPos)
        {
            var turned = candidate;
            turned.cost += 1000;
            turned.justTurned = true;
            turned.Facing = (Directions)(((int)turned.Facing + 1) % 4);
            if (!visited.Contains((turned.Facing, turned.Pos)))
            {
                turned.hint = Hint(turned);
                candidates.Add(turned);
            }
            turned.Facing = (Directions)(((int)turned.Facing + 2) % 4);
            if (!visited.Contains((turned.Facing, turned.Pos)))
            {
                turned.hint = Hint(turned);
                candidates.Add(turned);
            }
        }

        var forward = candidate;
        forward.cost++;
        forward.Pos = Add(forward.Pos, offsets[forward.Facing]);
        forward.justTurned = false;
        forward.hint = Hint(forward);
        if(map.Index(forward.Pos) != '#')
            candidates.Add(forward);
        Debug.Assert(candidates.Count < 100 * map.Width * map.Height);
    }


    return candidate.cost;
}

string testInput = "###############\n#.......#....E#\n#.#.###.#.###.#\n#.....#.#...#.#\n#.###.#####.#.#\n#.#.#.......#.#\n#.#.#####.###.#\n#...........#.#\n###.#.#####.#.#\n#...#.....#.#.#\n#.#.#.###.#.#.#\n#.....#...#.#.#\n#.###.#.#.#.#.#\n#S..#.....#...#\n###############";
string testInput2 =
    "#################\n#...#...#...#..E#\n#.#.#.#.#.#.#.#.#\n#.#.#.#...#...#.#\n#.#.#.#.###.#.#.#\n#...#.#.#.....#.#\n#.#.#.#.#.#####.#\n#.#...#.#.#.....#\n#.#.#####.#.###.#\n#.#.#.......#...#\n#.#.###.#####.###\n#.#.#...#.....#.#\n#.#.#.#####.###.#\n#.#.#.........#.#\n#.#.#.#########.#\n#S#.............#\n#################";

Debug.Assert(7036 == Calculate(testInput));
Debug.Assert(11048 == Calculate(testInput2));

Console.WriteLine($"Best path has cost {Calculate(PuzzleInput.Input)}");

Console.WriteLine($"Done!");

enum Directions
{
    East=0,
    North=1,
    West=2,
    South=3
}