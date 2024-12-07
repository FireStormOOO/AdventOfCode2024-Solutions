using System.Diagnostics;
using System.Text.RegularExpressions;
using Day7;

long Add(long a, long b) => a + b;
long Mul(long a, long b) => a * b;
long Concat(long a, long b) => (long)(a * Math.Pow(10, (long)Math.Log10(b) + 1L)) + b;
long Calculate(string input, bool part1=true)
{
    var ops = new List<Func<long,long,long>>{Add, Mul};
    if(!part1)
        ops.Add(Concat);
    var matches = Regex.Matches(input, @"^(\d+): (\d+ ?){2,}$", RegexOptions.Multiline);
    var tally = 0L;
    foreach (Match match in matches)
    {
        var argMatch = Regex.Matches(match.Value, @"(\d+)");
        var res = long.Parse(argMatch[0].Value);
        var args = new List<long>();
        for(int i = 1; i < argMatch.Count; i++) args.Add(long.Parse(argMatch[i].Value));
        
        
        for (int i = 0; i < Math.Pow(ops.Count, args.Count - 1); i++)
        {
            Func<long, long, long> GetOp(int iOp) => ops[(int)(i / Math.Pow(ops.Count, iOp)) % ops.Count];
            long running = GetOp(0)(args[0], args[1]);
            for (int iOp = 1; iOp < args.Count -1; iOp++) 
                running = GetOp(iOp)(running, args[iOp + 1]);
            if (res==running)
            {
                tally += running;
                break;
            }
        }
    }

    return tally;
}

Debug.Assert(3749 == Calculate("190: 10 19\n3267: 81 40 27\n83: 17 5\n156: 15 6\n7290: 6 8 6 15\n161011: 16 10 13\n192: 17 8 14\n21037: 9 7 18 13\n292: 11 6 16 20"));
Console.WriteLine($"Calibration sum is {Calculate(PuzzleInput.Input)}");

Debug.Assert(11387 == Calculate("190: 10 19\n3267: 81 40 27\n83: 17 5\n156: 15 6\n7290: 6 8 6 15\n161011: 16 10 13\n192: 17 8 14\n21037: 9 7 18 13\n292: 11 6 16 20",false));
Console.WriteLine($"Corrected calibration sum is  {Calculate(PuzzleInput.Input,false)}");

Console.WriteLine($"Done!");
