using Common;

List<BaseInstruction> instructions = File.ReadAllLines(Constants.FILE_NAME)
    .Select(x =>
    {
        BaseInstruction instruction = x == "noop" ?
            new Noop() :
            new AddX(int.Parse(x.Split()[1]));

        return instruction;
    })
    .ToList();

Console.WriteLine($"Part 1 solution: {Part1(instructions)}");
Console.WriteLine("Part 2 solution:");
Console.WriteLine(Part2(instructions));

int Part1(List<BaseInstruction> instructions)
{
    return GetSignal(instructions)
        .Where(x => ShouldCountSignalStrength(x.Cycle))
        .Select(x => x.Cycle * x.RegisterVal)
        .Sum();
}

string Part2(List<BaseInstruction> instructions)
{
    var output = string.Empty;
    int width = 40;
    foreach (var signal in GetSignal(instructions))
    {
        var spriteX = signal.RegisterVal;
        var col = (signal.Cycle - 1) % width;

        output += Math.Abs(spriteX - col) < 2 ? "#" : ".";

        if (col == width - 1)
        {
            output += Environment.NewLine;
        }
    }

    return output;
}

bool ShouldCountSignalStrength(int cycle)
{
    return (cycle - 20) % 40 == 0;
}

IEnumerable<ProcessorCycle> GetSignal(List<BaseInstruction> instructions)
{
    var cycle = 1;
    var registerVal = 1;
    foreach (var instruction in instructions)
    {
        if (instruction is Noop)
        {
            yield return new ProcessorCycle(cycle++, registerVal);
        }
        else if (instruction is AddX addX)
        {
            yield return new ProcessorCycle(cycle++, registerVal);
            yield return new ProcessorCycle(cycle++, registerVal);
            registerVal += addX.Value;
        }
        else
        {
            throw new Exception($"Instruction {instruction} is not recognized");
        }
    }
}

record BaseInstruction();
record Noop() : BaseInstruction();
record AddX(int Value) : BaseInstruction();

record ProcessorCycle(int Cycle, int RegisterVal);
