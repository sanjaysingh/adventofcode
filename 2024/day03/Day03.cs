using System.Reflection;
using System.Text.RegularExpressions;
namespace AdventOfCode;

public static class Day03
{
    private static readonly string InputPath = Path.Combine(
        Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ?? string.Empty,
        "day03/input.txt"
    );
    public static void SolvePart1()
    {
        var pattern = @"mul\(\d{1,3},\d{1,3}\)";
        var regex = new Regex(pattern, RegexOptions.Compiled);
        var matches = regex.Matches(ReadProgram());
        var sum = matches.Sum(m => EvalMulExpression(m.Value));
        Console.WriteLine(sum);
    }
    public static void SolvePart2() 
    {
        var pattern = @"mul\(\d{1,3},\d{1,3}\)|do\(\)|don't\(\)";
        var regex = new Regex(pattern, RegexOptions.Compiled);
        var sum = 0;
        var shouldMultiply = true;
        foreach (Match match in regex.Matches(ReadProgram()))
        {
            if (match.Value == "do()") shouldMultiply = true;
            else if (match.Value == "don't()") shouldMultiply = false;
            else if (shouldMultiply)
                sum += EvalMulExpression(match.Value);
        }

        Console.WriteLine(sum);
    }

    private static int EvalMulExpression(string expr) => expr.Replace("mul(", "").Replace(")", "").Split(",").Select(int.Parse).Aggregate((x, y) => x * y);

    private static string ReadProgram() =>
        File.ReadAllText(InputPath);
}