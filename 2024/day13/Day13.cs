using MathNet.Numerics.LinearAlgebra;
using System.Text.RegularExpressions;
namespace AdventOfCode;
public static class Day13
{
    private static readonly string InputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day13", "input.txt");
    public static void SolvePart1() => Solve();
    public static void SolvePart2() => Solve(10000000000000);
    private  static void Solve(long prizeLocationOffset = 0)
    {
        long cost = ReadInput(prizeLocationOffset)
                    .Sum(m =>
                    {
                        var a = Matrix<double>.Build.DenseOfArray(new double[,]
                        {
                            { m.ButtonA.X, m.ButtonB.X},
                            { m.ButtonA.Y, m.ButtonB.Y},
                        });
                        var b = Vector<double>.Build.Dense(new double[] { m.PrizeLocation.X, m.PrizeLocation.Y });
                        var solution = a.Solve(b);

                        if (IsWholeNumber(solution[0]) && IsWholeNumber(solution[1]))
                            return ((long)Math.Round(solution[0]) * 3) + (long)Math.Round(solution[1]);
                        return 0;
                    });
        Console.WriteLine(cost.ToString());
    }
    private static bool IsWholeNumber(double value, double tolerance = 0.001) => Math.Abs(value - Math.Round(value)) < tolerance;
    private static List<ClawMachine> ReadInput(long prizeDelta = 0)
    {
        string pattern = @"[XY]\+?=?(\d+)";
        var machines = new List<ClawMachine>();
        foreach (var machineConf in File.ReadAllText(InputPath).Split("\r\n\r\n"))
        {
            var matches = Regex.Matches(machineConf, pattern);
            var buttonA = new ButtonBehavior(long.Parse(matches[0].Groups[1].Value), long.Parse(matches[1].Groups[1].Value));
            var buttonB = new ButtonBehavior(long.Parse(matches[2].Groups[1].Value), long.Parse(matches[3].Groups[1].Value));
            var prize = new PrizeLocation(long.Parse(matches[4].Groups[1].Value) + prizeDelta, long.Parse(matches[5].Groups[1].Value) + prizeDelta);
            machines.Add(new ClawMachine(buttonA, buttonB, prize));
        }
        return machines;
    }
    private record ButtonBehavior(long X, long Y);
    private record PrizeLocation(long X, long Y);
    private record ClawMachine(ButtonBehavior ButtonA, ButtonBehavior ButtonB, PrizeLocation PrizeLocation);
}