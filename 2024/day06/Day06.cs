using System.Reflection;
namespace AdventOfCode;
using Map = char[][];

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
        while (pos.IsWithinMap(map))
        {
            map[pos.X][pos.Y] = 'X';
            var nextPos = pos.Move(dir);
            if (!nextPos.IsWithinMap(map))
                break;
            if (map[nextPos.X][nextPos.Y] == '#')
                dir = dir.TakeRight();
            else
                pos = nextPos;
        }

        var count = map.Sum(row => row.Count(ch => ch == 'X'));
        
        Console.WriteLine(count);
    }

    public static void SolvePart2()
    {
        Console.WriteLine(0);
    }

    private static (Position?, Direction?) FindGuard(Map map)
    {
        for (int i = 0; i < map.Length; i++)
            for (int j = 0; j < map[i].Length; j++)
                if (Direction.IsDirectionSymbol(map[i][j])) return (new Position(i, j), Direction.FromSymbol(map[i][j]));

        return (null, null);
    }

    private record Position(int X, int Y)
    {
        public Position Move(Direction dir) => new Position(X + dir.XOffset, Y + dir.YOffset);
        public bool IsWithinMap(Map map) => X >= 0 && Y >= 0 && X < map.Length && Y < map.Length;
    }
    private record Direction(int XOffset, int YOffset, char Symbol, char rightSymbol)
    {
        private static Dictionary<char, Direction> dirs = new Dictionary<char, Direction>()
        {
            {'^', new Direction(-1, 0, '^', '>') },
            {'<', new Direction(0, -1, '<', '^') },
            {'>', new Direction(0, 1, '>', 'V') },
            {'V', new Direction(1, 0, 'V','<') },
        };

        public static bool IsDirectionSymbol(char symbol) => dirs.ContainsKey(symbol);
        public static Direction FromSymbol(char symbol) => dirs[symbol];
        public Direction TakeRight() => dirs[this.rightSymbol];
    }

    private static Map ReadInput() => File.ReadAllLines(InputPath).Select(r => r.ToCharArray()).ToArray();
}