await using var stream = new FileStream("input.txt", FileMode.Open);
using var reader = new StreamReader(stream);
var lines = new List<List<int>>();
while (!reader.EndOfStream) 
    lines.Add(reader.ReadLine()!.Split(" ").Select(int.Parse).ToList());
int safeCount = 0;

bool CheckLineSafe(List<int> line, int deltaHigh, int deltaLow, int problemDampener = 1)
{
    bool safe = true;
    for (int i = 1; i < line.Count && safe; i++)
    {
        int delta = line[i] - line[i - 1];
        safe = delta >= deltaLow && delta <= deltaHigh;
        if (safe || problemDampener <= 0) continue;
        
        //handle problem dampener logic
        problemDampener--;
        if (i + 1 == line.Count)//we're already at the end and can drop the failing value
        {
            safe = true;
            continue;
        } 
          
        int delta2 = line[i + 1] - line[i - 1];
        if (delta2 >= deltaLow && delta2 <= deltaHigh)//can we skip i?
        {
            safe = true;
            i++;//if we skip i, jump forward twice
            continue;
        }

        //if we're at the beginning, check if we can drop the first value
        int delta3 = line[i + 1] - line[i];
        safe = i==1 && delta3 >= deltaLow && delta3 <= deltaHigh;
    }

    return safe;
}

foreach (var line in lines)
{
    safeCount += CheckLineSafe(line, -1, -3) ? 1 : 0;
    safeCount += CheckLineSafe(line, 3, 1) ? 1 : 0;
}
Console.WriteLine($"Safe line count: {safeCount}");