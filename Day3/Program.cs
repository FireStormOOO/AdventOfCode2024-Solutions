using System.Text.RegularExpressions;
using static Day3.PuzzleInput;

var matches = Regex.Matches(Input, @"mul\((\d{1,3}),(\d{1,3})\)|do\(\)|don't\(\)");
bool enable = true;
int tally = 0;
foreach (Match match in matches)
{
    if (match.Value.StartsWith("mul(") && enable)
        tally += int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value);
    else if(match.Value.StartsWith("do(")) enable = true;
    else if (match.Value.StartsWith("don't(")) enable = false;
}

Console.WriteLine($"Sum of match evals: {tally}");