using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public static class Day09
    {
        private static readonly string inputFileFullPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "day09/input.txt");
        private static long FindFirst(List<long> values)
        {
            if (values.All(v => v == 0))
                return 0;

            var reducedList = new List<long>();
            for (var i = 0; i < values.Count - 1; i++)
            {
                reducedList.Add(values[i + 1] - values[i]);
            }

            return values.First() - FindFirst(reducedList);
        }

        private static long FindLast(List<long> values)
        {
            if (values.All(v => v == 0))
                return 0;

            var reducedList = new List<long>();
            for (var i = 0; i < values.Count - 1; i++)
            {
                reducedList.Add(values[i + 1] - values[i]);
            }

            return values.Last() + FindLast(reducedList);
        }

        public static void SolvePart1()
        {
            var result = ReadInput().Select(x => FindLast(x)).Sum();
            Console.WriteLine(result);
        }

        public static void SolvePart2()
        {
            var result = ReadInput().Select(x => FindFirst(x)).Sum();
            Console.WriteLine(result);
        }

        private static List<List<long>> ReadInput()
        {
            return File.ReadAllLines(inputFileFullPath).Select(line => line.Split(" ").Select(long.Parse).ToList()).ToList();
        }
    }
}
