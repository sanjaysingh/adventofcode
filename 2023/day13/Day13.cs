using System.Reflection;
namespace AdventOfCode;
public static class Day13
{
    private static readonly string inputFileFullPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "day13/input.txt");
    private record PatternBlock(List<string> Patterns);
    public static void SolvePart1()
    {
        var blocks = ReadInput();
        Console.WriteLine(blocks.Sum(b => Summerize(b, 0)));
    }
    public static void SolvePart2()
    {
        var blocks = ReadInput();
        Console.WriteLine(blocks.Sum(b => Summerize(b, 1)));
    }

    private static long Summerize(PatternBlock block, int sumdge)
    {
        for (var i = 0; i < block.Patterns.Count - 1; i++)
            if (HasLineOfReflectionForRows(block, i, i + 1, sumdge))
                return (100 * (i + 1));

        for (var i = 0; i < block.Patterns[0].Length - 1; i++)
            if (HasLineOfReflectionForColumns(block, i, i + 1, sumdge))
                return  (i + 1);
        return 0;
    }
    private static bool HasLineOfReflectionForColumns(PatternBlock block, int c1, int c2, int smudge)
    {
        int diffs = 0;
        for (int c = 0; c1 - c >= 0 && c2 + c < block.Patterns[0].Length; c++)
        {
            diffs += CompareDiffs(block.Patterns.Select(p => p[c1 - c]).ToList(), block.Patterns.Select(p => p[c2 + c]).ToList());
        }
        return diffs == smudge;
    }
    private static bool HasLineOfReflectionForRows(PatternBlock block, int r1, int r2, int smudge)
    {
        int diffs = 0;
        for (int r = 0; r1 - r >= 0 && r2 + r < block.Patterns.Count; r++)
        {
            diffs += CompareDiffs(block.Patterns[r1 - r].ToList(), block.Patterns[r2 + r].ToList());
        }
        return diffs == smudge;
    }
    private static int CompareDiffs(List<char> seq1, List<char> seq2) => seq1.Where((_, i) => seq1[i] != seq2[i]).Count();
    private static List<PatternBlock> ReadInput()
    {
        var blocks = new List<PatternBlock>();
        var currBlock = new PatternBlock(new List<string>());
        File.ReadAllLines(inputFileFullPath).ToList().ForEach(line =>
        {
            if (line.Length > 0) currBlock.Patterns.Add(line);
            else { blocks.Add(currBlock); currBlock = new PatternBlock(new List<string>()); }
        });
        blocks.Add(currBlock);

        return blocks;
    }
}
