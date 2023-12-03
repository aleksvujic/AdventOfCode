using Common;
using System.Text.RegularExpressions;

var games = File.ReadAllLines(Constants.FILE_NAME)
    .Select(x => Regex.Replace(x, @"Game \d+: ", string.Empty))
    .Select(x => x.Split(';'))
    .Select(x => x.Select(y => CubesRevealed.Parse(y)).ToArray())
    .Select((val, idx) => new { Games = val, Index = (idx  + 1) })
    .ToDictionary(pair => pair.Index, pair => pair.Games);

var res1 = games
    .Where(x => x.Value.All(y => y.Red <= 12 && y.Green <= 13 && y.Blue <= 14))
    .Sum(x => x.Key);

Console.WriteLine($"Part 1 result: {res1}");

var res2 = games
    .Select(x =>
    {
        var maxRed = x.Value.Max(y => y.Red);
        var maxGreen = x.Value.Max(y => y.Green);
        var maxBlue = x.Value.Max(y => y.Blue);

        return maxRed * maxGreen * maxBlue;
    })
    .Sum();

Console.WriteLine($"Part 2 result: {res2}");

record CubesRevealed(int Red, int Green, int Blue)
{
    public static CubesRevealed Parse(string input)
    {
        var parts = input
            .Trim()
            .Split(", ")
            .Select(x => x.Split(' '));

        int red = 0, green = 0, blue = 0;
        foreach (var part in parts)
        {
            if (part[1] == nameof(Red).ToLower())
            {
                red = int.Parse(part[0]);
            }
            else if (part[1] == nameof(Green).ToLower())
            {
                green = int.Parse(part[0]);
            }
            else if (part[1] == nameof(Blue).ToLower())
            {
                blue = int.Parse(part[0]);
            }
        }

        return new CubesRevealed(red, green, blue);
    }
}