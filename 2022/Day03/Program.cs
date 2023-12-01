using Common;

var lines = File.ReadAllLines(Constants.FILE_NAME).ToList();
Part1(lines);
Part2(lines);

void Part1(List<string> lines)
{
    var sum = lines
        .Select(x => x.Chunk(x.Length / 2))
        .Select(GetItemPriority)
        .Sum();

    Console.WriteLine($"Part 1 result: {sum}");
}

void Part2(List<string> lines)
{
    var sum = lines
        .Chunk(3)
        .Select(GetItemPriority)
        .Sum();
    
    Console.WriteLine($"Part 2 result: {sum}");
}

int GetItemPriority(IEnumerable<IEnumerable<char>> items)
{
    var intersection = items
        .Skip(1)
        .Aggregate(
            new HashSet<char>(items.First()),
            (h, e) => { h.IntersectWith(e); return h; }
        )
        .First();

    return intersection < 'a' ? intersection - 'A' + 27 : intersection - 'a' + 1;
}