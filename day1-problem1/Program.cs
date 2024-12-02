var a = new List<int>();
var b = new List<int>();
await using var stream = new FileStream("input.txt", FileMode.Open);
using var reader = new StreamReader(stream);
while (!reader.EndOfStream)
{
    var line = reader.ReadLine()!.Split("   ");
    a.Add(int.Parse(line[0]));
    b.Add(int.Parse(line[1]));
}
a.Sort();
b.Sort();
var res = a.Zip(b)
    .Select(pair => Math.Abs(pair.First - pair.Second))
    .Aggregate((l, r) => l + r);
Console.WriteLine(res);