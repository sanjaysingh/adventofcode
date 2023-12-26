using System.Reflection;
namespace AdventOfCode;
using Map = List<List<char>>;
public static class Day23
{
    private static readonly string inputFileFullPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "day23/input.txt");
    private static Map EmptyMap = new Map();
    private static readonly Dictionary<char, List<Position>> Offsets = new()
    {
        ['.'] = [new(-1, 0, EmptyMap), new(0, -1, EmptyMap), new(1, 0, EmptyMap), new(0, 1, EmptyMap)],
        ['^'] = [new(-1, 0, EmptyMap)],
        ['v'] = [new(1, 0, EmptyMap)],
        ['<'] = [new(0, -1, EmptyMap)],
        ['>'] = [new(0, 1, EmptyMap)],
        ['#'] = [],
    };
    public static void SolvePart1()
    {
        var map = ReadInput();
        var start = new Position(0, map[0].IndexOf('.'), map);
        var end = new Position(map.Count - 1, map[map.Count - 1].IndexOf('.'), map);
        var nodeToProcess = new Stack<Hop>();
        nodeToProcess.Push(new Hop(start, start, 0, start.Id));
        int longestPath = 0;
        while (nodeToProcess.Count > 0)
        {
            var currentHop = nodeToProcess.Pop();
            if (currentHop.Current.Id == end.Id)
            {
                longestPath = currentHop.VisitedCount > longestPath ? currentHop.VisitedCount : longestPath;
            }
            foreach (var neighbor in map.GetValidNextHops(currentHop))
            {
                nodeToProcess.Push(neighbor);
            }
        }
        Console.WriteLine(longestPath);
    }

    private static IEnumerable<Hop> GetValidNextHops(this Map map, Hop currentHop)
    {
        var inBounds = (Map map, Position pos) => pos.X >= 0 && pos.Y >= 0 && pos.X < map.Count && pos.Y < map[pos.X].Count;
        var currentPosition = currentHop.Current;
        var validPositions = Offsets[map[currentPosition.X][currentPosition.Y]].Select(offset => currentPosition.Add(offset)).Where(x => inBounds(map, x) && map[x.X][x.Y] != '#');
        return validPositions.Where(p => !currentHop.Trail.Contains(p.Id)).Select(p => currentHop.NextHop(p));
    }

    private record Hop(Position? Previous, Position Current, int VisitedCount, string Trail)
    {
        public Hop NextHop(Position nextPosition) => new Hop(Current, nextPosition, VisitedCount + 1, $"{Trail}-{nextPosition.Id}");
    }

    private record Position(int X, int Y, Map map)
    {
        public Position Add(Position offset) => new Position(X + offset.X, Y + offset.Y, map);
        public string Id => $"'{X * map[X].Count + Y}'";
    }
    private static Map ReadInput() => File.ReadAllLines(inputFileFullPath).Select(line => line.ToList()).ToList();
}