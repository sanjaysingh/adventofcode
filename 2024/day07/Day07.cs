namespace AdventOfCode;

public static class Day07
{
    private static readonly string InputPath =  Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day07", "input.txt");

    public static void SolvePart1()
    {
        var equations = ReadInput();
        long result = 0;
        foreach (var equation in equations)
        {
            var operatorsList = Permute("+*", equation.Numbers.Count - 1, "", new List<string>());
            if (operatorsList.Any(o => equation.TestValue == SolveEquation(equation, o)))
                result += equation.TestValue;

        }

        Console.WriteLine(result);
    }

    public static void SolvePart2()
    {
        var equations = ReadInput();
        long result = 0;
        foreach (var equation in equations)
        {
            var operatorsList = Permute("+*|", equation.Numbers.Count - 1, "", new List<string>());
            if (operatorsList.Any(o => equation.TestValue == SolveEquation(equation, o)))
                result += equation.TestValue;

        }

        Console.WriteLine(result);
    }

    private static long SolveEquation(Equation equation, string operators)
    {
        int currOper = 0;
        long result = equation.Numbers.First();
        foreach (var number in equation.Numbers.Skip(1))
        {
            switch (operators[currOper])
            {
                case '+':
                    result += number; break;
                case '*':
                    result *= number; break;
                default:
                    result = long.Parse($"{result}{number}");
                    break;

            }
            currOper++;
        }
        return result;
    }

    private static List<Equation> ReadInput()
    {
        var inputs = new List<Equation>();
        foreach (var line in File.ReadLines(InputPath))
        {
            var parts = line.Split(':', StringSplitOptions.RemoveEmptyEntries);
            inputs.Add(new Equation(long.Parse(parts[0].Trim()), parts[1].Trim().Split(" ").Select(long.Parse).ToList()));
        }
        return inputs;
    }

    private static List<string> Permute(string input, int length, string currPermuation, List<string> permutations)
    {
        if (currPermuation.Length == length)
        {
            permutations.Add(currPermuation);
            return permutations;
        }

        foreach (var item in input)
        {
            Permute(input, length, currPermuation + item, permutations);
        }

        return permutations;
    }

    private record Equation(long TestValue, List<long> Numbers);
}