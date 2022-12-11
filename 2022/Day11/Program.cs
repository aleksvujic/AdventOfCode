using Common;

var input = File.ReadAllLines(Constants.FILE_NAME)
    .Select(x => x.Trim())
    .Chunk(7);

Console.WriteLine($"Part 1 result: {Part1(input)}");
Console.WriteLine($"Part 2 result: {Part2(input)}");

long Part1(IEnumerable<string[]> input)
{
    var monkeys = ParseMonkeys(input);
    var monkeyGroup = new MonkeyGroup(monkeys, x => x / 3);
    return SimulateRounds(monkeyGroup, 20);
}

long Part2(IEnumerable<string[]> input)
{
    var monkeys = ParseMonkeys(input);
    var mod = monkeys.Values.Aggregate(1, (mod, monkey) => mod * monkey.DivisibleByNum);
    var monkeyGroup = new MonkeyGroup(monkeys, x => x % mod);
    return SimulateRounds(monkeyGroup, 10000);
}

long SimulateRounds(MonkeyGroup monkeyGroup, int numRounds)
{
    for (int i = 0; i < numRounds; i++)
    {
        monkeyGroup.SimulateRound();
    }

    return monkeyGroup.GetMonkeyBusinessLevel();
}

Dictionary<int, Monkey> ParseMonkeys(IEnumerable<string[]> input)
{
    return input
        .Select(x =>
        {
            var id = int.Parse(x[0].Replace(":", "").Replace("Monkey ", ""));
            var items = x[1].Replace("Starting items: ", "");
            var operation = x[2].Replace("Operation: new = ", "");
            var divisionTest = int.Parse(x[3].Replace("Test: divisible by ", ""));
            var monkeyIdxTestTrue = int.Parse(x[4].Replace("If true: throw to monkey ", ""));
            var monkeyIdxTestFalse = int.Parse(x[5].Replace("If false: throw to monkey ", ""));

            return new Monkey(id, items, operation, divisionTest, monkeyIdxTestTrue, monkeyIdxTestFalse);
        })
        .ToDictionary(x => x.Id);
}

class MonkeyGroup
{
    public MonkeyGroup(Dictionary<int, Monkey> monkeys, Func<long, long> updateWorryLevelFunc)
    {
        _monkeys = monkeys;
        _updateWorryLevelFunc = updateWorryLevelFunc;
    }

    readonly Dictionary<int, Monkey> _monkeys;
    readonly Func<long, long> _updateWorryLevelFunc;

    public void SimulateRound()
    {
        for (int i = 0; i < _monkeys.Count; i++)
        {
            var monkey = _monkeys[i];
            for (int j = 0; j < monkey.Items.Count; j++)
            {
                monkey.IncreaseItemsInspected();

                monkey.Items[j] = monkey.MathOperation.Invoke(monkey.Items[j]);
                monkey.Items[j] = _updateWorryLevelFunc(monkey.Items[j]);

                if (monkey.Items[j] % monkey.DivisibleByNum == 0)
                {
                    _monkeys[monkey.MonkeyIdxIfDivisibleTestTrue].Items.Add(monkey.Items[j]);
                }
                else
                {
                    _monkeys[monkey.MonkeyIdxIfDivisibleTestFalse].Items.Add(monkey.Items[j]);
                }
            }

            monkey.Items.Clear();
        }
    }

    public long GetMonkeyBusinessLevel()
    {
        return _monkeys.Values
            .Select(x => x.GetInspectedItems())
            .OrderByDescending(x => x)
            .Take(2)
            .Aggregate((a, x) => a * x);
    }
}

class Monkey
{
    public Monkey(int id, string items, string operation, int divisionTest, int monkeyIdxTestTrue, int monkeyIdxTestFalse)
    {
        Id = id;
        Items = items.Split(", ").Select(long.Parse).ToList();
        MathOperation = new MathOperation(operation);
        DivisibleByNum = divisionTest;
        MonkeyIdxIfDivisibleTestTrue = monkeyIdxTestTrue;
        MonkeyIdxIfDivisibleTestFalse = monkeyIdxTestFalse;
    }

    public int Id { get; set; }
    public List<long> Items { get; set; }
    public MathOperation MathOperation { get; set; }
    public int DivisibleByNum { get; set; }
    public int MonkeyIdxIfDivisibleTestTrue { get; set; }
    public int MonkeyIdxIfDivisibleTestFalse { get; set; }
    private long _itemsInspected = 0;

    public void IncreaseItemsInspected()
    {
        _itemsInspected++;
    }

    public long GetInspectedItems()
    {
        return _itemsInspected;
    }

    public override string ToString()
    {
        return $"Id={Id},Items={string.Join(",", Items)},Operation={MathOperation},Test={DivisibleByNum},TrueIdx={MonkeyIdxIfDivisibleTestTrue},FalseIdx={MonkeyIdxIfDivisibleTestFalse}";
    }
}

enum EMathOperation
{
    Addition,
    Subtraction,
    Multiplication,
    Division
}

class MathOperation
{
    public MathOperation(string expr)
    {
        var parts = expr.Split();

        _operands = new BaseOperand[2]
        {
            parts[0] == "old" ? new VarOperand() : new Number(int.Parse(parts[0])),
            parts[2] == "old" ? new VarOperand() : new Number(int.Parse(parts[2]))
        };

        if (parts[1] == "+")
            _mathOperation = EMathOperation.Addition;
        else if (parts[1] == "-")
            _mathOperation = EMathOperation.Subtraction;
        else if (parts[1] == "*")
            _mathOperation = EMathOperation.Multiplication;
        else if (parts[1] == "/")
            _mathOperation = EMathOperation.Division;
        else
            throw new Exception($"Operation {parts[1]} is not recognized");
    }

    private readonly BaseOperand[] _operands;
    private readonly EMathOperation _mathOperation;

    public long Invoke(long val)
    {
        long firstVal = GetValFromOperand(_operands[0], val);
        long secondVal = GetValFromOperand(_operands[1], val);

        return _mathOperation switch
        {
            EMathOperation.Addition => firstVal + secondVal,
            EMathOperation.Subtraction => firstVal - secondVal,
            EMathOperation.Multiplication => firstVal * secondVal,
            EMathOperation.Division => firstVal / secondVal,
            _ => throw new Exception($"{_mathOperation} is not recognized")
        };
    }

    private long GetValFromOperand(BaseOperand operand, long val)
    {
        if (operand is VarOperand)
            return val;
        else if (operand is Number number)
            return number.Value;
        else
            throw new Exception($"Operand {operand} is not recognized");
    }

    public override string ToString()
    {
        return $"{_operands[0]}{_mathOperation}{_operands[1]}";
    }

    record BaseOperand();
    record VarOperand() : BaseOperand;
    record Number(long Value) : BaseOperand;
}