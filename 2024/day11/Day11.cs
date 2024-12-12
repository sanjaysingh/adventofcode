namespace AdventOfCode;
public static class Day11
{
    private static readonly string InputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day11", "input.txt");
    private static Dictionary<string, long> stonesCountMap = new Dictionary<string, long>();
    public static void SolvePart1() => Console.WriteLine(GetStoneCountAfterBlink(ReadInput(), 25));
    public static void SolvePart2() => Console.WriteLine(GetStoneCountAfterBlink(ReadInput(), 75));
    private static long GetStoneCountAfterBlink(long[] stones, int blinkCount) => stones.Sum(s => GetStoneCountAfterBlink(s, blinkCount));
    private static long GetStoneCountAfterBlink(long stone, int blinkCount)
    {
        var key = $"{stone},{blinkCount}";
        if (stonesCountMap.ContainsKey(key)) return stonesCountMap[key];
        long result = 0;
        if (blinkCount == 1)
            result = Blink(stone).LongLength;
        else
            result = Blink(stone).Sum(s => GetStoneCountAfterBlink(s, blinkCount - 1));
        stonesCountMap[key] = result;
        return result;
    }
    private static long[] Blink(long stone)
    {
        if (stone == 0)
            return new long[] { 1 };
        else if (stone.ToString().Length % 2 == 0)
            return new long[] { long.Parse(stone.ToString().Substring(0, stone.ToString().Length / 2)), long.Parse(stone.ToString().Substring(stone.ToString().Length / 2)) };
        return new long[] { stone * 2024 };
    }
    private static long[] ReadInput() => File.ReadAllText(InputPath).Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
}