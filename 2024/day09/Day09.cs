using System.Text;

namespace AdventOfCode;

public static class Day09
{
    private static readonly string InputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day09", "input.txt");
    public static void SolvePart1() 
    {
        var blocks = new List<long>();
        var diskMap = ReadInput().Select(x => int.Parse(x.ToString())).ToList();
        long fileId = 0;
        var isFileBlock = true;
        const int EmptyBlockId = -1;
        for (int i = 0; i < diskMap.Count; i++)
        {
            var blockid = isFileBlock ? fileId++ : EmptyBlockId;
            blocks.AddRange(Enumerable.Range(1, diskMap[i]).Select(_ => blockid));
            isFileBlock = !isFileBlock;
        }
        int startIndex = 0, endIndex = blocks.Count-1;
        while (startIndex < endIndex)
        {
            while (blocks[startIndex] != EmptyBlockId) startIndex++;
            while (blocks[endIndex] == EmptyBlockId) endIndex--;
            if(startIndex >= endIndex) break;
            blocks[startIndex] = blocks[endIndex];
            blocks[endIndex] = EmptyBlockId;
        }
        var checkSum  = blocks.Select((x, i) => x == EmptyBlockId ? 0 : x * i).Sum();

        Console.WriteLine(checkSum);
    }
    public static void SolvePart2() 
    { 
    }
    private static string ReadInput() => File.ReadAllText(InputPath);
}