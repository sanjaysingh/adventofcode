using System.Reflection;

namespace AdventOfCode;

public static class Day06
{
    private static readonly string InputPath = Path.Combine(
        Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ?? string.Empty,
        "day06/input.txt"
    );

    public static void SolvePart1()
    {
        var map = ReadInput();
        var (pos, dir) = FindGuard(map);

        while (pos?.IsWithinMap(map) == true)
        {
            map[pos.X][pos.Y] = 'X';
            var nextPos = pos.Move(dir!);

            if (!nextPos.IsWithinMap(map)) break;

            pos = map[nextPos.X][nextPos.Y] == '#'
                ? pos
                : nextPos;
            dir = map[nextPos.X][nextPos.Y] == '#'
                ? dir.TakeRight()
                : dir;
        }

        Console.WriteLine(map.Sum(row => row.Count(ch => ch == 'X')));
    }

    public static void SolvePart2()
    {
        var map = ReadInput();
        var (initialPos, initialDir) = FindGuard(map);

        var count = (
            from row in Enumerable.Range(0, map.Length)
            from col in Enumerable.Range(0, map.Length)
            where map[row][col] == '.'
            let currMap = map.Select(r => r.ToArray()).ToArray()
            where SimulatePath(currMap, row, col, initialPos!, initialDir!)
            select 1
        ).Count();

        Console.WriteLine(count);
    }

    private static bool SimulatePath(char[][] map, int row, int col, Position pos, Direction dir)
    {
        map[row][col] = '#';
        var visited = new HashSet<string>();
        var curPos = pos;
        var currDir = dir;

        while (curPos.IsWithinMap(map))
        {
            var visitKey = $"{curPos.X},{curPos.Y},{currDir.Symbol}";
            if (!visited.Add(visitKey)) return true;

            map[curPos.X][curPos.Y] = 'X';
            var nextPos = curPos.Move(currDir);

            if (!nextPos.IsWithinMap(map)) return false;

            if (map[nextPos.X][nextPos.Y] == '#')
            {
                currDir = currDir.TakeRight();
            }
            else
            {
                curPos = nextPos;
            }
        }

        return false;
    }

    private static (Position?, Direction?) FindGuard(char[][] map) =>
        (from row in Enumerable.Range(0, map.Length)
         from col in Enumerable.Range(0, map[row].Length)
         where Direction.IsDirectionSymbol(map[row][col])
         select (new Position(row, col), Direction.FromSymbol(map[row][col])))
        .FirstOrDefault((null, null));

    private record Position(int X, int Y)
    {
        public Position Move(Direction dir) => new(X + dir.XOffset, Y + dir.YOffset);
        public bool IsWithinMap(char[][] map) => X >= 0 && Y >= 0 && X < map.Length && Y < map[0].Length;
    }

    private record Direction(int XOffset, int YOffset, char Symbol, char rightSymbol)
    {
        private static readonly Dictionary<char, Direction> dirs = new()
        {
            {'^', new Direction(-1, 0, '^', '>') },
            {'<', new Direction(0, -1, '<', '^') },
            {'>', new Direction(0, 1, '>', 'V') },
            {'V', new Direction(1, 0, 'V','<') },
        };

        public static bool IsDirectionSymbol(char symbol) => dirs.ContainsKey(symbol);
        public static Direction FromSymbol(char symbol) => dirs[symbol];
        public Direction TakeRight() => dirs[rightSymbol];
    }

    private static char[][] ReadInput() => File.ReadAllLines(InputPath).Select(r => r.ToCharArray()).ToArray();
}