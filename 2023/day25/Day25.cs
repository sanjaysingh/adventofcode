using System.Reflection;
using Vertex = string;
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

            if (graph.Edges.Count == 3)
            {
                Console.WriteLine(graph.Vertices[0].Split(",").Count() * graph.Vertices[1].Split(",").Count());
                break;
            }
        }
    }

    public static void SolvePart2()
    {

    }

    private static Graph kargerMinCut(Graph graph, Random r)
    {
       
        while (graph.Vertices.Count > 2)
        {
            int edgeIndex = r.Next(graph.Edges.Count);
            var removedEdge = graph.Edges[edgeIndex];
            graph.Edges.RemoveAt(edgeIndex);
            graph.Vertices.Remove(removedEdge.Vertices.First());
            graph.Vertices.Remove(removedEdge.Vertices.Last());
            var mergedNode = $"{removedEdge.Vertices.First()},{removedEdge.Vertices.Last()}";
            graph.Vertices.Add(mergedNode);
            int i = 0;
            while (i < graph.Edges.Count)
            {
                var e = graph.Edges[i];
                if (e.Vertices.Contains(removedEdge.Vertices.First()))
                {
                    e.Vertices.Remove(removedEdge.Vertices.First());
                    e.Vertices.Add(mergedNode);
                }
                if (e.Vertices.Contains(removedEdge.Vertices.Last()))
                {
                    e.Vertices.Remove(removedEdge.Vertices.Last());
                    e.Vertices.Add(mergedNode);
                }
                if (e.Vertices.Count == 1) graph.Edges.RemoveAt(i);
                else i++;
            }
        }

        return graph;
    }

    private static Graph ReadInput()
    {
        var vertices = new List<Vertex>();
        var edges = new List<Edge>();
        foreach (var line in File.ReadLines(inputFileFullPath))
        {
            var nodes = line.Split(':', ' ').Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            var parent = nodes[0];
            if (!vertices.Contains(parent)) vertices.Add(parent);
            foreach (var child in nodes.Skip(1))
            {
                if (!vertices.Contains(child)) vertices.Add(child);
                edges.Add(new Edge(new SortedSet<Vertex> { parent, child }));
            }
        }

        return new Graph(vertices, edges);
    }

    private record Graph(List<Vertex> Vertices, List<Edge> Edges);
    private record Edge(SortedSet<Vertex> Vertices);
}