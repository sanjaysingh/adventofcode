using System.Reflection;
namespace AdventOfCode;
using Update = List<int>;

public static class Day05
{
    private static readonly string InputPath = Path.Combine(
        Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ?? string.Empty,
        "day05/input.txt"
    );
    private static Comparer<int> RuleComparer(string rules) => Comparer<int>.Create((x, y) => rules.Contains($"{x}|{y}") ? -1 : 1);
    private static bool IsOrdered(this Update update, string rules) => update.SequenceEqual(update.Order(RuleComparer(rules)));
    public static void SolvePart1()
    {
        var (rules, updates) = ReadInput();
        var sum = updates
                    .Where(u => u.IsOrdered(rules))
                    .Sum(u => u[u.Count/2]);
        Console.WriteLine(sum);
    }
    public static void SolvePart2()
    {
        var (rules, updates) = ReadInput();
        var sum = updates
                    .Where(u => !u.IsOrdered(rules))
                    .Select(u => u.Order(RuleComparer(rules)).ToList())
                    .Sum(u => u[u.Count / 2]);
        Console.WriteLine(sum);
    }

    private static (string, List<List<int>>) ReadInput()
    {
        var input = File.ReadAllText(InputPath);
        var parts = input.Split("\r\n\r\n");
        return (parts[0], parts[1].Split('\n').Select(r => r.Split(",").Select(int.Parse).ToList()).ToList());
    }
}