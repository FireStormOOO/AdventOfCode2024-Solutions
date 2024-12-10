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
    public bool BoundsCheck((int X, int Y) coord) => coord is { X: >= 0, Y: >= 0} && coord.X < Width && coord.Y < Height;
    //public Func<(int X, int Y),bool> BoundsCheck =>
    
    public CharGrid(string input)
    {
        Grid = ParseToGrid(input);
        Width = input.IndexOf('\n');
        Height = input.Count(c => c == '\n') + 1;
    }

    public char[] this[int x] => Grid[x];
}

public class IntGrid
{
    public int[][] Grid { get; }
    public int Width { get; }
    public int Height { get; }
    public bool BoundsCheck((int X, int Y) coord) => coord is { X: >= 0, Y: >= 0} && coord.X < Width && coord.Y < Height;
    //public Func<(int X, int Y),bool> BoundsCheck =>
    
    public IntGrid(string input)
    {
        var charGrid = new CharGrid(input);
        Width = charGrid.Width;
        Height = charGrid.Height;
        Grid = charGrid.Grid.Select(row => row.Select(c => int.Parse(c.ToString())).ToArray()).ToArray();
    }

    public int Index((int X, int Y) index) => Grid[index.X][index.Y];
    public int[] this[int x] => Grid[x];
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