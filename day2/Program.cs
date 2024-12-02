await using var stream = new FileStream("input.txt", FileMode.Open);
using var reader = new StreamReader(stream);
var lines = new List<List<int>>();
while (!reader.EndOfStream) 
    lines.Add(reader.ReadLine()!.Split(" ").Select(int.Parse).ToList());
int safeCount = 0;
foreach (var line in lines)
{
    bool safe = true;
    int deltaLow = -3, deltaHigh = -1;
    if (line[0] < line[1])
    {
        deltaHigh = 3;
        deltaLow = 1;
    }
    for (int i = 1; i < line.Count && safe; i++)
    {
        int delta = line[i] - line[i - 1];
        safe = delta >= deltaLow && delta <= deltaHigh;
    }

    safeCount += safe ? 1 : 0;
}
Console.WriteLine($"Safe line count: {safeCount}");