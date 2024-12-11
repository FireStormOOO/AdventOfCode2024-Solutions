using System.Diagnostics;
using Common;
using Day11;

long Calculate(string input, int blinks = 25)
{
    var stones = new NumArgsByLine<long>(input).Lines[0];

    for (int blink = 0; blink < blinks; blink++)
    {
        var toAppend = new List<long>();
        for (int iStone = 0; iStone < stones.Count; iStone++)
        {
            var digits = (int)Math.Log10(stones[iStone]) + 1;
            if (stones[iStone] == 0)
                stones[iStone] = 1;
            else if (digits % 2 == 0)
            {
                var split = (int)Math.Pow(10, (digits >> 1));
                var left = stones[iStone] / split;
                var right = stones[iStone] % split;
                stones[iStone] = left;
                toAppend.Add(right);
            }
            else
                stones[iStone] *= 2024;
        }
        stones.AddRange(toAppend);

        if (stones.Count > int.MaxValue >> 2)
            throw new ArgumentOutOfRangeException();
    }

    return stones.Count;

}

string testInput = "125 17";
Debug.Assert(55312 == Calculate(testInput));
Console.WriteLine($"There are {Calculate(PuzzleInput.Input)} stones after 25 blinks");

Console.WriteLine($"There are {Calculate(PuzzleInput.Input,75)} stones after 75 blinks");

Console.WriteLine($"Done!");