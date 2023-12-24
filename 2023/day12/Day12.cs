using System.Reflection;

namespace AdventOfCode;
public static class Day12
{
    private static readonly string inputFileFullPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "day12/input.txt");
    private record SpringMap(string Pattern, List<int> DamageCounts);
    private static Dictionary<string, long> cache = new Dictionary<string, long>();
    public static void SolvePart1()
    {
        long count = ReadInput().Select(map => CountPossibleWays(map.Pattern, map.DamageCounts)).Sum();
        Console.WriteLine(count);
    }
    public static void SolvePart2()
    {
        long count = ReadInput().Select(map => CountPossibleWays(string.Join("?", Enumerable.Range(0,5).Select(_ =>map.Pattern)), Enumerable.Range(0, 5).SelectMany(_ => map.DamageCounts).Select(x=>x).ToList())).Sum();
        Console.WriteLine(count);
    }
    private static long CountPossibleWays(string pattern, List<int> damageCounts)
    {
        if (pattern == "")
            return !damageCounts.Any() ? 1 : 0;

        if (!damageCounts.Any())
            return pattern.Contains('#') ? 0 : 1;

        var key = pattern + "|" + string.Join(",", damageCounts);
        if(cache.ContainsKey(key)) return cache[key];
        long result = 0;
        var startChar = pattern[0];
        if (startChar == '.' || startChar == '?')
            result += CountPossibleWays(pattern.Substring(1), damageCounts);

        if (startChar == '#' || startChar == '?')
            if (damageCounts[0] <= pattern.Length && !(pattern.Substring(0, damageCounts[0]).Contains(".")) && (damageCounts[0] == pattern.Length || pattern[damageCounts[0]] != '#'))
                result += CountPossibleWays(pattern.SafeSubstring(damageCounts[0] + 1), damageCounts.Skip(1).ToList());
        
        cache[key] = result;
        return result;
    }
    private static string SafeSubstring(this string str, int start) => start >= str.Length ? string.Empty : str.Substring(start);
    private static List<SpringMap> ReadInput() => File.ReadAllLines(inputFileFullPath).Select(line =>
            {
                var parts = line.Split(" ");
                return new SpringMap(parts[0], parts[1].Split(",").Select(int.Parse).ToList());
            }).ToList();
}
