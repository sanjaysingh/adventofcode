using System.Reflection;
namespace AdventOfCode;
public static class Day10
{
    private static readonly string inputFileFullPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "day10/input.txt");
    private static Dictionary<char, string> pipeDirections = new Dictionary<char, string>() { { '|', "NS" }, { '-', "EW" }, { 'L', "NE" }, { 'J', "NW" }, { '7', "SW" }, { 'F', "SE" }, { 'S', "EWNS" }, { '.', "" } };
    private static Dictionary<char, int> DirectionXDelta = new Dictionary<char, int> { { 'E', 0 }, { 'W', 0 }, { 'N', -1 }, { 'S', 1 } };
    private static Dictionary<char, int> DirectionYDelta = new Dictionary<char, int> { { 'E', 1 }, { 'W', -1 }, { 'N', 0 }, { 'S', 0 } };
    private static Dictionary<char, char> Opposites = new Dictionary<char, char>() { { 'E', 'W' }, { 'W', 'E' }, { 'N', 'S' }, { 'S', 'N' } };

    private record MapPoint(int X, int Y, char Symbol, string Connections, bool IsStart);
    private record Map(MapPoint StartPoint, List<List<MapPoint>> Grid);
    private record Hop(char FromDirection, MapPoint CurrentPoint, int Length)
    {
        public List<Hop> GetTargetConnections(Map map)
        {
            var dirs = CurrentPoint.Connections.Where(ch => Opposites[ch] != FromDirection);
            var hops = new List<Hop>();
            foreach (var dir in dirs)
            {
                int x = CurrentPoint.X + DirectionXDelta[dir];
                int y = CurrentPoint.Y + DirectionYDelta[dir];
                if (x >= 0 && y >= 0 && x < map.Grid.Count() && y < map.Grid[0].Count() && map.Grid[x][y].Connections.Contains(Opposites[dir]))
                {
                    hops.Add(new Hop(dir, map.Grid[x][y], Length + 1));
                }
            }
            return hops;
        }
    }

    public static void SolvePart1()
    {
        var map = ReadInput();
        var pointsToVisit = new Stack<Hop>();
        pointsToVisit.Push(new Hop('0', map.StartPoint, 0));
        int hopLength = 0;
        while (pointsToVisit.Count > 0)
        {
            var currHop = pointsToVisit.Pop();
            if (currHop.CurrentPoint.IsStart && currHop.Length > 0) { hopLength = currHop.Length; break; };
            currHop.GetTargetConnections(map).ForEach(x => pointsToVisit.Push(x));
        }

        Console.WriteLine(hopLength/2);
    }
    public static void SolvePart2()
    {
        var result = 0;
        Console.WriteLine(result);
    }
    private static Map ReadInput()
    {
        var points = File.ReadAllLines(inputFileFullPath).Select((line, row) => line.Select((ch, col) => new MapPoint(row, col, ch, pipeDirections[ch], ch == 'S')).ToList()).ToList();
        return new Map(points.SelectMany(x => x).First(x => x.IsStart), points);
    }
}
