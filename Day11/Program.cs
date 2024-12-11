using System.Diagnostics;
using Common;
using Day11;

long Calculate(string input, int blinks = 25)
{
    var stones = new NumArgsByLine<long>(input).Lines[0];

    for (int blink = 0; blink < blinks; blink++)
    {
        var toSplit = new List<long>();
        for (int iStone = 0; iStone < stones.Count; iStone++)
        {
            if (stones[iStone] == 0)
                stones[iStone] = 1;
            else if ((int)Math.Log10(stones[iStone]) % 2 == 1)
            {
                var str = stones[iStone].ToString();
                var half = str.Length >> 1;
                var left = long.Parse(str.Substring(0, half));
                var right = long.Parse(str.Substring(half, half));
                stones[iStone] = left;
                stones.Insert(++iStone,right);
            }
            else
                stones[iStone] *= 2024;
        }

        if (stones.Count > int.MaxValue >> 2)
            throw new ArgumentOutOfRangeException();
    }

    return stones.Count;

}

string testInput = "125 17";
Debug.Assert(55312 == Calculate(testInput));
Console.WriteLine($"There are {Calculate(PuzzleInput.Input)} stones after 25 blinks");

//Console.WriteLine($"There are {Calculate(PuzzleInput.Input,75)} stones after 75 blinks");

Console.WriteLine($"Done!");