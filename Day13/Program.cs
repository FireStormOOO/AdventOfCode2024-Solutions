using System.Diagnostics;
using System.Text.RegularExpressions;
using Common;
using Day13;
using static Common.TupleMath<int>;

long Calculate(string input, bool part1=true)
{
    var matches = Regex.Matches(input, @"(?:Button A: X([+-]\d+), Y([+-]\d+))\n(?:Button B: X([+-]\d+), Y([+-]\d+))\n(?:Prize: X=(\d+), Y=(\d+))", RegexOptions.Multiline);
    var games = matches.Select(match => match.Groups.Values.Take(1..7).Select(cap => int.Parse(cap.Value)).ToList())
        .Select(arg => new Game((arg[0], arg[1]), (arg[2], arg[3]), (arg[4], arg[5]))).ToList();

    int cost = 0, prizes = 0;
    foreach (var game in games)
    {
        var A = game.A;
        var B = game.B;
        var prize = game.Prize;
        if (A.X * B.Y == A.Y * B.X)
            throw new NotImplementedException("Assumed A and B are not colinear");
        int aCount = (prize.X * B.Y - prize.Y * B.X) / (A.X * B.Y - A.Y * B.X);
        int bCount = (prize.X * A.Y - prize.Y * A.X) / (B.X * A.Y - B.Y * A.X);
        if (Add(ScalarMult(aCount, A), ScalarMult(bCount, B)) == prize)
        {
            prizes++;
            cost += 3 * aCount + bCount;
        }

    }

    return cost;
}

string testInput = "Button A: X+94, Y+34\nButton B: X+22, Y+67\nPrize: X=8400, Y=5400\n\nButton A: X+26, Y+66\nButton B: X+67, Y+21\nPrize: X=12748, Y=12176\n\nButton A: X+17, Y+86\nButton B: X+84, Y+37\nPrize: X=7870, Y=6450\n\nButton A: X+69, Y+23\nButton B: X+27, Y+71\nPrize: X=18641, Y=10279";
Debug.Assert(480 == Calculate(testInput));
Console.WriteLine($"All possible prizes need {Calculate(PuzzleInput.Input)} tokens to collect.");

//Debug.Assert(80==Calculate(testInput, false));
//Console.WriteLine($"{Calculate(PuzzleInput.Input,false)}");

Console.WriteLine($"Done!");

record Game((int X, int Y) A, (int X, int Y) B, (int X, int Y) Prize);