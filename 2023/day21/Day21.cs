using System.Reflection;
namespace AdventOfCode;
public static class Day21
{
    private static readonly string inputFileFullPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "day21/input.txt");
    private static List<Position> ValidPositionOffsets = new List<Position> { new Position(-1, 0), new Position(0, -1), new Position(1, 0), new Position(0, 1) };

    public static void SolvePart1()
    {
        var garden = ReadInput();
        for(var i=0; i< 64; i++)
        {
            var positions = GetGardenerPositions(garden);
            positions.ForEach(p => MoveGardenerToValidPositions(garden, p));
        }

        Console.WriteLine(GetGardenerPositions(garden).Count());
    }

    public static void SolvePart2()
    {
        Console.WriteLine();
    }

    private static void MoveGardenerToValidPositions(List<List<char>> garden, Position gardener)
    {
        ValidPositionOffsets.ForEach(offset => 
        {
            var newPostion = gardener.AddOffset(offset);
            if (IsValidPosition(garden, newPostion))
            {
                garden[newPostion.X][newPostion.Y] = 'O';
                garden[gardener.X][gardener.Y] = '.';
            }
        });
    }

    private static bool IsValidPosition(List<List<char>> garden, Position position) =>
        position.X >= 0 && position.X < garden.Count() && position.Y >= 0 && position.Y < garden[position.X].Count && garden[position.X][position.Y] != '#';
    private static List<Position> GetGardenerPositions(List<List<char>> garden) =>
        garden.SelectMany((row, i) =>
                     row.Select((cell, j) => new { i, j, cell })
                        .Where(p => p.cell == 'O' || p.cell == 'S'))
                    .Select(p => new Position(p.i, p.j))
                    .ToList();
    private record Position(int X, int Y)
    {
        public Position AddOffset(Position offset) => new Position(X + offset.X, Y + offset.Y);
    }
    private static List<List<char>> ReadInput() => File.ReadAllLines(inputFileFullPath).Select(line => line.ToList()).ToList();
}