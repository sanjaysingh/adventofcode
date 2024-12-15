namespace AdventOfCode;
using Map = char[][];
public static class Day15
{
    private static readonly string InputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day15", "input.txt");
    private const char ROBOT = '@';
    private const char WALL = '#';
    private const char EMPTY = '.';
    private const char BOX = 'O';
    private static readonly Dictionary<char, Position> charDirections = new Dictionary<char, Position>
    {
        { '^', new Position(-1, 0)},
        { 'v', new Position(1, 0)},
        { '<', new Position(0, -1)},
        { '>', new Position(0, 1)}
    };
    public static void SolvePart1()
    {
        var (map, movements) = ReadInput();
        var robotPosition = map.SelectMany((row, x) => row.Select((ch, y) => new { Ch = ch, Pos = new Position(x, y) }))
                                .First(pos => pos.Ch == '@').Pos;
        foreach (var ch in movements)
        {
            var direction = charDirections[ch];
            robotPosition = map.Shift(robotPosition, direction);
        }
        var sum = map
                   .SelectMany((row, x) => row.Select((ch, y) => new { Ch = ch, Pos = new Position(x, y) }))
                   .Where(c => c.Ch == BOX)
                   .Select(c => (100 * c.Pos.X) + c.Pos.Y)
                   .Sum();
        Console.WriteLine(sum);
    }
    public static void SolvePart2()
    {
        var (map, movements) = ReadInput();
        map = map.Select(row => new string(row).Replace("#", "##").Replace("O", "[]").Replace(".", "..").Replace("@", "@.").ToCharArray()).ToArray();
        var robotPosition = map.SelectMany((row, x) => row.Select((ch, y) => new { Ch = ch, Pos = new Position(x, y) }))
                                .First(pos => pos.Ch == '@').Pos;
    }

    private static Position Shift(this Map map, Position[] positions, Position direction)
    {
        var nextPositions = new List<Position>();
        nextPositions.AddRange(positions.Select(p => p.Next(direction)));
    }

    private static Position Shift(this Map map, Position currentPosition, Position direction)
    {
        var nextPos = currentPosition.Next(direction);
        if (map.CharAt(nextPos) == EMPTY) return map.Swap(currentPosition, nextPos);
        else if (map.CharAt(nextPos) == BOX)
        {
            Shift(map, nextPos, direction);
            if (map.CharAt(nextPos) == EMPTY) return map.Swap(currentPosition, nextPos);
        }
        return currentPosition;
    }
    private static Position Swap(this Map map, Position pos1, Position pos2)
    {
        var pos1Char = map.CharAt(pos1);
        map.Set(pos1, map.CharAt(pos2));
        map.Set(pos2, pos1Char);
        return pos2;
    }
    private static char CharAt(this Map map, Position pos) => map[pos.X][pos.Y];
    private static void Set(this Map map, Position pos, char ch) => map[pos.X][pos.Y] = ch;
    private record Position(int X, int Y)
    {
        public Position Next(Position offset) => new Position(X + offset.X, Y + offset.Y);
    }
    private static (Map, string) ReadInput()
    {
        var lines = File.ReadAllLines(InputPath);
        return (lines.TakeWhile(line => !string.IsNullOrWhiteSpace(line)).Select(line => line.ToCharArray()).ToArray(),
              lines.SkipWhile(line => !string.IsNullOrWhiteSpace(line)).Skip(1).Aggregate((l1, l2) => l1.Trim() + l2.Trim()));
    }
}