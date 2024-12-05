using System;
using System.Reflection;
using System.Text.RegularExpressions;
namespace AdventOfCode;

public static class Day04
{
    private static readonly string InputPath = Path.Combine(
        Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ?? string.Empty,
        "day04/input.txt"
    );

    public static void SolvePart1()
    {
        Console.WriteLine(GetAllWords(ReadWordMatrix())
            .Sum(word => GetBidirctionalMatches(word, "XMAS").Count()));
    }
    public static void SolvePart2() 
    {
        var matchCenters = GetDiagonalWords(ReadWordMatrix())
            .SelectMany(word => GetBidirctionalMatches(word, "MAS")
                .Select(match => word[match.Index + 1].Coord))
            .GroupBy(coord => coord)
            .ToDictionary(g => g.Key, g => g.Count());

        Console.WriteLine(matchCenters.Count(x => x.Value == 2));
    }

    private static List<List<Symbol>> GetAllWords(List<List<Symbol>> matrix)
    {
        var words = new List<List<Symbol>>();
        words.AddRange(matrix.ToArray());
        words.AddRange(Enumerable.Range(0, matrix.Count).Select(c => Enumerable.Range(0, matrix.Count).Select(r => matrix[r][c]).ToList()));
        words.AddRange(GetDiagonalWords(matrix).ToArray());  
        return words;
    }

    private static List<List<Symbol>> GetDiagonalWords(List<List<Symbol>> matrix)
    {
        var words = new List<List<Symbol>>();
        
        // count backward diagonal 
        for (int col = 0; col < matrix.Count; col++)
        {
            int startRow = 0, startCol = col;
            var symbols = new List<Symbol>();
            while (startRow < matrix.Count && startCol < matrix[startRow].Count)
                symbols.Add(matrix[startRow++][startCol++]);
            words.Add(symbols);
        }
        for (int row = 1; row < matrix.Count; row++)
        {
            int startRow = row, startCol = 0;
            var symbols = new List<Symbol>();
            while (startRow < matrix.Count && startCol < matrix[startRow].Count)
                symbols.Add(matrix[startRow++][startCol++]);
            words.Add(symbols);
        }

        // count forward diagonal
        for (int col = matrix.Count - 1; col >= 0; col--)
        {
            int startRow = 0, startCol = col;
            var symbols = new List<Symbol>();
            while (startRow < matrix.Count && startCol >= 0)
                symbols.Add(matrix[startRow++][startCol--]);
            words.Add(symbols);
        }
        for (int row = 1; row < matrix.Count; row++)
        {
            int startRow = row, startCol = matrix.Count - 1;
            var symbols = new List<Symbol>();
            while (startRow < matrix.Count && startCol >= 0)
                symbols.Add(matrix[startRow++][startCol--]);
            words.Add(symbols);
        }

        return words;
    }

    private record MatchInfo(string Value, int Index);

    private record Symbol(char Value, int X, int Y)
    {
        public string Coord => $"{X},{Y}";
    }

    private static List<MatchInfo> GetBidirctionalMatches(List<Symbol> symbols, string word) =>
        Regex.Matches(string.Concat(symbols.Select(s => s.Value)), $"(?={word}|{string.Concat(word.Reverse())})").Select(m => new MatchInfo(m.Value, m.Index)).ToList();

    private static List<List<Symbol>> ReadWordMatrix() =>
        File.ReadAllLines(InputPath).Select((line, x) => line.ToList().Select((val, y) => new Symbol(val, x, y)).ToList()).ToList();
}