using Common;

int TOTAL_DISK_SIZE = 70000000;
int REQUIRED_FREE_DISK_SPACE = 30000000;

var input = File.ReadAllLines(Constants.FILE_NAME)
    .Skip(1)
    .Select(x =>
    {
        BaseOutput output;

        if (x.StartsWith("$"))
        {
            // command
            x = x.Replace("$ ", string.Empty);

            output = x.StartsWith("cd") ?
                new MoveCommand(x.Replace("cd ", string.Empty)) :
                new ListCommand();
        }
        else
        {
            // output
            var parts = x.Split();
            output = x.StartsWith("dir") ?
                new ConsoleOutput(parts[1], 0, ENodeType.Dir) :
                new ConsoleOutput(parts[1], long.Parse(parts[0]), ENodeType.File);
        }

        return output;
    })
    .ToArray();

var rootNode = new Node("/", ENodeType.Dir, 0);
ConstructTree(rootNode, rootNode, input, 0);
CalculateTotalSizes(rootNode);
Console.WriteLine($"Result: {Part1(rootNode)}");
Console.WriteLine($"Result: {Part2(rootNode, REQUIRED_FREE_DISK_SPACE - TOTAL_DISK_SIZE + rootNode.TotalSize)}");

long Part1(Node currentNode)
{
    long sum = 0;

    if (currentNode.Type == ENodeType.Dir && currentNode.TotalSize <= 100000)
    {
        sum += currentNode.TotalSize;
    }

    foreach (var child in currentNode.Children)
    {
        sum += Part1(child);
    }

    return sum;
}

long? Part2(Node root, long additionalNeededSpace)
{
    return FlattenTree(root)?
        .Where(x => x.Type == ENodeType.Dir)
        .Where(x => x.TotalSize >= additionalNeededSpace)?
        .OrderBy(x => x.TotalSize)?
        .FirstOrDefault()?
        .TotalSize;
}

IEnumerable<Node> FlattenTree(Node root)
{
    yield return root;
    foreach (var child in root.Children)
    {
        foreach (var descendant in FlattenTree(child))
        {
            yield return descendant;
        }
    }
}

long CalculateTotalSizes(Node currentNode)
{
    foreach (var child in currentNode.Children)
    {
        currentNode.TotalSize += CalculateTotalSizes(child);
    }
    return currentNode.TotalSize;
}

void ConstructTree(Node? root, Node? currentNode, BaseOutput[]? output, int currentIndex)
{
    if (root == null || currentNode == null || output == null)
    {
        throw new Exception();
    }
    
    if (currentIndex >= output.Length)
    {
        return;
    }

    switch (output[currentIndex])
    {
        case ListCommand listCommand:
            ConstructTree(root, currentNode, output, currentIndex + 1);
            break;
        case MoveCommand moveCommand:
            if (moveCommand.Folder == "..")
            {
                ConstructTree(root, currentNode.Parent, output, currentIndex + 1);
            }
            else if (moveCommand.Folder == "/")
            {
                ConstructTree(root, root, output, currentIndex + 1);
            }
            else
            {
                ConstructTree(root, currentNode.GetChildWithName(moveCommand.Folder), output, currentIndex + 1);
            }
            break;
        case ConsoleOutput consoleOutput:
            currentNode.AddChild(new Node(consoleOutput.Name, consoleOutput.Type, consoleOutput.Size));
            ConstructTree(root, currentNode, output, currentIndex + 1);
            break;
        default:
            throw new Exception("Unrecognized command");
    }
}

class Node
{
    public Node(string name, ENodeType type, long size)
    {
        Name = name;
        Type = type;
        Size = size;
        TotalSize = size;
        Parent = null;
        Children = new List<Node>();
    }

    public void AddChild(Node node)
    {
        node.Parent = this;
        Children.Add(node);
    }

    public Node? GetChildWithName(string name)
    {
        return Children.FirstOrDefault(x => x.Name == name);
    }

    public string Name { get; }
    public ENodeType Type { get; }
    public long Size { get; }
    public long TotalSize { get; set; }
    public Node? Parent { get; set; }
    public List<Node> Children { get; }

    public override string ToString()
    {
        return $"{Name} ({Type}, size={Size}, total size = {TotalSize})";
    }
}

public enum ENodeType
{
    File,
    Dir
}

record BaseOutput();
record ListCommand() : BaseOutput;
record MoveCommand(string Folder) : BaseOutput;
record ConsoleOutput(string Name, long Size, ENodeType Type) : BaseOutput;
