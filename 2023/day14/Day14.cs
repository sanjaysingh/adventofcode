using System.Reflection;
namespace AdventOfCode;
using Map = List<List<char>>;
public static class Day14
{
    private static readonly string inputFileFullPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "day14/input.txt");
    public static void SolvePart1()
    {
        var map = ReadInput();
        long sum = map.TiltNorth().Sum();
        Console.WriteLine(sum);
    }
    public static void SolvePart2()
    {
        var map = ReadInput();
        var cycles = new Dictionary<int, int>();
        int totalCycles = 1000000000, remaingCycles = -1;
        for (int cycle = 1; cycle <= totalCycles; cycle++)
        {
            map.RunCycle();
            var key = map.Hash();
            if (cycles.ContainsKey(key))
            {
                remaingCycles = (totalCycles - cycle) % (cycle - cycles[key]);
                while (remaingCycles-- > 0)
                    RunCycle(map);
                break;
            }
            else
            {
                cycles.Add(key, cycle);
            }
        }
        Console.WriteLine(map.Sum());
    }

    private static int Hash(this Map map) => map.Select(r => string.Join(",", r)).Aggregate((r1, r2) => $"{r1}|{r2}").GetHashCode();
    private static long Sum(this Map map)
    {
        long sum = 0;
        for (int r = 0, m = map.Count; r < map.Count; r++, m--)
        {
            sum += (map[r].Count(x => x == 'O') * m);
        }
        return sum;
    }
    private static Map RunCycle(this Map map) => map.TiltNorth().TiltWest().TiltSouth().TiltEast();
    private static Map Tilt(this Map map, int rIndex, int rIncrement, int cIndex, int cIncrement, int rTilt, int cTilt)
    {
        int r = rIndex;
        while (r >= 0 && r < map.Count)
        {
            int c = cIndex;
            while (c >= 0 && c < map[0].Count)
            {
                ShiftRock(r, c, rTilt, cTilt, map);
                c += cIncrement;
            }
            r += rIncrement;
        }

        return map;
    }
    private static Map TiltNorth(this Map map) => Tilt(map, 0, 1, 0, 1, -1, 0);
    private static Map TiltWest(this Map map) => Tilt(map, 0, 1, 0, 1, 0, -1);
    private static Map TiltSouth(this Map map) => Tilt(map, map.Count - 1, -1, 0, 1, 1, 0);
    private static Map TiltEast(this Map map) => Tilt(map, 0, 1, map[0].Count - 1, -1, 0, 1);
    private static void ShiftRock(int r, int c, int shiftr, int shiftc, Map map)
    {
        var nextr = r + shiftr;
        var nextc = c + shiftc;
        if (r < 0 || r >= map.Count || nextr < 0 || nextr >= map.Count || c < 0 || c >= map[0].Count || nextc < 0 || nextc >= map[0].Count) return;
        if (map[nextr][nextc] == '.' && map[r][c] == 'O')
        {
            map[nextr][nextc] = 'O';
            map[r][c] = '.';
            ShiftRock(nextr, nextc, shiftr, shiftc, map);
        }
    }
    private static Map ReadInput() => File.ReadAllLines(inputFileFullPath).Select(x => x.ToList()).ToList();
}
