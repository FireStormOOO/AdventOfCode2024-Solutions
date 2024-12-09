using System.Diagnostics;
using Day9;

long Calculate(string input, bool part1=true)
{
    var compactedMap = input.ToCharArray().Select(c => int.Parse(c.ToString())).ToList();
    var map = new int[compactedMap.Sum()];
    var files = new List<(int FileId, int Len, int Sector)>();
    int sector = 0;
    for (int i = 0; i < compactedMap.Count; i++)
    {
        var isFile = (i & 1) == 0;
        var fileId =isFile ? i >> 1 : -1;
        if(fileId!=-1) files.Add((fileId,compactedMap[i],sector));
        for (int j = compactedMap[i]; j > 0; j--)
            map[sector++] = fileId;
    }

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
        int FindSpace(int startSector, int checkLen, int neededLen)
        {
            var candidateSector = -1;
            var subMap = map.AsSpan(startSector, checkLen);
            for (int i = 0; i < checkLen; i++)
            {
                if (subMap[i] != -1)
                {
                    candidateSector = -1;
                    continue;
                }
                if (candidateSector == -1) candidateSector = i;
                if (i - candidateSector + 1 >= neededLen)
                    return candidateSector + startSector;
            }
            return -1;
        }
        foreach (var file in files.OrderByDescending(f=>f.FileId))
        {
            var targetSector = FindSpace(0, file.Sector, file.Len);
            if (targetSector == -1) continue;
            for (int i = 0; i < file.Len; i++)
            {
                map[targetSector + i] = file.FileId;
                map[file.Sector + i] = -1;
            }
        }
    }

    var tally = 0L;
    for (sector = 0; sector < map.Length; sector++) 
        tally += Math.Max(sector * map[sector],0);

    return tally;
}

string testInput = "2333133121414131402";
Debug.Assert(1928 == Calculate(testInput));
Console.WriteLine($"Checksum after fragmenting compaction is {Calculate(PuzzleInput.Input)}");

Debug.Assert(2858 == Calculate(testInput,false));
Console.WriteLine($"Checksum after smarter compaction is {Calculate(PuzzleInput.Input,false)}");

Console.WriteLine($"Done!");