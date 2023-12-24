using System.Reflection;
namespace AdventOfCode;
public static class Day15
{
    private static readonly string inputFileFullPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "day15/input.txt");
    private record Lens(string Label, int FocalLength);
    private record Box(int BoxNumber, List<Lens> Lenses);
    public static void SolvePart1()
    {
        var initSequence = ReadInput();
        Console.WriteLine(initSequence.Select(seq => seq.Hash()).Sum());
    }
    public static void SolvePart2()
    {
        var initSequence = ReadInput();
        var boxes = new Dictionary<int, Box>();
        Enumerable.Range(0, 256).ToList().ForEach(i => boxes.Add(i, new Box(i + 1, new List<Lens>())));
        foreach (var seq in initSequence)
        {
            var lensLabel = "";
            if (seq.EndsWith('-'))
            {
                lensLabel = seq.Substring(0, seq.Length - 1);
                boxes[lensLabel.Hash()].Lenses.RemoveAll(l => l.Label == lensLabel);
                continue;
            }

            var focalLength = int.Parse(seq.Last().ToString());
            lensLabel = seq.Substring(0, seq.Length - 2);
            var boxIndex = lensLabel.Hash();
            var lenIndex = boxes[boxIndex].Lenses.FindIndex(l => l.Label == lensLabel);
            if (lenIndex >= 0)
                boxes[boxIndex].Lenses[lenIndex] = new Lens(boxes[boxIndex].Lenses[lenIndex].Label, focalLength);
            else
                boxes[boxIndex].Lenses.Add(new Lens(lensLabel, focalLength));
        }
        var totalFocusingPower = 0;
        foreach (var box in boxes.Values)
        {
            totalFocusingPower += Enumerable.Range(0, box.Lenses.Count).Sum(lensIndex => box.BoxNumber * (lensIndex + 1) * box.Lenses[lensIndex].FocalLength);
        }
        Console.WriteLine(totalFocusingPower);
    }
    private static int Hash(this string seqStep) => seqStep.Aggregate(0, (curent, ch) => ((curent + ch) * 17) % 256);
    private static List<string> ReadInput() => File.ReadAllLines(inputFileFullPath).First().Split(",").ToList();
}
