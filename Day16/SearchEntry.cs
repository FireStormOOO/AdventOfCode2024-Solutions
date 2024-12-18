using Common;

namespace Day16;

record SearchEntry
{
    public long Hint;

    public static readonly Dictionary<Directions, (int X, int Y)> Offsets = new() { 
        { Directions.East, (1, 0) }, { Directions.North, (0, -1)},
        { Directions.West, (-1, 0)}, { Directions.South, (0, 1)}};
    public SearchEntry(Directions Facing, (int X, int Y) Pos, long Cost, bool JustTurned, Func<SearchEntry, long> FHint, SearchEntry? Previous)
    {
        this.Facing = Facing;
        this.Pos = Pos;
        this.Cost = Cost;
        this.JustTurned = JustTurned;
        this.FHint = FHint;
        Hint = FHint(this);
        this.Previous = Previous;
    }

    public SearchEntry LeftSuccessor() => new((Directions)(((int)Facing + 1) % 4), Pos, Cost + 1000, true, FHint, this);
    public SearchEntry RightSuccessor() => new((Directions)(((int)Facing + 3) % 4), Pos, Cost + 1000, true, FHint, this);
    public SearchEntry FwdSuccessor() => new(Facing, TupleMath<int>.Add(Pos, Offsets[Facing]), Cost + 1, false, FHint, this);
    public (Directions Facing, (int X, int Y) Pos) ToVisited() => (Facing, Pos);

    public Directions Facing { get; init; }
    public (int X, int Y) Pos { get; init; }
    public long Cost { get; init; }
    public bool JustTurned { get; init; }
    public Func<SearchEntry, long> FHint { get; init; }
    public SearchEntry? Previous { get; init; }
    (Directions Facing, (int X, int Y) Pos) GetVisited() => (Facing, Pos);

    public List<(Directions Facing, (int X, int Y) Pos)> GetFullPath()
    {
        if (Previous == null)
            return [(Facing, Pos)];
        var path = Previous.GetFullPath();
        path.Add((Facing, Pos));
        return path;
    }

    // ReSharper disable ParameterHidesMember
    // ReSharper disable InconsistentNaming
    public void Deconstruct(out Directions Facing, out (int X, int Y) Pos, out long Cost, out bool JustTurned, out Func<SearchEntry, long> FHint, out SearchEntry? Previous)
    {
        Facing = this.Facing;
        Pos = this.Pos;
        Cost = this.Cost;
        JustTurned = this.JustTurned;
        FHint = this.FHint;
        Previous = this.Previous;
    }
}