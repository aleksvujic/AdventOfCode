using Common;

var pairs = File.ReadAllLines(Constants.FILE_NAME)
    .Select(x =>
    {
        var pair = x.Split(',');
        var parts = pair.Select(x => x.Split('-')).ToArray();
        var firstRange = new Range(int.Parse(parts[0][0]), int.Parse(parts[0][1]));
        var secondRange = new Range(int.Parse(parts[1][0]), int.Parse(parts[1][1]));
        
        return (firstRange, secondRange);
    })
    .ToList();

Console.WriteLine($"Part 1: {CountOccurences(pairs, DoRangesOverlap1)}");
Console.WriteLine($"Part 2: {CountOccurences(pairs, DoRangesOverlap2)}");

int CountOccurences(List<(Range, Range)> pairs, Func<Range, Range, bool> isContained)
{
    return pairs.Count(x => isContained(x.Item1, x.Item2));
}

bool DoRangesOverlap1(Range first, Range second)
{
    return
        (first.Start >= second.Start && first.End <= second.End) ||
        (second.Start >= first.Start && second.End <= first.End);
}

bool DoRangesOverlap2(Range first, Range second)
{
    return first.Start <= second.End && first.End >= second.Start;
}

record Range(int Start, int End);