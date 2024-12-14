using System.Text.RegularExpressions;
namespace AdventOfCode;
public static class Day14
{
    private static readonly string InputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day14", "input.txt");
    public static void SolvePart1()
    {
        var robots = ReadInput();
        int width = 101;
        int height = 103;
        int seconds = 100;
        robots.ForEach(r => r.Walk(seconds, width, height));
        var centerX = width / 2;
        var centerY = height / 2;
        var quadrants = robots
                .Where(r => r.CurrentPosition.X != centerX && r.CurrentPosition.Y != centerY)
                .GroupBy(r => (r.CurrentPosition.X > centerX, r.CurrentPosition.Y > centerY))
                .ToDictionary(g => g.Key, g => g.Count());

        var result = quadrants.Values.Aggregate(1, (acc, val) => acc * val);

        Console.WriteLine(result);
    }
    public static void SolvePart2()
    {
        var robots = ReadInput();
        int width = 101;
        int height = 103;
        int seconds = 0;
        for (var i = 0; i < 1000000000; i++)
        {
            robots.ForEach(r => r.Walk(1, width, height));
            seconds++;
            if (FormsXMasTree(robots, width, height)) break;
        }

        Console.WriteLine(seconds);
    }

    private record Coord(int X, int Y);
    private record Robot(Coord Position, Coord Velocity)
    {
        public Coord CurrentPosition { get; private set; } = Position;
        public void Walk(int seconds, int width, int height)
        {
            int newX = (width + ((this.CurrentPosition.X + (this.Velocity.X * seconds)) % width)) % width;
            int newXY = (height + ((this.CurrentPosition.Y + (this.Velocity.Y * seconds)) % height)) % height;
            CurrentPosition = new Coord(newX, newXY);
        }
    }

    private static bool FormsXMasTree(List<Robot> robots, int width, int height)
    {
        var map = new char[height, width];
        foreach (var robot in robots)
            map[robot.CurrentPosition.Y, robot.CurrentPosition.X] = '0';

        return string.Join(Environment.NewLine,
            Enumerable.Range(0, height)
                     .Select(h => new string(
                         Enumerable.Range(0, width)
                         .Select(w => map[h, w] == default ? ' ' : map[h, w])
                         .ToArray())))
            .Contains("0000000000");
    }

    private static List<Robot> ReadInput() =>
        File
            .ReadAllLines(InputPath)
            .Select(line =>
            {
                var matches = Regex.Matches(line, @"-?\d+");
                return new Robot(new Coord(int.Parse(matches[0].Value), int.Parse(matches[1].Value)), new Coord(int.Parse(matches[2].Value), int.Parse(matches[3].Value)));
            }).ToList();
}