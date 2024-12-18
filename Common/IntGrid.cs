namespace Common;

public class IntGrid : AbstractGrid<int>
{
    public IntGrid(string input) : base(input)
    {
        var charGrid = new CharGrid(input);
        Grid = charGrid.Grid.Select(row => row.Select(c => int.Parse(c.ToString())).ToArray()).ToArray();
    }
    public IntGrid(int[][] grid) : base(grid)
    { }
}