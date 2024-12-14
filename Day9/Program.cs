using System.Diagnostics;
using Day9;

long Calculate(string input, bool part1=true)
{
    var parseStart = DateTime.UtcNow;
    var compactedMap = input.ToCharArray().Select(c => int.Parse(c.ToString())).ToList();
    Debug.WriteLine($"Parsed data in {DateTime.UtcNow - parseStart}");
    var map = new int[compactedMap.Sum()];
    var files = new List<(int FileId, int Len, int SectorStart, int SectorEnd)>();
    int sector = 0;
    for (int i = 0; i < compactedMap.Count; i++)
    {
        var isFile = (i & 1) == 0;
        var fileId =isFile ? i >> 1 : -1;
        if(fileId!=-1) files.Add((fileId,compactedMap[i],sector,sector));
        for (int j = compactedMap[i]; j > 0; j--)
            map[sector++] = fileId;
    }
    files.Reverse();
    Debug.WriteLine($"Initialzed in {DateTime.UtcNow - parseStart}");

    if (part1)
    {
        int sourceSector = map.Length - 1;
        for (sector = 0; sector < sourceSector; sector++)
        {
            if (map[sector] != -1)
                continue;
            while (map[sourceSector] == -1) sourceSector--;
            map[sector] = map[sourceSector];
            map[sourceSector] = -1;
            sourceSector--;
        }
    }
    else
    {
        int[] searchFrom = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
        foreach (var file in files)
        {
            var searchStartSector = searchFrom[file.Len];
            if (searchStartSector == -1 || file.SectorStart < searchStartSector) continue;
            
            var targetSector = -1;
            for (int testSector = searchStartSector; testSector < file.SectorStart; testSector++)
            {
                if (map[testSector] != -1)
                {
                    targetSector = -1;
                    continue;
                }
                if (targetSector == -1) targetSector = testSector;
                if (testSector - targetSector + 1 >= file.Len) break;
            }
            if (targetSector + file.Len > file.SectorStart) targetSector = -1;
            
            for (int i = file.Len; i < 10; i++) 
                searchFrom[i] = targetSector == -1 ? -1 : int.Max(targetSector+file.Len, searchFrom[i]);
            
            if (targetSector == -1) continue;

            for (int i = 0; i < file.Len; i++)
            {
                map[targetSector + i] = file.FileId;
                map[file.SectorStart + i] = -1;
            }
        }
    }
    
    Debug.WriteLine($"Started finalizing at {DateTime.UtcNow - parseStart}");
    var tally = 0L;
    for (sector = 0; sector < map.Length; sector++)
        if(map[sector]!=-1)
            tally += (long)sector * map[sector];

    return tally;
}

string testInput = "2333133121414131402";
Debug.Assert(1928 == Calculate(testInput));
Console.WriteLine($"Checksum after fragmenting compaction is {Calculate(PuzzleInput.Input)}");

Debug.Assert(2858 == Calculate(testInput,false));
Console.WriteLine($"Checksum after smarter compaction is {Calculate(PuzzleInput.Input,false)}");

Console.WriteLine($"Done!");

Console.WriteLine("Upping the Ante 1:");
{
    var uta = PuzzleInput.UpTheAnte1;
    uta.Start();
    Debug.Assert(uta.Check(Calculate(uta.Input, false)));
    Debug.Assert(uta.Finish());
}
Console.WriteLine("Upping the Ante 1: SUCCESS");

Console.WriteLine("Upping the Ante 2:");
{
    var uta = PuzzleInput.UpTheAnte2;
    uta.Start();
    Debug.Assert(uta.Check(Calculate(uta.Input, false)));
    Debug.Assert(uta.Finish());
}
Console.WriteLine("Upping the Ante 2: SUCCESS");