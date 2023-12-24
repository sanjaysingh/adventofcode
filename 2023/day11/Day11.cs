using System.Reflection;
namespace AdventOfCode;
public static class Day11
{
    private static readonly string inputFileFullPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "day11/input.txt");
    private record Coord(long X, long Y);
    public static void SolvePart1()
    {
        var map = ReadInput();
        var galaxies = FindGalaxies(map);
        var result = galaxies.SelectMany(c1 => galaxies.Select(c2 => FindDistance(map, c1, c2, 1))).Sum()/2;
        Console.WriteLine(result);
    }

    public static void SolvePart2()
    {
        var map = ReadInput();
        var galaxies = FindGalaxies(map);
        var result = galaxies.SelectMany(c1 => galaxies.Select(c2 => FindDistance(map, c1, c2, 999999))).Sum() / 2;
        Console.WriteLine(result);
    }

    private static long FindDistance(List<List<char>> map, Coord g1, Coord g2, int emptyMultiplier)
    {
        long x = Math.Min(g1.X, g2.X), xDist = Math.Abs(g1.X - g2.X), y = Math.Min(g1.Y, g2.Y), yDist = Math.Abs(g1.Y - g2.Y);
        var emptyRows = emptyMultiplier * Enumerable.Range((int)x, (int)xDist).Where(i => map[i].All(ch => ch == '.')).Count();
        var emptyCols = emptyMultiplier * Enumerable.Range((int)y, (int)yDist).Where(cIndex => map.All(row => row[cIndex] == '.')).Count();
        return (xDist + emptyRows + yDist + emptyCols);
    }

    private static List<Coord> FindGalaxies(List<List<char>> map)
    {
        var galaxies = new List<Coord>();
        for (var row = 0; row < map.Count(); row++)
            for (var col = 0; col < map[row].Count(); col++)
                if (map[row][col] == '#')
                    galaxies.Add(new Coord(row, col));
        return galaxies;
    }
    private static List<List<char>> ReadInput()
    {
        return File.ReadAllLines(inputFileFullPath).Select(line => line.ToCharArray().ToList()).ToList();
    }
}
