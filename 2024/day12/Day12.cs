namespace AdventOfCode;
public static class Day12
{
    private static readonly string InputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day12", "input.txt");
    public static void SolvePart1()
    {
        var price = FindRegions(ReadInput())
                    .Sum(r => r.Count * r.Sum(p => p.Permiter()));
        Console.WriteLine(price);
    }
    public static void SolvePart2()
    {
        var price = FindRegions(ReadInput())
                     .Sum(r => r.Count * r.Sum(p => p.CountCorners()));
        Console.WriteLine(price);
    }

    private static Map ReadInput()
    {
        var map = new Map();
        var plots = File
        .ReadAllLines(InputPath)
            .Select((row, x) => row.Select((ch, y) => new Plot(ch, new Coord(x, y), map)).ToList())
            .ToList();
        map.Plots = plots;
        return map;
    }

    private static List<List<Plot>> FindRegions(Map map)
    {
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
        return regions;
    }

    private class Map
    {
        public List<List<Plot>> Plots { get; set; }
        public bool IsInBounds(Coord c) => c.X >= 0 && c.Y >= 0 && c.X < Plots.Count && c.Y < Plots.Count;

        public char? GetPlant(Coord coord) => IsInBounds(coord) ? Plots[coord.X][coord.Y].Plant : null;
    }

    private record Coord(int X, int Y)
    {
        public Coord Add(Coord offset) => new Coord(X + offset.X, Y + offset.Y);
    }
    private static readonly Coord Left = new Coord(0, -1);
    private static readonly Coord Right = new Coord(0, 1);
    private static readonly Coord Top = new Coord(-1, 0);
    private static readonly Coord Bottom = new Coord(1, 0);
    private static readonly Coord[] AllDirections = new Coord[] { Left, Right, Top, Bottom };

    private record Plot(char Plant, Coord Coord, Map map)
    {
        public List<Plot> Neighbors() =>
            AllDirections
                .Select(x => Coord.Add(x))
                .Where(x => map.IsInBounds(x))
                .Select(c => map.Plots[c.X][c.Y])
                .ToList();

        public int CountCorners()
        {
            int count = 0;
            foreach(var (dx, dy) in new[] { (Left, Top), (Right, Top), (Right, Bottom), (Left, Bottom) })
            {
                if(map.GetPlant(Coord.Add(dx)) != Plant && map.GetPlant(Coord.Add(dy)) != Plant) count++;
                if (map.GetPlant(Coord.Add(dx)) == Plant && map.GetPlant(Coord.Add(dy)) == Plant && map.GetPlant(Coord.Add(dx).Add(dy)) != Plant) count++;

            }
            return count;
        }

        public void MapRegion(List<Plot> region)
        {
            if (region.Contains(this)) return;
            region.Add(this);
            this.Neighbors().Where(n => n.Plant == Plant).ToList().ForEach(n => { n.MapRegion(region); });
        }

        public int Permiter() => Neighbors().Count(n => n.Plant != Plant) + (4 - Neighbors().Count);
    }
}