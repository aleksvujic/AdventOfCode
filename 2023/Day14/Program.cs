using Common;
using System.Text.RegularExpressions;

Console.WriteLine($"Part 1 result: {Part1()}");

long Part1()
{
    var platform = ParseInput();

    var tilted = TiltPlatform(platform, ETiltDirection.North);

    return GetLoad(tilted);
}

char[][] TiltPlatform(char[][] platform, ETiltDirection tiltDirection)
{
    for (int colIdx = 0; colIdx < platform[0].Length; colIdx++)
    {
        // get column
        var column = Enumerable.Range(0, platform.Length)
            .Select(rowIdx => platform[rowIdx][colIdx])
            .ToArray();

        // slide rocks
        var columnChars = string.Join(string.Empty, column);
        var newColumn = Regex.Replace(columnChars, @"[^#]+", x =>
        {
            var rocks = string.Concat(Enumerable.Repeat('O', x.Value.Count(y => y == 'O')));
            var empty = string.Concat(Enumerable.Repeat('.', x.Value.Count(y => y == '.')));
            return $"{rocks}{empty}";
        });

        // replace column
        Enumerable.Range(0, platform.Length)
            .ToList()
            .ForEach(rowIdx => platform[rowIdx][colIdx] = newColumn[rowIdx]);
    }
    
    return platform;
}

long GetLoad(char[][] platform)
{
    long res = 0;

    for (int i = 0; i < platform.Length; i++)
    {
        res += platform[i].Count(x => x == 'O') * (platform.Length - 1 - i);
    }
    
    return res;
}

char[][] ParseInput()
{
    var platform = File.ReadAllLines(Constants.FILE_NAME)
        .Select(x => x.ToCharArray())
        .ToArray();

    var res = new char[platform.Length + 2][];
    for (int i = 0; i < res.Length; i++)
    {
        if (i == 0 || i == res.Length - 1)
        {
            res[i] = Enumerable.Repeat('#', platform[0].Length + 2).ToArray();
            continue;
        }

        var row = platform[i - 1].Prepend('#').Append('#');
        res[i] = row.ToArray();
    }

    return res;
}

void VisualizePlatform(char[][] platform)
{
    foreach (var row in platform)
    {
        Console.WriteLine(row);
    }
    Console.WriteLine();
}

enum ETiltDirection
{
    North
}