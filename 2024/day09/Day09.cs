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
        int startIndex = 0, endIndex = blocks.Count - 1;
        while (startIndex < endIndex)
        {
            while (blocks[startIndex] != EmptyBlockId) startIndex++;
            while (blocks[endIndex] == EmptyBlockId) endIndex--;
            if (startIndex >= endIndex) break;
            blocks[startIndex] = blocks[endIndex];
            blocks[endIndex] = EmptyBlockId;
        }
        var checkSum = blocks.Select((x, i) => x == EmptyBlockId ? 0 : x * i).Sum();

        Console.WriteLine(checkSum);
    }
    public static void SolvePart2()
    {
        var blocks = new List<Block>();
        var diskMap = ReadInput().Select(x => int.Parse(x.ToString())).ToList();
        long fileId = 0;
        var isFileBlock = true;
        const int EmptyBlockId = -1;
        for (int i = 0; i < diskMap.Count; i++)
        {
            var blockid = isFileBlock ? fileId++ : EmptyBlockId;
            blocks.Add(new Block(blockid, diskMap[i]));
            isFileBlock = !isFileBlock;
        }

        var lastIndex = blocks.Count - 1;
        while (lastIndex > 0)
        {
            while (lastIndex >= 0 && blocks[lastIndex].IsEmpty) lastIndex--;
            if (lastIndex < 0) break;
            var blockToMove = blocks[lastIndex];
            var emptyBlockIndex = blocks.FindIndex(b => b.IsEmpty && b.CanFit(blockToMove));
            if (emptyBlockIndex != -1 && emptyBlockIndex < lastIndex)
            {
                var emptyBlock = blocks[emptyBlockIndex];
                var newBlock = emptyBlock.Allocate(blockToMove);
                blocks.Insert(emptyBlockIndex, newBlock);
            }
            lastIndex--;
        }

        var blocksIds = new List<long>();
        for (int i = 0; i < blocks.Count; i++)
        {
            var id = blocks[i].IsEmpty ? 0 : blocks[i].FileID;
            Enumerable.Range(1, blocks[i].Length).ToList().ForEach(_ => blocksIds.Add(id));
        }
        var checkSum = blocksIds.Select((x, i) => x * i).Sum();

        Console.WriteLine(checkSum);
    }

    private static string ReadInput() => File.ReadAllText(InputPath);

    private record Block(long FileID, int Length)
    {
        public long FileID { get; set; } = FileID;
        public bool IsEmpty => FileID == -1;
        public int Length { get; set; } = Length;
        public Block Allocate(Block b)
        {
            this.Length -= b.Length;
            var newBlock = new Block(b.FileID, b.Length);
            b.Empty();
            return newBlock;
        }

        public bool CanFit(Block b) => this.Length >= b.Length;
        public void Empty() => this.FileID = -1;
    }
}