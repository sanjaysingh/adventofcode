using System.Reflection;
namespace AdventOfCode;
using Part = Dictionary<char, long>;
public static class Day19
{
    private static readonly string inputFileFullPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "day19/input.txt");

    public static void SolvePart1()
    {
        var (workflows, parts) = ReadInput();
        var startWorkflow = workflows.First(w => w.Id == "in");
        var acceptedParts = new List<Part>();
        foreach (var part in parts)
        {
            var nextWorkflow = startWorkflow;
            while (true)
            {
                var result = RunPartThroughWorkflow(nextWorkflow, part);
                if (result == "R") break;
                if (result == "A")
                {
                    acceptedParts.Add(part);
                    break;
                }
                nextWorkflow = workflows.First(w => w.Id == result);
            }
        }

        var sum = acceptedParts.Sum(p => p.Values.Sum());

        Console.WriteLine(sum);
    }

    private static string RunPartThroughWorkflow(Workflow workflow, Part part)
    {
        foreach (var rule in workflow.Rules)
        {
            if (string.IsNullOrEmpty(rule.Expr)) return rule.Result;
            var category = rule.Expr[0];
            var oper = rule.Expr[1];
            var val = long.Parse(rule.Expr.Substring(2));
            switch(oper)
            {
                case '<':
                    if (part[category] < val) return rule.Result;
                    break;
                case '>':
                    if (part[category] > val) return rule.Result;
                    break;
            }
        }
        return string.Empty;
    }

    private record Rule(string Expr, string Result);
    private record Workflow(string Id, List<Rule> Rules);

    private static (List<Workflow>, List<Part>) ReadInput()
    {
        var lines = File.ReadAllLines(inputFileFullPath);
        int index = 0;
        var workflows = new List<Workflow>();
        for (index = 0; index < lines.Length; index++)
        {
            if (lines[index].Length <= 0) break;
            var line = lines[index];
            string id = line.Substring(0, line.IndexOf("{"));
            var rules = line.Substring(line.IndexOf("{") + 1).Trim('}').Split(',').Select(rule =>
            {
                var ruleParts = rule.Split(":");
                return ruleParts.Count() > 1 ? new Rule(ruleParts[0], ruleParts[1]) : new Rule(string.Empty, ruleParts[0]);
            }).ToList();
            workflows.Add(new Workflow(id, rules));
        }
        var parts = new List<Part>();
        for (int pindex = index + 1; pindex < lines.Length; pindex++)
        {
            parts.Add(lines[pindex].Trim('{', '}').Split(",").Select(ptext => new KeyValuePair<char, long>(ptext.Split("=").First()[0], int.Parse(ptext.Split("=").Last()))).ToDictionary());
        }
        return (workflows, parts);
    }
}