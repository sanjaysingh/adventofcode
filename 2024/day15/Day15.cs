namespace AdventOfCode;

public static class Day15
{
    private static readonly string InputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day15", "input.txt");
    private const char Robot = '@', Wall = '#', Empty = '.', Box = 'O';

    private static readonly Dictionary<char, Position> Directions = new()
    {
        ['^'] = new(-1, 0),
        ['v'] = new(1, 0),
        ['<'] = new(0, -1),
        ['>'] = new(0, 1)
    };

    public static void SolvePart1()
    {
        var (map, movements) = ReadInput();
        var robotPos = FindCharacter(map, Robot);

        foreach (var move in movements)
        {
            robotPos = map.Shift(robotPos, Directions[move]);
        }

        Console.WriteLine(CalculateScore(map, Box));
    }

    public static void SolvePart2()
    {
        var (map, movements) = ReadInput();
        map = map.Select(row => row.Select(c => c switch
        {
            Wall => "##",
            Box => "[]",
            Empty => "..",
            Robot => "@.",
            _ => throw new ArgumentException($"Invalid character: {c}")
        }).SelectMany(s => s).ToArray()).ToArray();

        var robotPos = FindCharacter(map, Robot);

        foreach (var move in movements)
        {
            var direction = Directions[move];
            if (map.CanShift(robotPos, direction))
                robotPos = map.Shift2(robotPos, direction);
        }

        Console.WriteLine(CalculateScore(map, '['));
    }

    private static Position Shift2(this char[][] map, Position pos, Position direction)
    {
        var nextPos = pos + direction;
        if (map.CharAt(nextPos) == Empty) return map.Swap(pos, nextPos);
        if (map.CharAt(nextPos) == Wall) return pos;

        var nextPositions = new List<Position> { nextPos };
        if (direction.IsUpOrDown)
        {
            if (map.CharAt(nextPos) == '[') nextPositions.Add(nextPos.Right);
            if (map.CharAt(nextPos) == ']') nextPositions.Add(nextPos.Left);
        }

        foreach (var next in nextPositions)
        {
            map.Shift2(next, direction);
        }

        return map.CharAt(nextPos) == Empty ? map.Swap(pos, nextPos) : nextPos;
    }

    private static bool CanShift(this char[][] map, Position pos, Position direction)
    {
        var nextPos = pos + direction;
        if (map.CharAt(nextPos) == Empty) return true;
        if (map.CharAt(nextPos) == Wall) return false;

        var nextPositions = new List<Position> { nextPos };
        if (direction.IsUpOrDown)
        {
            if (map.CharAt(nextPos) == '[') nextPositions.Add(nextPos.Right);
            if (map.CharAt(nextPos) == ']') nextPositions.Add(nextPos.Left);
        }

        return nextPositions.All(next => map.CanShift(next, direction));
    }

    private static Position Shift(this char[][] map, Position pos, Position direction)
    {
        var nextPos = pos + direction;
        if (map.CharAt(nextPos) == Empty) return map.Swap(pos, nextPos);
        if (map.CharAt(nextPos) == Box)
        {
            map.Shift(nextPos, direction);
            if (map.CharAt(nextPos) == Empty) return map.Swap(pos, nextPos);
        }
        return pos;
    }

    private static Position FindCharacter(char[][] map, char target) =>
        map.SelectMany((row, x) => row.Select((ch, y) => new { Ch = ch, Pos = new Position(x, y) }))
           .First(pos => pos.Ch == target).Pos;

    private static int CalculateScore(char[][] map, char target) =>
        map.SelectMany((row, x) => row.Select((ch, y) => new { Ch = ch, Pos = new Position(x, y) }))
           .Where(c => c.Ch == target)
           .Sum(c => 100 * c.Pos.X + c.Pos.Y);

    private static Position Swap(this char[][] map, Position pos1, Position pos2)
    {
        (map[pos1.X][pos1.Y], map[pos2.X][pos2.Y]) = (map[pos2.X][pos2.Y], map[pos1.X][pos1.Y]);
        return pos2;
    }

    private static char CharAt(this char[][] map, Position pos) => map[pos.X][pos.Y];

    private readonly record struct Position(int X, int Y)
    {
        public static Position operator +(Position a, Position b) => new(a.X + b.X, a.Y + b.Y);
        public bool IsUpOrDown => X is 1 or -1;
        public Position Right => this with { Y = Y + 1 };
        public Position Left => this with { Y = Y - 1 };
    }

    private static (char[][] Map, string Movements) ReadInput()
    {
        var lines = File.ReadAllLines(InputPath);
        var map = lines.TakeWhile(line => !string.IsNullOrWhiteSpace(line))
                      .Select(line => line.ToCharArray())
                      .ToArray();
        var movements = lines.SkipWhile(line => !string.IsNullOrWhiteSpace(line))
                           .Skip(1)
                           .Aggregate("", (acc, line) => acc + line.Trim());
        return (map, movements);
    }
}