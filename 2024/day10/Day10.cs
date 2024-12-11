using Map = int[][];
namespace AdventOfCode;

public static class Day10
{
    private static readonly string InputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day10", "input.txt");
    public static void SolvePart1()
    {
        var map = ReadInput();
        var trailHeads = map.Select((row, x) => row.Select((col, y) => new Location(x, y))).SelectMany(x => x).Where(c => map[c.X][c.Y] == 0);
        var sum = trailHeads.Sum(t => FindHikingTrailScore(map, t));
        Console.WriteLine(sum);

    }
    public static void SolvePart2()
    {
        var map = ReadInput();
        var trailHeads = map.Select((row, x) => row.Select((col, y) => new Location(x, y))).SelectMany(x => x).Where(c => map[c.X][c.Y] == 0);
        var sum = trailHeads.Sum(t => FindHikingTrailRating(map, t));
        Console.WriteLine(sum);
    }

    private static int FindHikingTrailScore(int[][] map, Location loc, HashSet<string> hikingTrails = null)
    {
        if (hikingTrails == null) hikingTrails = new ();
        if (map.At(loc) == 9)
        {
            hikingTrails.Add(loc.Id);
            return hikingTrails.Count;
        }
        var neighbors = new List<Location> { loc.Left, loc.Right, loc.Top, loc.Bottom };
        neighbors.ForEach(n =>
        {
            if (n.IsInBounds(map) && map.At(n) - map.At(loc) == 1)
                FindHikingTrailScore(map, n, hikingTrails);
        });
        return hikingTrails.Count;
    }
    private static int FindHikingTrailRating(int[][] map, Location loc)
    {
        if (map.At(loc) == 9) return 1;

        var count = 0;
        var neighbors = new [] { loc.Left, loc.Right, loc.Top, loc.Bottom }.ToList();
        neighbors.ForEach(n =>
        {
            if (n.IsInBounds(map) && map.At(n) - map.At(loc) == 1)
                count += FindHikingTrailRating(map, n);
        });
        return count;
    }

    private static int At(this Map map, Location loc) => map[loc.X][loc.Y];
    private static Map ReadInput() =>
        File.ReadAllLines(InputPath).Select(row => row.Select(ch => int.Parse(ch.ToString())).ToArray()).ToArray();
    private record Location(int X, int Y)
    {
        public string Id => $"{X},{Y}";
        public Location Left => new Location(X, Y - 1);
        public Location Right => new Location(X, Y + 1);
        public Location Top => new Location(X - 1, Y);
        public Location Bottom => new Location(X + 1, Y);
        public bool IsInBounds(Map map) => X >= 0 && Y >= 0 && X < map.Length && Y < map.Length;
    }
}