using System.Diagnostics;
using static Day4.PuzzleInput;


int Tally(string input)
{
    int tally = 0;
    int width = input.IndexOf('\n') + 1;
    int height = input.Count(c=>c=='\n') + 1;
    for (int i = 0; i < input.Length; i++)
    {
        int I(int x, int y) => i + x + y * width;
        char C(int x, int y)
        {
            var index = I(x, y);
            //return dummy character for OOB reads, otherwise just index
            return index < input.Length ? input[index] : ' ';
        }

        var horizontal = $"{C(0, 0)}{C(1, 0)}{C(2, 0)}{C(3, 0)}";
        var vertical = $"{C(0, 0)}{C(0, 1)}{C(0, 2)}{C(0, 3)}";
        var diagA = $"{C(0, 0)}{C(1, 1)}{C(2, 2)}{C(3, 3)}";
        var diagB = $"{C(3, 0)}{C(2, 1)}{C(1, 2)}{C(0, 3)}";
        foreach (var str in new[] { horizontal, vertical, diagA, diagB })
        {
            if (str == "XMAS") tally++;
            if (str == "SAMX") tally++;
        }
    }

    return tally;
}

Debug.Assert(Tally("..X...\n.SAMX.\n.A..A.\nXMAS.S\n.X....") == 4);
Debug.Assert(Tally("MMMSXXMASM\nMSAMXMSMSA\nAMXSXMAAMM\nMSAMASMSMX\nXMASAMXAMM\nXXAMMXXAMA\nSMSMSASXSS\nSAXAMASAAA\nMAMMMXMMMM\nMXMXAXMASX") == 18);
Console.WriteLine($"Input text has {Tally(Input)} hits for XMAS");

int Tally2(string input)
{
    int tally = 0;
    int width = input.IndexOf('\n') + 1;
    int height = input.Count(c=>c=='\n') + 1;
    for (int i = 0; i < input.Length; i++)
    {
        int I(int x, int y) => i + x + y * width;
        char C(int x, int y)
        {
            var index = I(x, y);
            //return dummy character for OOB reads, otherwise just index
            return index < input.Length ? input[index] : ' ';
        }

        var diagA = $"{C(0, 0)}{C(1, 1)}{C(2, 2)}";
        var diagB = $"{C(2, 0)}{C(1, 1)}{C(0, 2)}";

        if ((diagA == "MAS" || diagA == "SAM") && (diagB == "MAS" || diagB == "SAM"))
            tally++;
    }

    return tally;
}

Debug.Assert(Tally2("M.S\n.A.\nM.S")==1);
Debug.Assert(Tally2(".M.S......\n..A..MSMS.\n.M.S.MAA..\n..A.ASMSM.\n.M.S.M....\n..........\nS.S.S.S.S.\n.A.A.A.A..\nM.M.M.M.M.\n..........")==9);
Console.WriteLine($"Input text has {Tally2(Input)} hits for X-MAS");