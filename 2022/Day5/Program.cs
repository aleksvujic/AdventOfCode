using Common;

var lines = File.ReadAllLines(Constants.FILE_NAME);
var stackLines = lines.TakeWhile(x => !string.IsNullOrEmpty(x)).ToList();
var instructionLines = lines.Where(x => x.StartsWith("move"));

var stacks = new Dictionary<int, Stack<char>>();
for (int i = stackLines.Count - 2; i >= 0; i--)
{
    var line = stackLines[i];
    for (int j = 1; j < line.Length; j += 4)
    {
        var key = j / 4 + 1;
        if (!stacks.ContainsKey(key))
        {
            stacks[key] = new Stack<char>();
        }

        var crate = line[j];
        if (char.IsLetter(crate))
        {
            stacks[key].Push(crate);
        }
    }
}

var instructions = instructionLines
    .Select(x =>
    {
        var numbers = x
            .Replace("move ", string.Empty)
            .Replace("from ", string.Empty)
            .Replace("to ", string.Empty)
            .Split(' ')
            .Select(int.Parse)
            .ToArray();

        return new Instruction(numbers[0], numbers[1], numbers[2]);
    })
    .ToList();

Solve(CopyState(stacks), instructions, moveAll: false);
Solve(CopyState(stacks), instructions, moveAll: true);

void Solve(Dictionary<int, Stack<char>> stacks, List<Instruction> instructions, bool moveAll)
{
    foreach ((int HowMany, int From, int To) in instructions)
    {
        var sourceStack = stacks[From];
        if (moveAll)
        {
            var tempStack = new Stack<char>();
            for (int i = 0; i < HowMany; i++)
            {
                tempStack.Push(sourceStack.Pop());
            }
            sourceStack = tempStack;
        }

        for (int i = 0; i < HowMany; i++)
        {
            stacks[To].Push(sourceStack.Pop());
        }
    }

    var res = new string(Enumerable.Range(1, stacks.Count)
        .Select(x => stacks[x].Peek())
        .ToArray());

    Console.WriteLine($"Result: {res}");
}

Dictionary<int, Stack<char>> CopyState(Dictionary<int, Stack<char>> state)
{
    return state
        .ToDictionary(x => x.Key, x => new Stack<char>(x.Value.Reverse()));
}

record Instruction(int HowMany, int From, int To);