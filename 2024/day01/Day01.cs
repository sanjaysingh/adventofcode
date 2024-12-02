using System.Reflection;

namespace AdventOfCode;

public static class Day01
{
    private static readonly string InputPath = Path.Combine(
        Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ?? string.Empty,
        "day01/input.txt"
    );

    private static IEnumerable<(int First, int Second)> ReadInputPairs() =>
        File.ReadLines(InputPath)
            .Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .Select(vals => (int.Parse(vals[0]), int.Parse(vals[1])));

    public static void SolvePart1()
    {
        var pairs = ReadInputPairs().ToList();
        var result = pairs
            .Select(pair => Math.Abs(pair.First - pair.Second))
            .Sum();

        Console.WriteLine(result);
    }

    public static void SolvePart2()
    {
        var pairs = ReadInputPairs().ToList();
        var firstNumbers = pairs.Select(p => p.First);
        var secondNumbers = pairs.Select(p => p.Second).ToList();

        var result = firstNumbers
            .Sum(x => x * secondNumbers.Count(y => x == y));

        Console.WriteLine(result);
    }
}