namespace AdventOfCode;

public static class Day18
{
    private static readonly string InputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day18", "input.txt");

    public static void SolvePart1()
    {
        var bytePositions = ReadInput();
        var grid = InitGrid();

        foreach (var p in bytePositions.Take(1024))
        {
            grid[p[1], p[0]] = '#';
        }

        int shortestPath = FindShortestPath(grid);
        Console.WriteLine(shortestPath);
    }
    public static void SolvePart2()
    {
        var bytePositions = ReadInput();
        var grid = InitGrid();
        foreach (var p in bytePositions)
        {
            grid[p[1], p[0]] = '#';
            if (FindShortestPath(grid) < 0)
            {
                Console.WriteLine($"{p[1]},{p[0]}");
                break;
            }
        }
    }
    private static char[,] InitGrid()
    {
        int rows = 71, cols = 71;
        var grid = new char[rows, cols];
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
                grid[i, j] = '.';
        return grid;
    }
    public static int FindShortestPath(char[,] grid)
    {
        int n = grid.GetLength(0);

        var dirs = new[] { (-1, 0), (1, 0), (0, -1), (0, 1) };
        var queue = new Queue<(int x, int y, int steps)>();
        var visited = new bool[n, n];

        queue.Enqueue((0, 0, 0));
        visited[0, 0] = true;

        while (queue.Count > 0)
        {
            var (x, y, steps) = queue.Dequeue();
            if (x == n - 1 && y == n - 1) return steps;

            foreach (var (dx, dy) in dirs)
            {
                int newX = x + dx, newY = y + dy;
                if (newX >= 0 && newX < n && newY >= 0 && newY < n &&
                    !visited[newX, newY] && grid[newX, newY] == '.')
                {
                    visited[newX, newY] = true;
                    queue.Enqueue((newX, newY, steps + 1));
                }
            }
        }
        return -1;
    }
    private static int[][] ReadInput() =>
        File.ReadAllLines(InputPath).Select(line => line.Split(",").Select(x => int.Parse(x.Trim())).ToArray()).ToArray();
}