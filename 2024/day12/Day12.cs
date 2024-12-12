namespace AdventOfCode;
public static class Day12
{
    private static readonly string InputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day12", "input.txt");
    public static void SolvePart1()
    {
        var map = ReadInput();
        var plots = map.Plots.SelectMany(x => x).ToList();
        var regions = new List<List<Plot>>();
        while (plots.Any())
        {
            var plot = plots.First();
            var region = new List<Plot>();
            plot.MapRegion(region);
            regions.Add(region);
            plots.RemoveAll(p => region.Contains(p));
        }

        var totalPrice = 0;
        foreach (var region in regions)
        {
            var area = region.Count();
            var permiter = region.Sum(p => p.Permiter());
            var price = area * permiter;
            totalPrice += price;
        }

        Console.WriteLine(totalPrice);
    }
    public static void SolvePart2() { }

    private static Map ReadInput()
    {
        var map = new Map();
        var plots = File
        .ReadAllLines(InputPath)
            .Select((row, x) => row.Select((ch, y) => new Plot(ch, x, y, map)).ToList())
            .ToList();
        map.Plots = plots;
        return map;
    }

    private class Map
    {
        public List<List<Plot>> Plots { get; set; }
    }

    private record Plot(char Plant, int X, int Y, Map map)
    {
        private static readonly (int xOffset, int yOffset)[] neighborOffsets = new[] { (-1, 0), (0, 1), (1, 0), (0, -1) };
        public List<Plot> Neighbors()
        {
            var neighbors = new List<Plot>();
            foreach (var o in neighborOffsets)
            {
                int nx = X + o.xOffset, ny = Y + o.yOffset;
                if (IsInBounds(nx, ny)) neighbors.Add(map.Plots[nx][ny]);
            }
            return neighbors;
        }

        public void MapRegion(List<Plot> region)
        {
            if (region.Contains(this)) return;
            region.Add(this);
            this.Neighbors().Where(n => n.Plant == Plant).ToList().ForEach(n => { n.MapRegion(region); });
        }

        public int Permiter() => Neighbors().Count(n => n.Plant != Plant) + (4 - Neighbors().Count);
        private bool IsInBounds(int x, int y) => x >= 0 && y >= 0 && x < map.Plots.Count && y < map.Plots.Count;
    }
}