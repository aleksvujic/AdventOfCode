using Common;

Console.WriteLine($"Part 1 result: {Part1()}");

long Part1()
{
    var platform = ParseInput();
    VisualizePlatform(platform);

    var tilted = TiltPlatform(platform, ETiltDirection.North);

    return 0;
}

char[][] TiltPlatform(char[][] platform, ETiltDirection tiltDirection)
{
    return null;
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
}

enum ETiltDirection
{
    North
}