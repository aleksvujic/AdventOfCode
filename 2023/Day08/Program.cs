using Common;

Console.WriteLine($"Part 1 result: {Part1()}");
Console.WriteLine($"Part 2 result: {Part2()}");

long Part1()
{
    (EDirection[] instructions, Dictionary<string, Node> graph) = ParseInput();

    var currentNode = "AAA";
    var instrIdx = 0;
    while (currentNode != "ZZZ")
    {
        var instruction = instructions[instrIdx % instructions.Length];
        currentNode = GetNextNode(graph, currentNode, instruction);
        instrIdx++;
    }

    return instrIdx;
}

long Part2()
{
    long GetListLcm(List<long> nums)
    {
        long lcm = nums[0];
        for (int i = 1; i < nums.Count; i++)
        {
            lcm = GetLcm(lcm, nums[i]);
        }
        return lcm;
    }
    
    long GetLcm(long a, long b)
    {
        return (a * b) / GetGcd(a, b);
    }

    long GetGcd(long a, long b)
    {
        while (b != 0)
        {
            long temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    (EDirection[] instructions, Dictionary<string, Node> graph) = ParseInput();

    var startNodes = graph.Keys.Where(x => x.EndsWith('A')).ToList();
    var instrIdx = 0;
    var numSteps = new List<long>();

    while (startNodes.Any())
    {
        var instruction = instructions[instrIdx % instructions.Length];
        for (int i = startNodes.Count - 1; i >= 0; i--)
        {
            startNodes[i] = GetNextNode(graph, startNodes[i], instruction);
            if (startNodes[i].EndsWith('Z'))
            {
                numSteps.Add(instrIdx + 1);
                startNodes.RemoveAt(i);
            }
        }

        instrIdx++;
    }

    return GetListLcm(numSteps);
}

string GetNextNode(Dictionary<string, Node> graph, string currentNode, EDirection instruction)
{
    return instruction switch
    {
        EDirection.Left => graph[currentNode].LeftVal,
        EDirection.Right => graph[currentNode].RightVal,
        _ => throw new Exception($"Instruction {instruction} doesn't exist"),
    };
}

(EDirection[] Instructions, Dictionary<string, Node> Graph) ParseInput()
{
    EDirection[] instr = null;
    var lines = File.ReadAllLines(Constants.FILE_NAME);
    var graph = new Dictionary<string, Node>();
    for (int i = 0; i < lines.Length; i++)
    {
        var line = lines[i];
        
        if (i == 0)
        {
            instr = line.ToCharArray()
                .Select(x => x == 'R' ? EDirection.Right : EDirection.Left)
                .ToArray();

            continue;
        }

        if (string.IsNullOrEmpty(line))
        {
            continue;
        }

        var val = line[0..3];
        var leftVal = line[7..10];
        var rightVal = line[12..15];
        graph.Add(val, new Node(val, leftVal, rightVal));
    }

    return (instr, graph);
}

record Node(string Val, string LeftVal, string RightVal);

enum EDirection
{
    Left,
    Right
}