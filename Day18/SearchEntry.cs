using static Common.TupleMath<int>;

namespace Day18;

record SearchEntry
{
    public long Hint;

    public static readonly Dictionary<Directions, (int X, int Y)> Offsets = new() { 
        { Directions.East, (1, 0) }, { Directions.North, (0, -1)},
        { Directions.West, (-1, 0)}, { Directions.South, (0, 1)}};
    public SearchEntry(Directions Facing, (int X, int Y) Pos, long Cost, Func<SearchEntry, long> FHint, SearchEntry? Previous)
    {
        this.Facing = Facing;
        this.Pos = Pos;
        this.Cost = Cost;
        this.FHint = FHint;
        Hint = FHint(this);
        this.Previous = Previous;
    }

    public SearchEntry LeftSuccessor()
    {
        var newFacing = (Directions)(((int)Facing + 1) % 4);
        var newPos = Add(Pos, Offsets[newFacing]);
        return new SearchEntry(newFacing, newPos, Cost + 1, FHint, this);
    }

    public SearchEntry RightSuccessor()
    {
        var newFacing = (Directions)(((int)Facing + 3) % 4);
        var newPos = Add(Pos, Offsets[newFacing]);
        return new SearchEntry(newFacing, newPos, Cost + 1, FHint, this);
    }

    public SearchEntry FwdSuccessor()
    {
        var newPos = Add(Pos, Offsets[Facing]);
        return new SearchEntry(Facing, newPos, Cost + 1, FHint, this);
    }

    public Directions Facing { get; init; }
    public (int X, int Y) Pos { get; init; }
    public long Cost { get; init; }
    public Func<SearchEntry, long> FHint { get; init; }
    public SearchEntry? Previous { get; init; }
    (Directions Facing, (int X, int Y) Pos) GetVisited() => (Facing, Pos);

    public List<(int X, int Y)> GetFullPath()
    {
        if (Previous == null)
            return [Pos];
        var path = Previous.GetFullPath();
        path.Add(Pos);
        return path;
    }

    // ReSharper disable ParameterHidesMember
    // ReSharper disable InconsistentNaming
    public void Deconstruct(out Directions Facing, out (int X, int Y) Pos, out long Cost, out Func<SearchEntry, long> FHint, out SearchEntry? Previous)
    {
        Facing = this.Facing;
        Pos = this.Pos;
        Cost = this.Cost;
        FHint = this.FHint;
        Previous = this.Previous;
    }
}