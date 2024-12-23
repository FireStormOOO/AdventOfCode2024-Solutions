﻿using System.Diagnostics;
using System.Text.RegularExpressions;
using Common;
using Day15;
using static Common.TupleMath<int>;

long Calculate(string input, bool part1=true)
{
    var inputSplit = Regex.Match(input,@"((?:.+\n?)+)\n\n((?:.+\n?)+)", RegexOptions.Multiline);
    var map = new CharGrid(inputSplit.Groups[1].Value);
    Dictionary<char, (int X, int Y)> moveOffsets = new()
        { { '<', (-1, 0) }, { '>', (1, 0) }, { '^', (0, -1) }, { 'v', (0, 1) } };
    var moves = inputSplit.Groups[2].Value.Where(c=>!"\n".Contains(c)).Select(c => moveOffsets[c]).ToList();

    bool CanPush((int X, int Y) moveDir, (int X, int Y) endPos, char movingPiece)
    {
        switch (map.Index(endPos))
        {
            case '#':
                return false;
            case 'O':
                return CanPush(moveDir, Add(endPos, moveDir), 'O');
            case '.':
                map.Index(endPos, movingPiece);
                return true;
            case '@':
                throw new InvalidOperationException("Should only be one robot!");
            default:
                throw new InvalidOperationException("Unknown piece");
        }
    }

    (int X, int Y) roboPos = (-1, -1);
    for (int i = 0; i < map.Width; i++)
        for (int j = 0; j < map.Height; j++)
                if (map.Grid[j][i]=='@') roboPos = (i, j);
    Debug.Assert(roboPos != (-1, -1));

    foreach (var move in moves)
    {
        if (part1)
        {
            var endPos = Add(move, roboPos);
            if (CanPush(move, endPos, '@'))
            {
                map.Index(roboPos, '.');
                map.Index(endPos, '@');
                roboPos = endPos;
            }
        }
        else
        {
            List<((int X, int Y) pushFrom, char tile)> toPush = [(roboPos, '@')];
            List<(int X, int Y)> toCheck = [Add(roboPos, move)];
            var vertical = move.Y != 0;
            var canPush = true;
            while (toCheck.Count > 0 && canPush)
            {
                var checking = toCheck[0];
                toCheck.RemoveAt(0);
                var here = map.Index(checking);
                switch (here)
                {
                    case '#':
                        canPush = false;
                        break;
                    case '[':                    
                    case ']':
                        if (vertical)
                        {
                            var twin = Add(checking, here == '[' ?  (1, 0) : (-1, 0));
                            if (toPush.Select(t=>t.pushFrom).Union(toCheck).All(xy=>xy!=twin)) 
                                toCheck.Add(twin);
                        }
                        toCheck.Add(Add(checking, move));
                        toPush.Add((checking, here));
                        break;
                    case '.':
                        break;
                    case '@':
                        throw new InvalidOperationException("Should only be one robot!");
                    default:
                        throw new InvalidOperationException("Unknown piece");
                }
                Debug.Assert(toPush.Count < 100 && toCheck.Count < 100);
            }

            if (canPush)
            {
                foreach (var update in toPush) map.Index(update.pushFrom, '.');
                foreach (var update in toPush) map.Index(Add(move, update.pushFrom), update.tile);
                roboPos = Add(roboPos, move);
            }
        }
    }

    var tally = 0L;
    for (int i = 0; i < map.Width; i++) for (int j = 0; j < map.Height; j++)
    {
        if (map.Grid[j][i] == 'O') tally += 100 * j + i;
        if (map.Grid[j][i] == '[') tally += 100 * j + i;
    }

    return tally;
}

string testInput = "########\n#..O.O.#\n##@.O..#\n#...O..#\n#.#.O..#\n#...O..#\n#......#\n########\n\n<^^>>>vv<v>>v<<";
string testInput2 = "##########\n#..O..O.O#\n#......O.#\n#.OO..O.O#\n#..O@..O.#\n#O#..O...#\n#O..O..O.#\n#.OO.O.OO#\n#....O...#\n##########\n\n<vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^\nvvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v\n><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<\n<<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^\n^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><\n^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^\n>^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^\n<><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>\n^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>\nv^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^";
Debug.Assert(2028 == Calculate(testInput));
Debug.Assert(10092 == Calculate(testInput2));

Console.WriteLine($"Final position of the boxes sums to {Calculate(PuzzleInput.Input)}");

string ExpandInput(string input) => input.Replace("#", "##").Replace("O", "[]").Replace(".", "..").Replace("@", "@.");
Debug.Assert(9021==Calculate(ExpandInput(testInput2), false));
Console.WriteLine($"Final position of the boxes sums to {Calculate(ExpandInput(PuzzleInput.Input), false)}");

Console.WriteLine($"Done!");