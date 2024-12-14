using System.Diagnostics;
using Common;
using Day14;
using static Common.TupleMath<int>;

long Calculate(string input, (int width, int height) dims, int steps = 100, bool draw=false)
{
    List<((int X, int Y) R, (int X, int Y) V)> robots = new NumArgsByLine<int>(input).Lines
        .Select(argList=>((argList[0],argList[1]),(argList[2],argList[3]))).ToList();

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

Console.ReadKey();
List<((int X, int Y) R, (int X, int Y) V)> robots = new NumArgsByLine<int>(PuzzleInput.Input).Lines
    .Select(argList=>((argList[0],argList[1]),(argList[2],argList[3]))).ToList();
(int width, int height) dims = (101, 103);
for (int step = 0;; step++)
{
    List<(int X, int Y)> robotsStep = robots
        .Select(robot => Add(robot.R, ScalarMult(step, Add(robot.V, dims))))
        .Select(robot => (robot.X % dims.width, robot.Y % dims.height)).ToList();
    
    
    //if(robotsStep.Distinct().Count() != 500) continue;
    var largestColumn = robotsStep.Select(robot => robot.Y).Distinct()
        .Select(y => robotsStep.Count(robot => robot.Y == y)).Max();
    var largestRow = robotsStep.Select(robot => robot.X).Distinct()
        .Select(x => robotsStep.Count(robot => robot.X == x)).Max();
    if (largestColumn < 25 || largestRow < 25) continue;
    
    Console.Clear();
    Console.WriteLine($"Step {step}:");
    for (int i = 0; i < dims.width; i++)
    {
        for (int j = 0; j < dims.height; j++) 
            Console.Write(robotsStep.Count(robot=>robot.X == i && robot.Y == j) > 0 ? 'X' : '_');
        Console.WriteLine();
    }

    Console.ReadLine();
}