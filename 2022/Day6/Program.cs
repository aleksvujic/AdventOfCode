using Common;

var input = File.ReadAllText(Constants.FILE_NAME);

FindMarker(input, 4);
FindMarker(input, 14);

void FindMarker(string input, int windowLen)
{
    for (int i = 0; i < input.Length - windowLen + 1; i++)
    {
        var str = input[i..(i + windowLen)];
        if (ContainsAllUniqueCharacters(str))
        {
            Console.WriteLine($"Solution: {i + windowLen}");
            return;
        }
    }
}

bool ContainsAllUniqueCharacters(string input)
{
    return input
        .GroupBy(x => x)
        .Select(x => x.Count())
        .All(x => x == 1);
}