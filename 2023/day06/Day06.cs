using System.Reflection;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public static class Day06
    {
        private static readonly string inputFileFullPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "day06/input.txt");
        private record Race(long Time, long Distance);
        public static void SolvePart1()
        {
            var result = ReadRaces()
                .Select(race => Enumerable.Range(1, (int)race.Time - 1).Where(chargeTime => ((race.Time - chargeTime) * chargeTime) > race.Distance).Count())
                .Aggregate((x, y) => x * y);
            Console.WriteLine(result);
        }
        public static void SolvePart2()
        {
            var races = ReadRaces();
            var raceTime = long.Parse(races.Select(x=>x.Time.ToString()).Aggregate((x,y) => x.ToString() + y.ToString()));
            var raceDistance = long.Parse(races.Select(x => x.Distance.ToString()).Aggregate((x, y) => x.ToString() + y.ToString()));
            var result = Enumerable
                        .Range(1, (int)raceTime - 1)
                        .Where(chargeTime => ((raceTime - chargeTime) * chargeTime) > raceDistance)
                        .Count();
            Console.WriteLine(result);
        }
        private static List<Race> ReadRaces()
        {
            var numbersRegex = new Regex(@"\d+");
            var races = new List<Race>();
            var lines = File.ReadAllLines(inputFileFullPath);
            var times = numbersRegex
                .Matches(lines[0])
                .Select(m => long.Parse(m.Value)).ToList();
            var distances = numbersRegex
                .Matches(lines[1])
                .Select(m => long.Parse(m.Value)).ToList();
            return times
                .Select((time, index) => new Race(time, distances[index]))
                .ToList();
        }
    }
}
