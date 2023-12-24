using System.Reflection;
namespace AdventOfCode;

public static class Day16
{
    private static readonly string inputFileFullPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "day16/input.txt");

    public static void SolvePart1()
    {
        var grid = ReadInput();
        Console.WriteLine(grid.Light(0, 0, 'E'));
    }

    public static void SolvePart2()
    {
        var grid = ReadInput();
        var energyList = new List<int>();
        for (var i = 0; i< grid.Count; i++)
        {
            energyList.Add(grid.Light(i, 0, 'E'));
            energyList.Add(grid.Light(i, grid[0].Count-1, 'W'));
            energyList.Add(grid.Light(0, i, 'S'));
            energyList.Add(grid.Light(0, i, 'N'));
        }

        Console.WriteLine(energyList.Max());
    }

    private static int Light(this List<List<Tile>> grid, int startX, int startY, char direction)
    {
        var tilesToTraverse = new Stack<BeamPos>();
        tilesToTraverse.Push(new BeamPos(direction, grid[startX][startY]));
        while (tilesToTraverse.Count > 0)
        {
            var currBeamPos = tilesToTraverse.Pop();
            var nextBeamPositions = currBeamPos.Tile.BeamLight(currBeamPos.Direction);
            nextBeamPositions.ToList().ForEach(newPos => tilesToTraverse.Push(newPos));
        }
        var energy = grid.TotalEnergy();
        grid.Reset();
        return energy;
    }

    private static int TotalEnergy(this List<List<Tile>> grid) => grid.SelectMany(x => x).Count(t => t.IsEnergized);
    private static void Reset(this List<List<Tile>> grid) => grid.SelectMany(x=>x).ToList().ForEach(t => t.Reset());
    private static List<List<Tile>> ReadInput()
    {
        var lines = File.ReadAllLines(inputFileFullPath);
        var grid = new List<List<Tile>>();
        for (var i = 0; i < lines.Length; i++)
        {
            var row = new List<Tile>();
            for (var j = 0; j < lines[i].Length; j++)
            {
                row.Add(new Tile(lines[i][j], new Point(i, j), grid));
            }
            grid.Add(row);
        }
        return grid;
    }
    private record Point(int X, int Y)
    {
        public Point Next(Point delta) => new Point(X + delta.X, Y + delta.Y);
    }

    private record BeamPos(char Direction, Tile Tile);

    private class Tile(char symbol, Point pos, List<List<Tile>> grid)
    {
        private readonly Dictionary<char, Point> nextDirectionDelta = new Dictionary<char, Point>() { { 'N', new Point(-1, 0) }, { 'S', new Point(1, 0) }, { 'E', new Point(0, 1) }, { 'W', new Point(0, -1) } };
        private readonly Dictionary<string, string> beamReflection = new Dictionary<string, string>()
        {
            {@"N|", "N"},
            {@"N-", "EW"},
            {@"N/", "E"},
            {@"N\", "W"},
            {@"N.", "N"},
            {@"S|", "S"},
            {@"S-", "EW"},
            {@"S/", "W"},
            {@"S\", "E"},
            {@"S.", "S"},
            {@"E|", "NS"},
            {@"E-", "E"},
            {@"E/", "N"},
            {@"E\", "S"},
            {@"E.", "E"},
            {@"W|", "NS"},
            {@"W-", "W"},
            {@"W/", "S"},
            {@"W\", "N"},
            {@"W.", "W"}
        };
        private string currentBeams = string.Empty;

        public void Reset() => currentBeams = string.Empty;

        public bool IsEnergized => currentBeams.Length > 0;

        public IEnumerable<BeamPos> BeamLight(char direction)
        {
            var dirs = new List<BeamPos>();
            if (currentBeams.Contains(direction))
                return dirs;

            var key = $"{direction}{symbol}";
            foreach (char reflectedDir in beamReflection[key])
            {
                var newPos = pos.Next(nextDirectionDelta[reflectedDir]);
                if (newPos.X >= 0 && newPos.X < grid.Count && newPos.Y >= 0 && newPos.Y < grid[0].Count)
                {
                    dirs.Add(new BeamPos(reflectedDir, grid[newPos.X][newPos.Y]));
                }
            }
            currentBeams += direction.ToString();
            return dirs;
        }
    }
}
