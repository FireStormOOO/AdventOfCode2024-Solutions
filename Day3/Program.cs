using System.Text.RegularExpressions;
using static Day3.PuzzleInput;

var matches = Regex.Matches(Input, @"mul\((\d{1,3}),(\d{1,3})\)");
var res = matches.Select(match => int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value)).Sum();
Console.WriteLine($"Sum of match evals: {res}");