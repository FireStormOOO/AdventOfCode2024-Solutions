using System.Diagnostics;
using System.Text.RegularExpressions;
using Day17;


List<long> Calculate(string input, long? aOverride=null)
{
    var match = Regex.Match(input, @"Register \w: (\d+)\nRegister \w: (\d+)\nRegister \w: (\d+)\n\nProgram: ((?:\d+,?)+)");
    long[] prog = match.Groups[4].Value.Split(',').Select(long.Parse).ToArray();
    List<long> outputs = new();
    long[] reg = match.Groups.Values.ToList().Take(1..4).Select(g => long.Parse(g.Value)).ToArray();
    int counter = 0;
    if (aOverride != null)
        reg[0] = aOverride.Value;
    
    while (counter + 1 < prog.Length)
    {
        var op = (Operand)prog[counter++];
        var combo = (prog[counter] & 4) == 4 ? reg[(int)(prog[counter++] & 3)] : prog[counter++];
        switch (op)
        {
            case Operand.Adv:
                reg[0] >>= (int)combo;
                break;
            case Operand.Bxl:
                reg[1] ^= prog[counter - 1];
                break;
            case Operand.Bst:
                reg[1] = combo & 7;
                break;
            case Operand.Jnz:
                if (reg[0] != 0) counter = (int)prog[--counter];
                break;
            case Operand.Bxc:
                reg[1] ^= reg[2];
                break;
            case Operand.Out:
                outputs.Add(combo & 7L);
                break;
            case Operand.Bdv:
                reg[1] = reg[0] >> (int)combo;
                break;
            case Operand.Cdv:
                reg[2] = reg[0] >> (int)combo;
                break;
        }

        if (aOverride != null && outputs.Count > prog.Length)
            break;
    }

    return outputs;
}

long FindMatch(long[] prog)
{
    Dictionary<int, List<(long values, long bitmap)>> constraints = new();
    for (int i = 0; i < prog.Length; i++)
    {
        constraints[i] = new();
        for (long j = 0; j < 8; j++)
        {
            long b = j;
            int baseOffset = 3 * i;
            int cOffset = (int)((b ^ 1) + baseOffset);
            var c = (b ^ 4L ^ prog[i]) & 7L;
            b <<= baseOffset;
            c <<= cOffset;
            if ((b & (7L << cOffset)) != (c & (7L << baseOffset)))
                continue;
            Debug.Assert(b >= 0 && c >= 0);
            long bitmap = (7L << baseOffset) | (7L << cOffset);
            constraints[i].Add((b | c, bitmap));
        }
    }

    bool Compatible((long values, long bitmap) a, (long values, long bitmap) b) =>
        (a.values & b.bitmap) == (b.values & a.bitmap);

    (long values, long bitmap) Combine((long values, long bitmap) a, (long values, long bitmap) b) =>
        (a.values | b.values, a.bitmap | b.bitmap);

    var a = constraints[0]
        .SelectMany(a => constraints[1].Where(b => Compatible(a, b)).Select(b => Combine(a, b)))
        .SelectMany(a => constraints[2].Where(b => Compatible(a, b)).Select(b => Combine(a, b)))
        .SelectMany(a => constraints[3].Where(b => Compatible(a, b)).Select(b => Combine(a, b)))
        .SelectMany(a => constraints[4].Where(b => Compatible(a, b)).Select(b => Combine(a, b)))
        .SelectMany(a => constraints[5].Where(b => Compatible(a, b)).Select(b => Combine(a, b)))
        .SelectMany(a => constraints[6].Where(b => Compatible(a, b)).Select(b => Combine(a, b)))
        .SelectMany(a => constraints[7].Where(b => Compatible(a, b)).Select(b => Combine(a, b)))
        .SelectMany(a => constraints[8].Where(b => Compatible(a, b)).Select(b => Combine(a, b)))
        .SelectMany(a => constraints[9].Where(b => Compatible(a, b)).Select(b => Combine(a, b)))
        .SelectMany(a => constraints[10].Where(b => Compatible(a, b)).Select(b => Combine(a, b)))
        .SelectMany(a => constraints[11].Where(b => Compatible(a, b)).Select(b => Combine(a, b)))
        .SelectMany(a => constraints[12].Where(b => Compatible(a, b)).Select(b => Combine(a, b)))
        .SelectMany(a => constraints[13].Where(b => Compatible(a, b)).Select(b => Combine(a, b)))
        .SelectMany(a => constraints[14].Where(b => Compatible(a, b)).Select(b => Combine(a, b)))
        .SelectMany(a => constraints[15].Where(b => Compatible(a, b)).Select(b => Combine(a, b)))
        .First();
        //.First(aVal => Calculate(PuzzleInput.Input, aVal.values).SequenceEqual(prog));
    return a.values;
}


string testInput = "Register A: 729\nRegister B: 0\nRegister C: 0\n\nProgram: 0,1,5,4,3,0";
Debug.Assert("4,6,3,5,6,3,5,2,1,0" ==string.Join(',',Calculate(testInput)));

Console.WriteLine($"Program outputs: {string.Join(',',Calculate(PuzzleInput.Input))}");

string testInput2 = "Register A: 2024\nRegister B: 0\nRegister C: 0\n\nProgram: 0,3,5,4,3,0";
long[] testProg2 = Regex.Match(testInput2, @"Register \w: (\d+)\nRegister \w: (\d+)\nRegister \w: (\d+)\n\nProgram: ((?:\d+,?)+)")
    .Groups[4].Value.Split(',').Select(long.Parse).ToArray();
Debug.Assert(testProg2.SequenceEqual(Calculate(testInput2, 117440)));

var match = Regex.Match(PuzzleInput.Input, @"Register \w: (\d+)\nRegister \w: (\d+)\nRegister \w: (\d+)\n\nProgram: ((?:\d+,?)+)");
long[] prog = match.Groups[4].Value.Split(',').Select(long.Parse).ToArray();
long a = FindMatch(prog);
Debug.Assert(prog.SequenceEqual(Calculate(PuzzleInput.Input, a)));
Console.WriteLine($"Correct A register value is {a}");

Console.WriteLine($"Done!");