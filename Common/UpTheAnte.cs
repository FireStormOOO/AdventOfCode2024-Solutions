namespace Common;

public record UpTheAnte(string Input, long Solution, TimeSpan TimeLimit)
{
    public DateTime StartTime;
    public TimeSpan TimeTaken;

    public void Start() => StartTime = DateTime.UtcNow;
    /// <summary>
    /// Register completion
    /// </summary>
    /// <returns>Did this finish within the time limit</returns>
    public bool Finish()
    {
        TimeTaken = DateTime.UtcNow - StartTime;
        return TimeTaken < TimeLimit;
    }

    public bool Check(long result) => result == Solution;
}