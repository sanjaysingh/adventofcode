using System.Reflection;
namespace AdventOfCode;
public static class Day17
{
    private static readonly string inputFileFullPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "day17/input.txt");

    public static void SolvePart1()
    {
        Solve(0, 3);
    }

    public static void SolvePart2()
    {
        Solve(4, 10);
    }

    public static void Solve(int minStraight, int maxStraight)
    {
        var graph = ReadInput();
        var stepsToProcess = new PriorityQueue<Crucible, int>();
        graph[0][0] = 0;
        stepsToProcess.Enqueue(new Crucible(new Coord(0, 0), graph), 0);
        var energyLoss = int.MaxValue;
        var track = new Dictionary<string, int>();
        while (stepsToProcess.TryDequeue(out var currCruciblePos, out var heatloss))
        {
            if (currCruciblePos.IsAtDestination())
            {
                energyLoss = currCruciblePos.HeatLoss;
                break;
            }

            foreach (var newPos in currCruciblePos.DriveNext(minStraight, maxStraight))
            {
                if (!track.ContainsKey(newPos.PositionId))
                {
                    stepsToProcess.Enqueue(newPos, newPos.HeatLoss);
                    track[newPos.PositionId] = newPos.HeatLoss;
                }
            }
        }

        Console.WriteLine(energyLoss);
    }

    private static List<List<int>> ReadInput()
    {
        return File.ReadAllLines(inputFileFullPath).Select(line => line.Select(ch => int.Parse(ch.ToString())).ToList()).ToList();
    }

    private record Coord(int X, int Y)
    {
        public Coord NewCoord(Coord delta) => new Coord(X + delta.X, Y + delta.Y);
        public string AsString() => $"{X},{Y}";
    }
    private class Crucible
    {
        private static Dictionary<char, string> possibleDirections = new Dictionary<char, string>() { { '0', "NSEW" }, { 'N', "NEW" }, { 'S', "SEW" }, { 'E', "ENS" }, { 'W', "WNS" } };
        private Dictionary<char, Coord> directionOffset = new Dictionary<char, Coord>
        {
            ['N'] = new Coord(-1, 0),
            ['S'] = new Coord(1, 0),
            ['E'] = new Coord(0, 1),
            ['W'] = new Coord(0, -1)
        };
        private readonly string direction = "0";
        private readonly List<List<int>> graph;
        private int straightHop = 1;

        public Crucible(Coord coord, List<List<int>> graph)
        {
            this.Coord = coord;
            this.graph = graph;
            this.HeatLoss = graph[coord.X][coord.Y];
        }

        private Crucible(Crucible prev, char newDir, Coord newCoord)
        {
            if (prev.direction.Last() == newDir || prev.direction == "0")
            {
                this.straightHop = prev.straightHop + 1;
            }
            this.direction = prev.direction == "0" ? $"{newDir}{newDir}" : $"{prev.direction}{newDir}";
            this.Coord = newCoord;
            this.graph = prev.graph;
            this.HeatLoss = prev.HeatLoss + graph[newCoord.X][newCoord.Y];
        }

        public string PositionId => $"{this.Coord.AsString()}|{this.direction.Last()}|{this.straightHop}";
        public Coord Coord { get; private set; }
        public int HeatLoss { get; private set; }

        public bool IsAtDestination() => this.Coord.X == this.graph.Count - 1 && this.Coord.Y == this.graph.Count - 1;
        public IEnumerable<Crucible> DriveNext(int minStraight, int maxStraight)
        {
            if (this.IsAtDestination()) return Enumerable.Empty<Crucible>();
            var positions = new List<Crucible>();
            foreach (var newDir in possibleDirections[this.direction.Last()])
            {
                var newCoord = this.Coord.NewCoord(directionOffset[newDir]);
                if (!IsValidCoord(newCoord)) continue;
                var newCrucible = new Crucible(this, newDir, newCoord);
                if(newCrucible.straightHop > maxStraight) continue;
                if(this.straightHop < minStraight && newCrucible.straightHop == 1) continue;
                if(newCrucible.IsAtDestination() && newCrucible.straightHop < minStraight) continue;
                positions.Add(new Crucible(this, newDir, newCoord));
            }
            return positions;
        }
        private bool IsValidCoord(Coord c) => c.X >= 0 && c.Y >= 0 && c.X < graph.Count && c.Y < graph.Count;

    }
}





























