using Common;

Dictionary<EExtrapolateDirection, Func<List<long>, long>> ElementSelector = new()
{
    { EExtrapolateDirection.Forward, x => x.Last() },
    { EExtrapolateDirection.Backward, x => x.First() },
};

var readings = File.ReadAllLines(Constants.FILE_NAME)
    .Select(x => x.Split().Select(long.Parse).ToList())
    .ToList();

Console.WriteLine($"Part 1 result: {Solve(readings, EExtrapolateDirection.Forward)}");
Console.WriteLine($"Part 2 result: {Solve(readings, EExtrapolateDirection.Backward)}");

long Solve(List<List<long>> readings, EExtrapolateDirection extrapolateDirection)
{
    var nextValues = new List<long>();
    
    for (int i = 0; i < readings.Count; i++)
    {
        var extrapolation = ExtrapolateReading(readings[i]);
        extrapolation[^1].Add(0);

        for (int j = extrapolation.Length - 2; j >= 0; j--)
        {
            switch (extrapolateDirection)
            {
                case EExtrapolateDirection.Forward:
                    extrapolation[j].Add(ElementSelector[extrapolateDirection](extrapolation[j + 1]) + ElementSelector[extrapolateDirection](extrapolation[j]));
                    break;
                case EExtrapolateDirection.Backward:
                    extrapolation[j].Insert(0, ElementSelector[extrapolateDirection](extrapolation[j]) - ElementSelector[extrapolateDirection](extrapolation[j + 1]));
                    break;
            }
        }

        nextValues.Add(ElementSelector[extrapolateDirection](extrapolation[0]));
    }

    return nextValues.Sum();
}

List<long>[] ExtrapolateReading(List<long> reading)
{
    var extrapolation = new List<List<long>>
    {
        reading
    };

    while (!extrapolation.Last().All(x => x == 0))
    {
        var currentExtrapolation = extrapolation.Last();

        var newExtrapolation = new List<long>();
        for (int i = 1; i < currentExtrapolation.Count; i++)
        {
            newExtrapolation.Add(currentExtrapolation[i] - currentExtrapolation[i - 1]);
        }

        extrapolation.Add(newExtrapolation);
    }

    return extrapolation.ToArray();
}

enum EExtrapolateDirection
{
    Forward,
    Backward
}