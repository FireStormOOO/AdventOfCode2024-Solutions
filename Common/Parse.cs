using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;

namespace Common;


public class CharGrid
{
    public static char[][] ParseToGrid(string input)=>input.Split('\n').Select(s=>s.ToCharArray()).ToArray();
    public char[][] Grid { get; }
    public int Width { get; }
    public int Height { get; }
    
    public CharGrid(string input)
    {
        Grid = ParseToGrid(input);
        Width = input.IndexOf('\n');
        Height = input.Count(c => c == '\n') + 1;
    }
}

public class NumArgsByLine<T> where T : INumber<T>,IParsable<T>
{
    public readonly List<List<T>> Lines;
    public NumArgsByLine(string input)
    {
        Lines = input.Split('\n')
            .Select(s => 
                Regex.Matches(s, @"(\d+)")
                    .Select(match => 
                        T.Parse(match.Value, NumberFormatInfo.InvariantInfo)).ToList()).ToList();
    }
    public static implicit operator List<List<T>> (NumArgsByLine<T> numArgsByLine)=>numArgsByLine.Lines;
}

public class ArgsByLine
{
    private readonly List<List<string>> _lines;
    /// <summary>
    /// Split an input string on newline and further split lines by Regex
    /// </summary>
    /// <param name="input">The input string</param>
    /// <param name="argRegex">Your Regex to split the string; all matches will be returned</param>
    public ArgsByLine(string input,string argRegex)
    {
        _lines = input.Split('\n')
            .Select(line => 
                Regex.Matches(line, argRegex)
                    .Select(match=>match.Value).ToList()).ToList();
    }
    public static implicit operator List<List<string>> (ArgsByLine argsByLine)=>argsByLine._lines;
}
public class ArgsByLine<T>
{
    private readonly List<List<T>> _lines;
    /// <summary>
    /// Split an input string on newline and further split lines by Regex
    /// </summary>
    /// <param name="input">The input string</param>
    /// <param name="argRegex">Your Regex to split the string; all matches will be returned</param>
    public ArgsByLine(string input,string argRegex, Func<string,T> parse)
    {
        _lines = input.Split('\n')
            .Select(line => 
                Regex.Matches(line, argRegex)
                    .Select(match=>parse(match.Value)).ToList()).ToList();
    }
    public static implicit operator List<List<T>> (ArgsByLine<T> argsByLine)=>argsByLine._lines;
}