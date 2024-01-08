using System.Reflection;
using Graph = System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>;
namespace AdventOfCode;

public static class Day25
{
    private static readonly string inputFileFullPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "day25/input.txt");

    public static void SolvePart1()
    {
        Random r = new Random(Environment.TickCount);
        while (true)
        {
            var graph = ReadInput();
            graph = kargerMinCut(graph, r);

            if (graph.First().Value.Count == 3)
            {
                Console.WriteLine(graph.First().Key.Split(",").Count() * graph.Last().Key.Split(",").Count());
                break;
            }
        }
    }

    private static Graph kargerMinCut(Graph graph, Random r)
    {
        while (graph.Count > 2)
        {
            var u = graph.Keys.ElementAt(r.Next(graph.Count));
            var v = graph[u].ElementAt(r.Next(graph[u].Count));
            var mergedNode = $"{u},{v}";
            graph[mergedNode] = [.. graph[u].Where(n => n != v), .. graph[v].Where(n => n != u)];
            graph.Remove(u);
            graph.Remove(v);

            foreach (var n2 in graph[mergedNode])
            {
                while (graph[n2].Remove(u)) graph[n2].Add(mergedNode);
                while (graph[n2].Remove(v)) graph[n2].Add(mergedNode);
            }
        }

        return graph;
    }

    private static Graph ReadInput()
    {
        var graph = new Graph();
        foreach (var line in File.ReadLines(inputFileFullPath))
        {
            var nodes = line.Split(':', ' ').Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            var parent = nodes[0];
            if (!graph.ContainsKey(parent)) graph.Add(parent, new List<string>());
            foreach (var child in nodes.Skip(1))
            {
                if (!graph.ContainsKey(child)) graph.Add(child, new List<string>());
                graph[parent].Add(child);
                graph[child].Add(parent);
            }
        }

        return graph;
    }
}