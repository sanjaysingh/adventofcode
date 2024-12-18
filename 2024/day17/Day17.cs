using System.Text.RegularExpressions;
namespace AdventOfCode;

public static class Day17
{
    private static readonly string InputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day17", "input.txt");

    public static void SolvePart1()
    {
        var p = ReadInput();
        var result = p.Execute();
        Console.WriteLine(string.Join(',', result));
    }

    public static void SolvePart2()
    {
    }

    private static ComputerProgram ReadInput()
    {
        var nums = Regex.Matches(File.ReadAllText(InputPath), @"\d+").Select(m => int.Parse(m.Value)).ToList();
        return new ComputerProgram(nums[0], nums[1], nums[2], nums.Skip(3).ToArray());
    }
    private record ComputerProgram(int RegisterA, int RegisterB, int RegisterC, int[] Instructions)
    {

        private static int ComboOperandValue(int operand, int regA, int regB, int regC) =>
            operand switch
            {
                >= 0 and <= 3 => operand,
                4 => regA,
                5 => regB,
                6 => regC,
                _ => throw new InvalidOperationException()
            };

        public ComputerProgram WithRegA(int regA) => new ComputerProgram(regA, RegisterB, RegisterC, Instructions);
        public IEnumerable<int> Execute()
        {
            var opcodeIndex = 0;
            var outputs = new List<int>();
            int regA = RegisterA, regB = RegisterB, regC = RegisterC;

            while (opcodeIndex < Instructions.Length)
            {
                var opcode = Instructions[opcodeIndex];
                var comboOperand = ComboOperandValue(Instructions[opcodeIndex + 1], regA, regB, regC);
                switch (opcode)
                {
                    case 0:
                        regA = (int)Math.Floor(regA / Math.Pow(2, comboOperand));
                        break;
                    case 1:
                        regB = regB ^ Instructions[opcodeIndex + 1];
                        break;
                    case 2:
                        regB = comboOperand % 8;
                        break;
                    case 3:
                        if (regA == 0) break;
                        opcodeIndex = Instructions[opcodeIndex + 1] - 2;
                        break;
                    case 4:
                        regB = regB ^ regC;
                        break;
                    case 5:
                        outputs.Add(comboOperand % 8);
                        break;
                    case 6:
                        regB = (int)Math.Floor(regA / Math.Pow(2, comboOperand));
                        break;
                    case 7:
                        regC = (int)Math.Floor(regA / Math.Pow(2, comboOperand));
                        break;
                }

                opcodeIndex += 2;
            }
            return outputs;
        }
    }
}