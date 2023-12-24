using System.Reflection;
namespace AdventOfCode;
using PulseState = bool;
public static class Day20
{
    private static readonly string inputFileFullPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "day20/input.txt");
    private const PulseState LOW = false;
    private const PulseState HIGH = true;

    public static void SolvePart1()
    {
        var modules = ReadInput();
        var broadCaster = modules.First(m => m.Value.Kind == ModuleKind.Broadcast).Value;
        var startPulse = new Pulse(LOW, broadCaster);
        var pulseQueue = new Queue<Pulse>();
        Dictionary<PulseState, long> pulseCount = new Dictionary<PulseState, long> { { HIGH, 0 }, { LOW, 0 } };
        for (var i = 0; i < 1000; i++)
        {
            pulseQueue.Enqueue(startPulse);
            while (pulseQueue.Count > 0)
            {
                var currentPulse = pulseQueue.Dequeue();
                ProcessPulse(currentPulse).ForEach(nextPulse => pulseQueue.Enqueue(nextPulse));
                pulseCount[currentPulse.Signal]++;
            }
        }
        Console.WriteLine(pulseCount[HIGH] * pulseCount[LOW]);
    }

    public static void SolvePart2()
    {
        var modules = ReadInput();
        var broadCaster = modules.First(m => m.Value.Kind == ModuleKind.Broadcast).Value;
        var startPulse = new Pulse(LOW, broadCaster);
        var pulseQueue = new Queue<Pulse>();
        var rxModule = modules.First(m => m.Value.Name == "rx").Value;
        var buttonPress = 0;
        var rxInputMdduleHighStateButtonPress = new Dictionary<Module, long>();
        foreach (var module in rxModule.Input.First().Input)
        {
            rxInputMdduleHighStateButtonPress.Add(module, 0);
        }

        while (rxInputMdduleHighStateButtonPress.Any(m => m.Value <= 0))
        {
            pulseQueue.Enqueue(startPulse);
            buttonPress++;
            while (pulseQueue.Count > 0)
            {
                ProcessPulse(pulseQueue.Dequeue()).ForEach(nextPulse => pulseQueue.Enqueue(nextPulse));
            }

            rxInputMdduleHighStateButtonPress.Keys.Where(k => k.HighPulseCount == 1 && rxInputMdduleHighStateButtonPress[k] <= 0).ToList().ForEach(k=>rxInputMdduleHighStateButtonPress[k] = buttonPress);
        }
        Console.WriteLine(Lcm(rxInputMdduleHighStateButtonPress.Values.ToList()));
    }

    enum ModuleKind
    {
        None = 0,
        Broadcast,
        FlipFlop,
        Conjunction
    }

    private static long Gcd(long n1, long n2) => n2 == 0 ? n1 : Gcd(n2, n1 % n2);
    private static long Lcm(List<long> nums) => nums.Aggregate((x, y) => x * y / Gcd(x, y));

    private static List<Pulse> ProcessPulse(Pulse pulse)
    {

        var nextPulses = new List<Pulse>();
        switch (pulse.Target.Kind)
        {
            case ModuleKind.FlipFlop:
                if (pulse.Signal == HIGH) return nextPulses;
                pulse.Target.State = !pulse.Target.State;
                break;
            case ModuleKind.Conjunction:
                pulse.Target.State = pulse.Target.Input.All(x => x.State == HIGH) ? LOW : HIGH;
                break;
        }

        nextPulses.AddRange(pulse.Target.Output.Select(x => new Pulse(pulse.Target.State, x)));

        if (pulse.Target.State == HIGH) pulse.Target.HighPulseCount++;

        return nextPulses;
    }

    private record Pulse(PulseState Signal, Module Target);
    private class Module
    {
        public ModuleKind Kind;
        public string Name = string.Empty;
        public PulseState State = false;
        public List<Module> Input = new();
        public List<Module> Output = new();
        public long HighPulseCount = 0;
    }
    private static Dictionary<string, Module> ReadInput()
    {
        var modules = new Dictionary<string, Module>();
        foreach (var line in File.ReadLines(inputFileFullPath))
        {
            var parts = line.Split("->").Select(x => x.Trim()).ToList();
            var kind = ModuleKind.Broadcast;
            var name = parts[0];
            if (name[0] == '%')
                kind = ModuleKind.FlipFlop;
            else if (name[0] == '&')
                kind = ModuleKind.Conjunction;
            name = name.Replace("%", "").Replace("&", "");

            Module currModule = modules.ContainsKey(name) ? modules[name] : new Module() { Name = name, Kind = kind };
            modules[name] = currModule;
            currModule.Kind = kind;
            foreach (var outputModuleName in parts[1].Split(",").Select(x => x.Trim()))
            {
                if (!modules.ContainsKey(outputModuleName)) modules[outputModuleName] = new Module() { Name = outputModuleName, Kind = ModuleKind.None };
                var outputModule = modules[outputModuleName];
                if (!outputModule.Input.Contains(currModule)) outputModule.Input.Add(currModule);
                currModule.Output.Add(outputModule);
            }
        }
        return modules;
    }
}