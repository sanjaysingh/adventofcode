namespace AdventOfCode;

public static class Day19
{
    private static readonly string InputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day19", "input.txt");

    public static void SolvePart1()
    {
        var (towelPatterns, towelDesigns) = ReadInput();
        Console.WriteLine(towelDesigns.Count(t => CountDesignOptions(t, towelPatterns) > 0));
    }
    public static void SolvePart2()
    {
        var (towelPatterns, towelDesigns) = ReadInput();
        Console.WriteLine(towelDesigns.Sum(t => CountDesignOptions(t, towelPatterns)));
    }
    private static Dictionary<string, long> designOptions = new ();
    private static long CountDesignOptions(string design, HashSet<string> patterns)
    {
        if (designOptions.ContainsKey(design)) return designOptions[design];
        if (string.IsNullOrEmpty(design)) return 1;
        return designOptions[design] = patterns.Where(p => design.StartsWith(p)).Sum(p => CountDesignOptions(design.Substring(p.Length), patterns));
    }
    private static (HashSet<string>, List<string>) ReadInput()
    {
        var lines = File.ReadAllLines(InputPath);
        return (lines[0].Split(",", StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToHashSet(), lines.Skip(2).ToList());
    }
}