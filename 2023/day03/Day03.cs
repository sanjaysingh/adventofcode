using System.Reflection;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public static class Day03
    {
        private static readonly string inputFileFullPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "day03/input.txt");
        private record DataLocation(string Text, int RowIndex, int ColIndex)
        {
            public bool IsAdjacent(DataLocation other)
            {
                return Math.Abs(this.RowIndex - other.RowIndex) <= 1 &&
                       this.ColIndex <= other.ColIndex + other.Text.Length &&
                       other.ColIndex <= this.ColIndex + this.Text.Length;
            }

            public int IntValue() => int.Parse(this.Text);
        }

        public static void SolvePart1()
        {
            var rows = ReadSchematic();
            var symbols = FindMatches(rows, new Regex(@"[^.0-9]"));
            var numbers = FindMatches(rows, new Regex(@"\d+"));

            var sum = numbers.Where(n => symbols.Any(s => n.IsAdjacent(s))).Select(n => n.IntValue()).Sum();

            Console.WriteLine(sum);
        }

        public static void SolvePart2()
        {
            var rows = ReadSchematic();
            var gears = FindMatches(rows, new Regex(@"\*"));
            var numbers = FindMatches(rows, new Regex(@"\d+"));

            var sum = gears
                .Select(g => {
                    var neighbors = numbers.Where(n => n.IsAdjacent(g));
                    if (neighbors.Count() == 2) {
                        return neighbors.First().IntValue() * neighbors.Last().IntValue();
                    }
                    return 0;
                })
                .Sum();
                

            Console.WriteLine(sum);
        }

        private static List<DataLocation> FindMatches(List<string> rows, Regex regex)
        {
            return rows.SelectMany((row, i) => regex.Matches(rows[i]).Select(x => new DataLocation(x.Value, i, x.Index))).ToList();
        }

        private static List<string> ReadSchematic()
        {
            return File.ReadAllLines(inputFileFullPath).ToList();
        }
    }
}
