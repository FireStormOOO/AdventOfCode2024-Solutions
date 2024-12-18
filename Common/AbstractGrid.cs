using System.Collections;
using System.Diagnostics;

namespace Common;

public abstract class AbstractGrid<T> : IEnumerable<(int X, int Y)>
{
    public T[][] Grid { get; protected init; }
    public int Width { get; protected init; }
    public int Height { get; protected init; }
    public bool BoundsCheck((int X, int Y) coord) => coord is { X: >= 0, Y: >= 0} && coord.X < Width && coord.Y < Height;

    public AbstractGrid(string input)
    {
        Width = input.IndexOf('\n');
        Height = input.Count(c => c == '\n') + 1;
        Debug.Assert(Grid != null);
    }

    public AbstractGrid(T[][] grid)
    {
        Grid = grid;
        Height = grid.Length;
        Width = grid[0].Length;
        Debug.Assert(grid.All(row=>row.Length == Width));
    }
    public T Index((int X, int Y) index) => Grid[index.Y][index.X];
    /// <summary>
    /// Returns a single line/row of the grid.  Note that this means the indexes are reversed, (line,column) or (Y,X)
    /// </summary>
    /// <param name="row"></param>
    public T[] this[int row] => Grid[row];

    public void Index((int X, int Y) index, T toSet) => Grid[index.Y][index.X] = toSet;
    
    public (int X, int Y) FindFirst(T toFind)
    {
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if (toFind != null && toFind.Equals(Grid[j][i]))
                    return (i, j);
            }
        }

        throw new InvalidOperationException($"Character {toFind} was not present in the grid");
    }
    public bool TryFindFirst(T toFind, out (int X, int Y)? match)
    {
        match = null;
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if (Grid[j][i]!.Equals(toFind))
                {
                    match = (i, j);
                    break;
                }
            }
        }
        return match != null;
    }
    
    public IEnumerator<(int X, int Y)> GetEnumerator() => new AbstractGridEnumerator(Width, Height);
    IEnumerator IEnumerable.GetEnumerator() => new AbstractGridEnumerator(Width, Height);
}

class AbstractGridEnumerator(int width, int height) : IEnumerator<(int X, int Y)>
{
    private readonly int _max = width * height;
    private int _position = -1;
    private (int X, int Y) _current;

    public bool MoveNext()
    {
        _position++;
        _current = (_position % width, _position / width);
        return _position < _max;
    }

    public void Reset() => _position = -1;

    (int X, int Y) IEnumerator<(int X, int Y)>.Current => _current;
    public object Current => _current;
    public void Dispose() { }
}