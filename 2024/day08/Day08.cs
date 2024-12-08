namespace AdventOfCode;

public static class Day08
{
    private static readonly string InputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day08", "input.txt");

    public static void SolvePart1() => Solve(considerResonance: false);
    public static void SolvePart2() => Solve(considerResonance: true);

    private static void Solve(bool considerResonance)
    {
        var map = ReadInput();
        var antennas = FindAntennaGroups(map);

        foreach (var (_, locations) in antennas)
        {
            for (int i = 0; i < locations.Count - 1; i++)
            {
                for (int j = i + 1; j < locations.Count; j++)
                {
                    var antinodes = FindAntiNodeLocations(map, locations[i], locations[j], considerResonance);
                    foreach (var node in antinodes)
                    {
                        node.Antinode = true;
                    }
                }
            }
        }

        Console.WriteLine(map.SelectMany(l => l).Count(l => l.Antinode));
    }

    private static List<Location> FindAntiNodeLocations(List<List<Location>> map, Location first, Location second, bool considerResonance)
    {
        var result = new List<Location>();
        var (lower, higher) = first.X + first.Y <= second.X + second.Y ? (first, second) : (second, first);
        var (dx, dy) = (higher.X - lower.X, higher.Y - lower.Y);

        if (considerResonance)
        {
            result.AddRange(new[] { first, second });
        }

        result.AddRange(TraverseMap(map, lower, -dx, -dy, considerResonance));
        result.AddRange(TraverseMap(map, higher, dx, dy, considerResonance));

        return result;
    }

    private static IEnumerable<Location> TraverseMap(List<List<Location>> map, Location start, int dx, int dy, bool continueTraversal)
    {
        var current = start;
        while (true)
        {
            var next = map.Next(current, dx, dy);
            if (next is null) break;

            yield return next;
            if (!continueTraversal) break;

            current = next;
        }
    }

    private static bool IsInBounds(this List<List<Location>> map, int x, int y) =>
        x >= 0 && y >= 0 && x < map.Count && y < map.Count;

    private static Location? Next(this List<List<Location>> map, Location loc, int dx, int dy)
    {
        var (x, y) = (loc.X + dx, loc.Y + dy);
        return map.IsInBounds(x, y) ? map[x][y] : null;
    }

    private static Dictionary<char, List<Location>> FindAntennaGroups(List<List<Location>> map) =>
        map.SelectMany(r => r)
           .Where(l => l.HasAntenna)
           .GroupBy(l => l.Frequency)
           .ToDictionary(g => g.Key, g => g.ToList());

    private static List<List<Location>> ReadInput() =>
        File.ReadLines(InputPath)
            .Select((line, x) => line.Select((ch, y) => new Location(x, y, ch)).ToList())
            .ToList();

    private record Location(int X, int Y, char Frequency)
    {
        public bool HasAntenna => char.IsLetterOrDigit(Frequency);
        public bool Antinode { get; set; }
    }
}