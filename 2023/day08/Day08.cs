using System.Reflection;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public static class Day08
    {
        private static readonly string inputFileFullPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "day08/input.txt");
        private record Node(string Val, List<Node> Children);
        private static long Gcd(long n1, long n2) => n2 == 0 ? n1 : Gcd(n2, n1 % n2);
        private static long Lcm(List<long> nums) => nums.Aggregate((x, y) => x * y / Gcd(x, y));
        public static void SolvePart1()
        {
            var tuple = ReadInput();
            var instructions = tuple.Item2;
            var currNode = tuple.Item1.First(curr => curr.Val == "AAA");
            int count = 0;
            for (var nextInstruction = 0; currNode.Val != "ZZZ"; nextInstruction = (nextInstruction + 1) % instructions.Count(), count++)
            {
                currNode = currNode.Children[instructions[nextInstruction]];
            }
            Console.WriteLine(count);
        }

        public static void SolvePart2()
        {
            var tuple = ReadInput();
            var nodes = tuple.Item1;
            var instructions = tuple.Item2;
            var aNodes = nodes.Where(curr => curr.Val.EndsWith("A")).ToList();

            var result = Lcm(aNodes
                 .Select(aNode =>
                 {
                     long count = 0;
                     var nextNode = aNode;
                     for(var nextInstruction = 0; !nextNode.Val.EndsWith("Z"); nextInstruction = (nextInstruction + 1) % instructions.Count(), count++)
                     {
                         nextNode = nextNode.Children[instructions[nextInstruction]];
                     }
                     return count;
                 }).ToList());

            Console.WriteLine(result);
        }

        private static Tuple<List<Node>, List<int>> ReadInput()
        {
            var lines = File.ReadAllLines(inputFileFullPath).ToList();
            var instruction = lines.First().Replace('L', '0').Replace('R', '1').Select(c => int.Parse(c.ToString())).ToList();
            var nodesRegex = new Regex(@"[0-9A-Z]{3}");
            var nodes = new Dictionary<string, Node>();
            lines.Skip(2).ToList().ForEach(line =>
            {
                var currNodes = nodesRegex.Matches(line).Select(m => nodes.ContainsKey(m.Value) ? nodes[m.Value] : nodes[m.Value] = new Node(m.Value, new List<Node>())).ToList();
                var head = currNodes.First();
                head.Children.Add(currNodes[1]);
                head.Children.Add(currNodes[2]);
            });

            return new Tuple<List<Node>, List<int>>(nodes.Values.ToList(), instruction);
        }
    }
}
