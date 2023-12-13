using Common;

Console.WriteLine($"Part 1 result: {Part1()}");

long Part1()
{
    var patterns = ParseInput();
    return -1;
}

char[][][] ParseInput()
{
    var lines = File.ReadAllLines(Constants.FILE_NAME);

    List<char[][]> patterns = [];
    List<char[]> pattern = [];
    for (int i = 0; i < lines.Length; i++)
    {
        var line = lines[i];

        if (!string.IsNullOrEmpty(line))
        {
            pattern.Add(line.ToCharArray());
        }

        var flush = string.IsNullOrEmpty(line) || i == lines.Length - 1;
        if (flush)
        {
            patterns.Add(pattern.ToArray());
            pattern = [];
        }
    }

    return patterns.ToArray();
}