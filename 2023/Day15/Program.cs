using Common;

Console.WriteLine($"Part 1 result: {Part1()}");

long Part1()
{
    var sequences = ParseInput();
    return sequences
        .Select(GetHash)
        .Sum();
}

string[] ParseInput()
{
    return File.ReadAllText(Constants.FILE_NAME)
        .Split(',', StringSplitOptions.RemoveEmptyEntries);
}

long GetHash(string input)
{
    long hash = 0;

    for (int i = 0; i < input.Length; i++)
    {
        hash += input[i];
        hash *= 17;
        hash %= 256;
    }

    return hash;
}