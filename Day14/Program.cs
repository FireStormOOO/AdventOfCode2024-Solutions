using System.Diagnostics;
using Common;
using Day14;
using static Common.TupleMath<int>;

long Calculate(string input, (int width, int height) dims, bool part1=true)
{
    List<((int X, int Y) R, (int X, int Y) V)> robots = new NumArgsByLine<int>(input).Lines
        .Select(argList=>((argList[0],argList[1]),(argList[2],argList[3]))).ToList();

    int steps = 100;
    List<(int X, int Y)> robotsFinal = robots
        .Select(robot => Add(robot.R, ScalarMult(steps, Add(robot.V, dims))))
        .Select(robot => (robot.X % dims.width, robot.Y % dims.height)).ToList();
    var safteyFactor = robotsFinal.Count(robot => robot.X < dims.width >> 1 && robot.Y < dims.height >> 1) *
                       robotsFinal.Count(robot => robot.X < dims.width >> 1 && robot.Y > dims.height >> 1) *
                       robotsFinal.Count(robot => robot.X > dims.width >> 1 && robot.Y < dims.height >> 1) *
                       robotsFinal.Count(robot => robot.X > dims.width >> 1 && robot.Y > dims.height >> 1);
    return safteyFactor;
}

string testInput = "p=0,4 v=3,-3\np=6,3 v=-1,-3\np=10,3 v=-1,2\np=2,0 v=2,-1\np=0,0 v=1,3\np=3,0 v=-2,-2\np=7,6 v=-1,-3\np=3,0 v=-1,-2\np=9,3 v=2,3\np=7,3 v=-1,2\np=2,4 v=2,-3\np=9,5 v=-3,-3";
Debug.Assert(12 == Calculate(testInput, (11,7)));
Console.WriteLine($"Robot safety factor is {Calculate(PuzzleInput.Input, (101,103))} after 100 steps.");

//Console.WriteLine($"{Calculate(PuzzleInput.Input, false)}");

Console.WriteLine($"Done!");