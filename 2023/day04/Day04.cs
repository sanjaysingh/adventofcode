using Microsoft.VisualBasic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public static class Day04
    {
        private static readonly string inputFileFullPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "day04/input.txt");
        private record Card (int Id, IEnumerable<int> WinningNumbers, IEnumerable<int> PlayerNumbers)
        {
            public int Point { get; init; } = (int) Math.Floor(Math.Pow(2, WinningNumbers.Intersect(PlayerNumbers).Count()-1));
            public int WinningMatchCount { get; init; } = WinningNumbers.Intersect(PlayerNumbers).Count();
        }

        public static void SolvePart1()
        {
            var sum = ReadCards()
                     .Select(card => card.Point)
                     .Sum();
            Console.WriteLine(sum);
        }

        public static void SolvePart2()
        {
            var cards = ReadCards();
            var countCache = new Dictionary<int, int>();

            var sum = cards
                        .Select((card, index )=> CountScratchCards(cards, index, countCache))
                        .Sum();

            Console.WriteLine(sum);
        }

        private static int CountScratchCards(List<Card> cards, int cardIndex, Dictionary<int, int> countCache)
        {
            if (cardIndex >= cards.Count())
                return 0;

            if(countCache.ContainsKey(cardIndex)) 
                return countCache[cardIndex];

            int count = 1;
            for(var i = 0; i < cards[cardIndex].WinningMatchCount; i++)
            {
                count += CountScratchCards(cards, cardIndex + i + 1, countCache);
            }

            countCache[cardIndex] = count;
            return count;
        }

        private static List<Card> ReadCards()
        {
            var numbersRegex = new Regex(@"\d+");

            return File.ReadAllLines(inputFileFullPath)
                .Select(line => {
                    var parts = line.Split("|");
                    var firstPartNumbers = numbersRegex.Matches(parts.First());
                    var cardId = int.Parse(firstPartNumbers.First().Value);
                    var winningNumbers = firstPartNumbers.Skip(1).Select(x => int.Parse(x.Value));
                    var secondPartNumbers = numbersRegex.Matches(parts.Last());
                    var playerNumbers = secondPartNumbers.Select(x => int.Parse(x.Value));
                    return new Card(cardId, winningNumbers, playerNumbers);
                }).ToList();
        }
    }
}
