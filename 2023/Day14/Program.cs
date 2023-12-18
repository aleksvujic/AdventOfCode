using Common;
using System.Text.RegularExpressions;

Console.WriteLine($"Part 1 result: {Part1()}");
Console.WriteLine($"Part 2 result: {Part2()}");

long Part1()
{
    var platform = ParseInput();

    var tilted = TiltPlatform(platform, ETiltDirection.North);

    return GetLoad(tilted);
}

long Part2()
{
    var platform = ParseInput();

    var history = new List<string>();
    for (int i = 1_000_000_000; i > 0; i--)
    {
        platform = TiltPlatform(platform, ETiltDirection.North);
        platform = TiltPlatform(platform, ETiltDirection.West);
        platform = TiltPlatform(platform, ETiltDirection.South);
        platform = TiltPlatform(platform, ETiltDirection.East);

        var stringRepr = GetPlatformStringRepresentation(platform);
        var idx = history.IndexOf(stringRepr);

        if (idx < 0)
        {
            history.Add(stringRepr);
        }
        else
        {
            var loopLen = history.Count - idx;
            var rem = (i - 1) % loopLen;
            platform = GetPlatformFromStringRepresentation(history[idx + rem]);
            break;
        }
    }

    return GetLoad(platform);
}

char[][] TiltPlatform(char[][] platform, ETiltDirection tiltDirection)
{
    string SlideRocks(char[] data, ETiltDirection tiltDirection)
    {
        var chars = string.Join(string.Empty, data);
        return Regex.Replace(chars, @"[^#]+", x =>
        {
            var rocks = string.Concat(Enumerable.Repeat('O', x.Value.Count(y => y == 'O')));
            var empty = string.Concat(Enumerable.Repeat('.', x.Value.Count(y => y == '.')));
            
            if (tiltDirection == ETiltDirection.North || tiltDirection == ETiltDirection.West)
            {
                return $"{rocks}{empty}";
            }
            else if (tiltDirection == ETiltDirection.East || tiltDirection == ETiltDirection.South)
            {
                return $"{empty}{rocks}";
            }

            throw new Exception($"Tilt direction {tiltDirection} doesn't exist");
        });
    }

    if (tiltDirection == ETiltDirection.North || tiltDirection == ETiltDirection.South)
    {
        for (int colIdx = 0; colIdx < platform[0].Length; colIdx++)
        {
            // get column
            var column = Enumerable.Range(0, platform.Length)
                .Select(rowIdx => platform[rowIdx][colIdx])
                .ToArray();

            // slide rocks
            var slidedRocks = SlideRocks(column, tiltDirection);

            // replace column
            Enumerable.Range(0, platform.Length)
                .ToList()
                .ForEach(rowIdx => platform[rowIdx][colIdx] = slidedRocks[rowIdx]);
        }
    }
    else if (tiltDirection == ETiltDirection.West || tiltDirection == ETiltDirection.East)
    {
        for (int rowIdx = 0; rowIdx < platform.Length; rowIdx++)
        {
            // get row
            var row = platform[rowIdx];

            // slide rocks
            var slidedRocks = SlideRocks(row, tiltDirection);

            // replace row
            platform[rowIdx] = slidedRocks.ToCharArray();
        }
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

string GetPlatformStringRepresentation(char[][] platform)
{
    var res = string.Empty;

    for (int i = 0; i < platform.Length; i++)
    {
        if (i > 0)
        {
            res += Environment.NewLine;
        }

        res += new string(platform[i]);
    }

    return res;
}

char[][] GetPlatformFromStringRepresentation(string stringRepr)
{
    return stringRepr
        .Split(Environment.NewLine)
        .Select(x => x.ToCharArray())
        .ToArray();
}

enum ETiltDirection
{
    North,
    West,
    South,
    East
}
