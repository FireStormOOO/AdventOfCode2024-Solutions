using System.Diagnostics;
using System.Text.RegularExpressions;
using Day19;

//Some people, when confronted with a problem, think "I know, I'll use regular expressions." Now they have two problems."
long Calculate(string input)
{
    var regexInner = string.Join(null, input.Split('\n')[0].Split(", ").Select(s => $"(?:{s})?"));
    var matches = Regex.Matches(input, $"^({regexInner})+$", RegexOptions.Multiline | RegexOptions.Compiled);
    return matches.Count - 1;
}

string testInput = "r, wr, b, g, bwu, rb, gb, br\n\nbrwrr\nbggr\ngbbr\nrrbgbr\nubwu\nbwurrg\nbrgr\nbbrgwb";
Debug.Assert(6 == Calculate(testInput));

Console.WriteLine($"Achievable designs: {Calculate(PuzzleInput.Input)}");

Console.WriteLine($"Done!");