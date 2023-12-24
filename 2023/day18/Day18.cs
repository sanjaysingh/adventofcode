using System.Reflection;
namespace AdventOfCode;
public static class Day18
{
    private static readonly string inputFileFullPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "day18/input.txt");

    public static void SolvePart1()
    {
        Solve(ReadInput());
    }

    public static void SolvePart2()
    {
        var instructionsFaulty = ReadInput();
        var instructions = instructionsFaulty.Select(i => new Instruction("RDLU"[int.Parse(i.Color.Substring(7, 1))], Convert.ToInt64(i.Color.Substring(2, 5), 16), i.Color)).ToList();
        Solve(instructions);
    }

    private static void Solve(List<Instruction> instructions)
    {
        var polygon = new List<Point>() { new Point(0, 0) };
        instructions.ForEach(instruction =>
        {
            polygon.Add(polygon.Last().Add(instruction.Direction, instruction.Length));
        });
        polygon.Reverse();
        var boundaryPoints = instructions.Sum(i => i.Length);
        // shoelace formule
        long area = 0;
        for (int i = 0; i < polygon.Count - 1; i++)
            area += ((polygon[i].Y + polygon[i + 1].Y) * (polygon[i].X - polygon[i + 1].X)) / 2;
        // Picks theorem
        var innerPoints = (area + 1) - (boundaryPoints / 2);
        var result = boundaryPoints + innerPoints;
        Console.WriteLine(result);
    }

    private record Point(long X, long Y)
    {
        private static readonly Dictionary<char, Point> directionDeltaMap = new Dictionary<char, Point>
        {
            { 'D', new Point(1, 0) },
            { 'U', new Point(-1, 0) },
            { 'L', new Point(0, -1) },
            { 'R', new Point(0, 1) }
        };
        public Point Add(char direction, long length) => new Point(X + directionDeltaMap[direction].X * length, Y + directionDeltaMap[direction].Y * length);
    }
    private record Instruction(char Direction, long Length, string Color);
    private static List<Instruction> ReadInput() => File.ReadAllLines(inputFileFullPath).Select(line =>
                        {
                            var parts = line.Split(" ");
                            return new Instruction(char.Parse(parts[0]), long.Parse(parts[1]), parts[2]);
                        }).ToList();
}





























