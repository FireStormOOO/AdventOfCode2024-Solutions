using System.Text;

namespace Common;

public class CharGrid : AbstractGrid<char>
{
    public static char[][] ParseToGrid(string input)=>input.Split('\n').Select(s=>s.ToCharArray()).ToArray();
    
    public CharGrid(string input) : base(input)
    {
        Grid = ParseToGrid(input);
    }

    public CharGrid(char[][] grid) : base(grid) { }

    public string Print()
    {
        var sb = new StringBuilder();
        foreach (var row in Grid)
        {
            foreach (var c in row) sb.Append(c);
            sb.AppendLine();
        }

        return sb.ToString();
    }
}