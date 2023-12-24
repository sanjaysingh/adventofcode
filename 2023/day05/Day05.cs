using Microsoft.VisualBasic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public static class Day05
    {
        private static readonly string inputFileFullPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "day05/input.txt");
        private record AlmanacMap(long Destinsion, long Source, int Length, int Kind);

        private record AlmanacMatch(long Start, long Length);

        private record Almanac(List<long> Seeds, List<AlmanacMap> Mappings)
        {
            public long FindEndMapping(long source, int kind = 1)
            {
                if (!Mappings.Any(x => x.Kind == kind))
                {
                    return source;
                }

                long mappedValue = source;
                foreach (var map in Mappings.Where(m => m.Kind == kind))
                {
                    if (source >= map.Source && source < map.Source + map.Length)
                    {
                        mappedValue = map.Destinsion + (source - map.Source);
                        break;
                    }
                }

                return FindEndMapping(mappedValue, kind + 1);
            }
        }

        public static void SolvePart1()
        {
            var almanac = ReadAlmanac();
            var min = almanac
                        .Seeds
                        .Select(x => almanac.FindEndMapping(x))
                        .Min();
            Console.WriteLine(min);
        }

        public static void SolvePart2()
        {
            // TODO
        }



        private static Almanac ReadAlmanac()
        {
            var numbersRegex = new Regex(@"\d+");

            int kind = 0;
            List<long> seeds = null;
            List<AlmanacMap> mapping = new List<AlmanacMap>();
            foreach (var line in File.ReadAllLines(inputFileFullPath))
            {
                if (line.StartsWith("seeds:"))
                {
                    seeds = numbersRegex.Matches(line).Select(x => long.Parse(x.Value)).ToList();
                }
                else if (line.Contains("map"))
                {
                    kind++;
                    continue;
                }
                else if (line.Trim().Length < 1)
                {
                    continue;
                }
                else
                {
                    var values = numbersRegex.Matches(line).Select(x => long.Parse(x.Value)).ToList();
                    mapping.Add(new AlmanacMap(values[0], values[1], (int)values[2], kind));
                }
            }

            return new Almanac(seeds, mapping);
        }
    }
}
