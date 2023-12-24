using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public static class Day02
    {
        private static readonly string inputFileFullPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "day02/input.txt");
        private record GameSet(int Red, int Green, int Blue);
        private record Game(int Id, List<GameSet> Sets);
       
        public static void SolvePart1()
        {
            var possibleGameIdSum = ReadGamesPlayed().Where(game =>
                !game.Sets.Any(gameset => gameset.Red > 12 || gameset.Green > 13 || gameset.Blue > 14)
            ).Sum(game => game.Id);


            Console.WriteLine(possibleGameIdSum);
        }
       
        public static void SolvePart2()
        {
            var powerSum = 0;
            foreach(var game in ReadGamesPlayed())
            {
                var minRedNeeded = game.Sets.Max(gameset => gameset.Red);
                var minGreenNeeded = game.Sets.Max(gameset => gameset.Green);
                var minBlueNeeded = game.Sets.Max(gameset => gameset.Blue);

                var power = Math.Max(1, minRedNeeded) * Math.Max(1, minGreenNeeded) * Math.Max(1, minBlueNeeded);
                powerSum += power;
            }

            Console.WriteLine(powerSum);
        }

        private static List<Game> ReadGamesPlayed()
        {
            var games = new List<Game>();
            foreach (var line in File.ReadAllLines(inputFileFullPath))
            {
                var gameAndSets = line.Split(":", StringSplitOptions.RemoveEmptyEntries);
                var gameId = int.Parse(gameAndSets[0].Replace("Game ", "").Trim());
                var gameSets = new List<GameSet>();
                foreach(var gameSet in gameAndSets[1].Split(";", StringSplitOptions.RemoveEmptyEntries))
                {
                    int red = 0, green = 0, blue = 0;
                    foreach(var cubeInfo in gameSet.Split(",", StringSplitOptions.RemoveEmptyEntries))
                    {
                        var cubeCountAndColor = cubeInfo.Trim().Split(" ");
                        var cubeCount = cubeCountAndColor.First();
                        var cubeColor = cubeCountAndColor.Last();
                        switch (cubeColor)
                        {
                            case "red":
                                red = int.Parse(cubeCount);
                                break;
                            case "green":
                                green = int.Parse(cubeCount);
                                break;
                            case "blue":
                                blue = int.Parse(cubeCount);
                                break;
                        }
                    }
                    gameSets.Add(new GameSet(red, green, blue));
                }

                games.Add(new Game(gameId, gameSets));
            }

            return games;
        }
    }
}
