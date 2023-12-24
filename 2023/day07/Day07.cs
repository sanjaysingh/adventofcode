using System.Reflection;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public static class Day07
    {
        private static readonly string inputFileFullPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "day07/input.txt");
        private record HandBid(string Hand, long Bid);
        private static string CardRanksPart1 ="23456789TJQKA";
        private static string CardRanksPart2 = "J23456789TQKA";
        public static void SolvePart1()
        {
            var result = ReadHandBids()
                            .OrderBy(x => 
                            {
                                var dict = new Dictionary<char, long>();
                                x.Hand.ToList().ForEach(c =>
                                {
                                    if (!dict.ContainsKey(c))
                                        dict[c] = 0;
                                    dict[c]++;
                                });
                                var rank = dict.Sum(v => (long)Math.Pow(v.Value, 2));
                                return rank;
                            })
                            .ThenBy(x => new string(x.Hand.Select(x => (char)('A' + CardRanksPart1.IndexOf(x))).ToArray()))
                            .Select((x, i) => (i + 1) * x.Bid)
                            .Sum();
            Console.WriteLine(result);
        }
        public static void SolvePart2()
        {
            var result = ReadHandBids()
                           .OrderBy(x =>
                           {
                               var dict = new Dictionary<char, long>();
                               x.Hand.ToList().ForEach(c =>
                               {
                                   if (!dict.ContainsKey(c))
                                       dict[c] = 0;
                                   dict[c]++;
                               });
                               if (dict.ContainsKey('J') && dict.Count() > 1)
                               {
                                   var jCount = dict['J'];
                                   dict.Remove('J');
                                   dict[dict.MaxBy(x => x.Value).Key] += jCount;
                               }
                               var rank = dict.Sum(v => (long)Math.Pow(v.Value, 2));
                               return rank;
                           })
                           .ThenBy(x => new string(x.Hand.Select(x => (char)('A' + CardRanksPart2.IndexOf(x))).ToArray()))
                           .Select((x, i) => (i + 1) * x.Bid)
                           .Sum();
            Console.WriteLine(result);
        }
        private static List<HandBid> ReadHandBids()
        {
           return  File
                .ReadAllLines(inputFileFullPath)
                .Select(line =>
                {
                    var parts = line.Split(" ");
                    return new HandBid(parts[0], long.Parse(parts[1]));
                })
                .ToList();
        }
    }
}
