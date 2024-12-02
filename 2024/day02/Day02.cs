using System.Reflection;
namespace AdventOfCode;

public static class Day02
{
    private static readonly string InputPath = Path.Combine(
        Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ?? string.Empty,
        "day02/input.txt"
    );

    public static void SolvePart1() =>
        Console.WriteLine(ReadReports().Count(IsSafe));

    public static void SolvePart2() =>
        Console.WriteLine(ReadReports().Count(CanBeMadeSafe));

    private static bool IsSafe(this IList<int> report)
    {
        var differences = report.Zip(report.Skip(1), (a, b) => b - a).ToList();
        return differences.All(d => Math.Abs(d) is >= 1 and <= 3) &&
               differences.All(d => d * differences[0] > 0);
    }

    private static bool CanBeMadeSafe(IList<int> report) =>
        report.IsSafe() || Enumerable.Range(0, report.Count)
            .Any(i => report.Where((_, k) => k != i).ToList().IsSafe());

    private static IEnumerable<List<int>> ReadReports() =>
        File.ReadLines(InputPath)
            .Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                               .Select(int.Parse)
                               .ToList());
}