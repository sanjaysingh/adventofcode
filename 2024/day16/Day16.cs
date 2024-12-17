namespace AdventOfCode;

public static class Day16
{
    private static readonly string InputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day16", "input.txt");
    private static readonly (int dx, int dy)[] Directions = { (-1, 0), (0, 1), (1, 0), (0, -1) }; // Direction vectors for Up, Right, Down, Left
    private const int East = 1;

    public static void SolvePart1()
    {
        var maze = ReadInput();
        var score = FindShortestPath(maze);
        Console.WriteLine(score);
    }

    public static void SolvePart2()
    {
        var maze = ReadInput();
        var uniqueCells = FindUnionCellsInMinCostPaths(maze);
        Console.WriteLine(uniqueCells);
    }

    public static int FindUnionCellsInMinCostPaths(char[][] maze)
    {
        var (startRow, startCol, endRow, endCol) = FindPoints(maze);
        var minCostPaths = new List<List<(int Row, int Col)>>();
        var minCost = int.MaxValue;

        var pq = new PriorityQueue<State, int>();
        var costAtState = new Dictionary<(int Row, int Col, int Direction), int>();

        var initialPath = new List<(int Row, int Col)> { (startRow, startCol) };
        pq.Enqueue(new State(startRow, startCol, East, 0, initialPath), 0);

        while (pq.TryDequeue(out var current, out _))
        {
            var stateKey = (current.Row, current.Col, current.Direction);

            // Skip if we've seen this state with a better or equal cost
            if (costAtState.TryGetValue(stateKey, out var prevCost) && prevCost < current.Cost)
                continue;

            costAtState[stateKey] = current.Cost;

            if (current.Row == endRow && current.Col == endCol)
            {
                if (current.Cost <= minCost)
                {
                    if (current.Cost < minCost)
                    {
                        minCost = current.Cost;
                        minCostPaths.Clear();
                    }
                    minCostPaths.Add(current.Path);
                }
                continue;
            }

            foreach (int newDir in GetValidDirections(current.Direction))
            {
                var (dx, dy) = Directions[newDir];
                int newRow = current.Row + dx;
                int newCol = current.Col + dy;

                if (maze[newRow][newCol] == '#')
                    continue;

                var newCost = current.Cost + (newDir == current.Direction ? 1 : 1001);

                // Don't prune paths if they might lead to an equal-cost solution
                if (minCost != int.MaxValue && newCost > minCost)
                    continue;

                var newPath = new List<(int Row, int Col)>(current.Path) { (newRow, newCol) };
                pq.Enqueue(new State(newRow, newCol, newDir, newCost, newPath), newCost);
            }
        }

        // Find union of all cells in minimum cost paths
        var unionCells = new HashSet<(int Row, int Col)>();
        foreach (var path in minCostPaths)
        {
            foreach (var cell in path)
            {
                unionCells.Add(cell);
            }
        }

        return unionCells.Count;
    }

    private static IEnumerable<int> GetValidDirections(int currentDir)
    {
        int oppositeDir = (currentDir + 2) % 4;
        for (int dir = 0; dir < 4; dir++)
        {
            if (dir != oppositeDir)
                yield return dir;
        }
    }

    public static int FindShortestPath(char[][] maze)
    {
        var (startRow, startCol, endRow, endCol) = FindPoints(maze);

        var pq = new PriorityQueue<State, int>();
        var visited = new HashSet<State>();

        pq.Enqueue(new State(startRow, startCol, East, 0, new List<(int Row, int Col)>()), 0);

        while (pq.TryDequeue(out var current, out _))
        {
            // Skip if we've already found a better path to this position+direction
            if (!visited.Add(current))
                continue;

            if (current.Row == endRow && current.Col == endCol)
                return current.Cost;

            int oppositeDir = (current.Direction + 2) % 4;

            for (int newDir = 0; newDir < 4; newDir++)
            {
                if (newDir == oppositeDir)
                    continue;

                var (dx, dy) = Directions[newDir];
                int newRow = current.Row + dx;
                int newCol = current.Col + dy;

                if (maze[newRow][newCol] == '#')
                    continue;

                var newState = new State(newRow, newCol, newDir,
                    current.Cost + (newDir == current.Direction ? 1 : 1001), new List<(int Row, int Col)>());

                pq.Enqueue(newState, newState.Cost);
            }
        }

        return -1;
    }

    private static (int startRow, int startCol, int endRow, int endCol) FindPoints(char[][] maze)
    {
        int startRow = 0, startCol = 0, endRow = 0, endCol = 0;

        for (int i = 0; i < maze.Length; i++)
            for (int j = 0; j < maze[0].Length; j++)
                switch (maze[i][j])
                {
                    case 'S': (startRow, startCol) = (i, j); break;
                    case 'E': (endRow, endCol) = (i, j); break;
                }

        return (startRow, startCol, endRow, endCol);
    }
    private record State(int Row, int Col, int Direction, int Cost, List<(int Row, int Col)> Path) : IComparable<State>
    {
        public int CompareTo(State? other) => Cost.CompareTo(other?.Cost);
        public virtual bool Equals(State? other) =>
            other is not null && Row == other.Row && Col == other.Col && Direction == other.Direction;
        public override int GetHashCode() => HashCode.Combine(Row, Col, Direction);
    }

    private static char[][] ReadInput() => File.ReadAllLines(InputPath).Select(r => r.ToCharArray()).ToArray();
}