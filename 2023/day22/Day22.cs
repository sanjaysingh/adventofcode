using System.Reflection;
namespace AdventOfCode;
public static class Day22
{
    private static readonly string inputFileFullPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "day22/input.txt");

    public static void SolvePart1()
    {
        var bricks = ReadInput();
        var SettledBricks = ApplyGravity(bricks);
        var (supports, supportedBy) = FindSupports(SettledBricks);
        int dismantleCount = supports.Where(s => s.Value.All(b => supportedBy.ContainsKey(b) && supportedBy[b].Count > 1)).Count();
        Console.WriteLine(dismantleCount);
    }

    public static void SolvePart2()
    {
        var bricks = ReadInput();
        var SettledBricks = ApplyGravity(bricks);
        var (supports, supportedBy) = FindSupports(SettledBricks);
        var count = SettledBricks.Select(s =>
        {
            var fallens = new HashSet<Brick>();
            FindDismantledChainOfBricks(s, supports, supportedBy, fallens);
            return fallens.Count - 1;
        }).Sum();
        Console.WriteLine(count);
    }

    private static void FindDismantledChainOfBricks(Brick disintegratingBrick, Dictionary<Brick, List<Brick>> supports, Dictionary<Brick, HashSet<Brick>> supportedBy, HashSet<Brick> Fallens)
    {
        Fallens.Add(disintegratingBrick);
        foreach (var childDismantedBrick in supports[disintegratingBrick].Where(supportedBrick => supportedBy[supportedBrick].All(x => Fallens.Contains(x))))
        {
            FindDismantledChainOfBricks(childDismantedBrick, supports, supportedBy, Fallens);
        }
    }

    private static List<Brick> ApplyGravity(List<Brick> bricks)
    {
        bricks = bricks.OrderBy(b => b.Start.Z).ToList();
        var updatedBricks = new List<Brick>();
        foreach (var brick in bricks)
        {
            var currPos = brick;
            while (!currPos.OnFloor())
            {
                var nextPos = currPos.FallByOne();
                if (nextPos.InvalidFallPosition(updatedBricks)) break;
                currPos = nextPos;
            }

            updatedBricks.Add(currPos);
        }
        return updatedBricks;
    }

    private static bool InvalidFallPosition(this Brick brick, List<Brick> bricks)
    {
        foreach (var neighbor in bricks.Where(b => b.MaxZ == brick.MinZ && b.ID != brick.ID))
        {
            if (brick.Start.X <= neighbor.End.X && brick.End.X >= neighbor.Start.X && brick.Start.Y <= neighbor.End.Y && brick.End.Y >= neighbor.Start.Y) return true;
        }
        return false;
    }

    private static (Dictionary<Brick, List<Brick>>, Dictionary<Brick, HashSet<Brick>>) FindSupports(List<Brick> bricks)
    {
        var supports = new Dictionary<Brick, List<Brick>>();
        foreach (var brick in bricks)
        {
            supports[brick] = new List<Brick>();
            foreach (var neighbor in bricks.Where(b => b.MinZ == brick.MaxZ + 1 && b.ID != brick.ID))
            {
                if (brick.Start.X <= neighbor.End.X && brick.End.X >= neighbor.Start.X && brick.Start.Y <= neighbor.End.Y && brick.End.Y >= neighbor.Start.Y)
                    supports[brick].Add(neighbor);
            }
        }

        var supportedBy = new Dictionary<Brick, HashSet<Brick>>();
        foreach (var brick in supports)
        {
            foreach (var s in brick.Value)
            {
                if (!supportedBy.ContainsKey(s)) supportedBy[s] = new HashSet<Brick>();
                supportedBy[s].Add(brick.Key);
            }
        }
        return (supports, supportedBy);
    }

    private record Pos(int X, int Y, int Z)
    {
        public Pos ShiftZ(int dz) => new Pos(X, Y, Z + dz);
    }
    private record Brick(int ID, Pos Start, Pos End)
    {
        public Brick FallByOne() => new Brick(ID, Start.ShiftZ(-1), End.ShiftZ(-1));
        public Brick OneAbove() => new Brick(ID, Start.ShiftZ(1), End.ShiftZ(1));
        public int MinZ => Math.Min(Start.Z, End.Z);
        public int MaxZ => Math.Max(Start.Z, End.Z);
        public bool OnFloor() => this.MinZ == 1;
    }

    private static List<Brick> ReadInput()
    {
        int Id = 0;
        return File.ReadAllLines(inputFileFullPath).Select(line =>
        {
            var parts = line.Split(',', '~').Select(int.Parse).ToArray();
            Id++;
            return new Brick(Id, new Pos(parts[0], parts[1], parts[2]), new Pos(parts[3], parts[4], parts[5]));
        }).ToList();
    }
}