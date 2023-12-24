using System.Reflection;

namespace AdventOfCode
{
    public static class Day01
    {
        private static readonly string inputFileFullPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "day01/input.txt");

        public static void SolveFirstPart()
        {
            var sum = 0;
            foreach (var line in File.ReadAllLines(inputFileFullPath))
            {
                var firstDigit = line.First(c => char.IsDigit(c));
                var lastDigit = line.Last(c => char.IsDigit(c));
                sum += int.Parse(firstDigit.ToString() + lastDigit.ToString());
            }

            Console.WriteLine(sum);

        }

        public static void SolveSecondPart()
        {
            var sum = 0;
            var possibleNumbers = new Dictionary<string, string>
            {
                {"1",       "1"},
                {"2",       "2"},
                {"3",       "3"},
                {"4",       "4"},
                {"5",       "5"},
                {"6",       "6"},
                {"7",       "7"},
                {"8",       "8"},
                {"9",       "9"},
                {"one",     "1"},
                {"two",     "2"},
                {"three",   "3"},
                {"four",    "4"},
                {"five",    "5"},
                {"six",     "6"},
                {"seven",   "7"},
                {"eight",   "8"},
                {"nine",    "9"}
            };

            var findFirstAndLastOccurences = (KeyValuePair<string, string> numberNameAndValue, string line) =>
            {
                return new Tuple<int, string>[] { new Tuple<int, string>(line.IndexOf(numberNameAndValue.Key), numberNameAndValue.Value), new Tuple<int, string>(line.LastIndexOf(numberNameAndValue.Key), numberNameAndValue.Value) };
            };
            foreach (var line in File.ReadAllLines(inputFileFullPath))
            {
                var numIndexAndValues = possibleNumbers
                                .Where(numberNameValue => line.Contains(numberNameValue.Key))
                                .SelectMany(numberNameValue => findFirstAndLastOccurences(numberNameValue, line))
                                .OrderBy(x => x.Item1).ToList();

                sum += int.Parse(numIndexAndValues.First().Item2 + numIndexAndValues.Last().Item2);
            }

            Console.WriteLine(sum);
        }
    }
}
