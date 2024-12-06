
using System.Diagnostics;
using Day6;

(int, int) ToCoordByWidth(int index, int width) => (index % width, index / width);

(int, int) Add((int, int) A, (int, int) B) => (A.Item1 + B.Item1, A.Item2 + B.Item2);
int Calculate(string input, bool part1=true)
{
    int width = input.IndexOf('\n');
    int height = input.Count(c => c == '\n') + 1;
    (int, int) ToCoord(int index) => ToCoordByWidth(index, width);
    input = input.Replace("\n", "");
    var guard = ToCoord(input.IndexOf('^'));
    bool BoundsCheck((int X, int Y) coord) => coord is { X: >= 0, Y: >= 0} && coord.X < width && coord.Y < height;
    List<(int, int)> obstacles = new();
    for (int i = 0; i < input.Length; i++)
    {
        if (input[i] != '#')
            continue;
        obstacles.Add(ToCoord(i));
    }
    List<(int, int)> visited = [guard];
    (int, int)[] dir = [(0, -1), (1, 0), (0, 1), (-1, 0)];
    int dirIndex = 0;
    while (BoundsCheck(guard))
    {
        var next = Add(guard, dir[dirIndex % 4]);
        if (obstacles.Contains(next))
            dirIndex++;
        else
        {
            visited.Add(guard);
            guard = next;
        }
    }

    if(part1)
        return visited.Distinct().Count();
    
    int loopTally = 0;
    foreach(var tempObstacle in visited.Distinct())
    {
        guard = ToCoord(input.IndexOf('^'));
        dirIndex = 0;
        if(guard == tempObstacle || obstacles.Contains(tempObstacle))
            continue;

        List<((int, int), int)> obstacleHit = new();
        obstacles.Add(tempObstacle);
        while (BoundsCheck(guard))
        {
            var next = Add(guard, dir[dirIndex % 4]);
            if (obstacles.Contains(next))
            {
                var hit = (guard, dirIndex % 4);
                if (obstacleHit.Contains(hit)) //Loop!
                {
                    loopTally++;
                    break;
                }
                obstacleHit.Add(hit);
                dirIndex++;
            }
            else
                guard = next;
        }
        obstacles.Remove(tempObstacle);
    }

    return loopTally;

}

Debug.Assert(41 == Calculate("....#.....\n.........#\n..........\n..#.......\n.......#..\n..........\n.#..^.....\n........#.\n#.........\n......#..."));
Console.WriteLine($"Guard exits after visiting {Calculate(PuzzleInput.Input)} tiles");

Debug.Assert(6 == Calculate("....#.....\n.........#\n..........\n..#.......\n.......#..\n..........\n.#..^.....\n........#.\n#.........\n......#...",false));
Console.WriteLine($"Guard can be looped in {Calculate(PuzzleInput.Input,false)} distinct places");

Console.WriteLine($"Done!");
