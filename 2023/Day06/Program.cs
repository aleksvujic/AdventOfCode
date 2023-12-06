using Common;
using System.Text.RegularExpressions;

var lines = File.ReadAllLines(Constants.FILE_NAME)
    .Select(x => Regex.Replace(x, @"\w+:", string.Empty))
    .Select(x => x.Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse));

var races = lines.First()
    .Zip(lines.Last(), (first, second) => new Race(first, second))
    .ToArray();

Console.WriteLine($"Part 1 result: {Part1(races)}");
Console.WriteLine($"Part 2 result: {Part2(races)}");

long Part1(Race[] races)
{
    return races.Aggregate(1L, (res, race) => res * GetNumWins(race));
}

long Part2(Race[] races)
{
    long Combine(Func<Race, long> selectorFunc)
    {
        return long.Parse(races.Aggregate(string.Empty, (res, val) => res += selectorFunc(val).ToString()));
    }

    var race = new Race(Combine(x => x.Time), Combine(x => x.Distance));

    return GetNumWins(race);
}

long GetNumWins(Race race)
{
    var winCounter = 0;

    for (int holdDuration = 0; holdDuration <= race.Time; holdDuration++)
    {
        var totalDistance = holdDuration * (race.Time - holdDuration);

        if (totalDistance > race.Distance)
        {
            winCounter++;
        }
    }

    return winCounter;
}

record Race(long Time, long Distance);