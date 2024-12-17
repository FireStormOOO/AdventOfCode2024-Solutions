using System.Diagnostics;
using System.Text.RegularExpressions;
using Day17;


string Calculate(string input, bool part1=true)
{
    var match = Regex.Match(input, @"Register \w: (\d+)\nRegister \w: (\d+)\nRegister \w: (\d+)\n\nProgram: ((?:\d+,?)+)");
    int[] prog = match.Groups[4].Value.Split(',').Select(int.Parse).ToArray();
    
    var aOverride = -1;
    List<int> outputs = new();
    
    do
    {
        int[] reg = match.Groups.Values.ToList().Take(1..4).Select(g => int.Parse(g.Value)).ToArray();
        int counter = 0;
        outputs.Clear();
        if (aOverride >= 0)
            reg[0] = aOverride++;
        else
            aOverride++;
        bool run = true;
        
        while (counter + 1 < prog.Length && run)
        {
            var op = (Operand)prog[counter++];
            var combo = (prog[counter] & 4) == 4 ? reg[prog[counter++] & 3] : prog[counter++];
            switch (op)
            {
                case Operand.Adv:
                    reg[0] >>= combo;
                    break;
                case Operand.Bxl:
                    reg[1] ^= prog[counter - 1];
                    break;
                case Operand.Bst:
                    reg[1] = combo % 8;
                    break;
                case Operand.Jnz:
                    if (reg[0] != 0) counter = prog[--counter];
                    break;
                case Operand.Bxc:
                    reg[1] ^= reg[2];
                    break;
                case Operand.Out:
                    outputs.Add(combo % 8);
                    if (!part1 && outputs.Last() != prog[outputs.Count - 1])
                        run = false;
                    break;
                case Operand.Bdv:
                    reg[1] = reg[0] >> combo;
                    break;
                case Operand.Cdv:
                    reg[2] = reg[0] >> combo;
                    break;
            }

            if (!part1 && outputs.Count > prog.Length)
                break;
        }
        if(part1)
            return string.Join(',', outputs.Select(i => i.ToString()));
        
    } while (!outputs.SequenceEqual(prog));

    return aOverride.ToString();
}



string testInput = "Register A: 729\nRegister B: 0\nRegister C: 0\n\nProgram: 0,1,5,4,3,0";

Debug.Assert("4,6,3,5,6,3,5,2,1,0" == Calculate(testInput));

Console.WriteLine($"Program outputs: {Calculate(PuzzleInput.Input)}");

Debug.Assert("117440" == Calculate(testInput, false));

Console.WriteLine($"Correct A register value is {Calculate(PuzzleInput.Input, false)}");

Console.WriteLine($"Done!");


enum Operand
{
    Adv=0,
    Bxl=1,
    Bst=2,
    Jnz=3,
    Bxc=4,
    Out=5,
    Bdv=6,
    Cdv=7
}