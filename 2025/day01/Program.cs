using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        SolvePart1();
        SolvePart2();
    }

    static void SolvePart1()
    {
        const int ringSize = 100;
        long position = 50;
        long touchesOfZero = 0;

        foreach (var (dir, steps) in ReadMoves("input.txt"))
        {
            position = dir == 'R'
                ? (position + steps) % ringSize
                : (ringSize + ((position - steps) % ringSize)) % ringSize;

            if (position == 0) touchesOfZero++;
        }

        Console.WriteLine(touchesOfZero);
    }

    private static void SolvePart2()
    {
        const int ringSize = 100;
        long position = 50;
        long wraps = 0;

        foreach (var (dir, steps) in ReadMoves("input.txt"))
        {
            if (dir == 'R')
            {
                var next = position + steps;
                wraps += next / ringSize;
                position = next % ringSize;
            }
            else
            {
                if (position == 0)
                {
                    wraps += steps / ringSize;
                }
                else
                {
                    wraps += (steps + (ringSize - position)) / ringSize;
                }

                position = (position - (steps % ringSize) + ringSize) % ringSize;
            }
        }

        Console.WriteLine(wraps);
    }

    private static IEnumerable<(char dir, long steps)> ReadMoves(string fileName)
    {
        var path = Path.Combine(AppContext.BaseDirectory, fileName);
        foreach (var line in File.ReadLines(path))
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            var dir = line[0];
            var steps = long.Parse(line.AsSpan(1));
            yield return (dir, steps);
        }
    }
}
