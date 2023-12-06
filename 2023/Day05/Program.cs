using Common;

(List<long> Seeds, Dictionary<MapKey, List<NumRange>> MapOfRanges) = ParseInput(File.ReadAllLines(Constants.FILE_NAME));

Console.WriteLine($"Part 1 result: {Part1(Seeds, MapOfRanges)}");
Console.WriteLine($"Part 2 result: {Part2(Seeds, MapOfRanges)}");

static long Part1(List<long> seeds, Dictionary<MapKey, List<NumRange>> mapOfRanges)
{
    long currentMin = long.MaxValue;

    foreach (var seed in seeds)
    {
        var res = GetSeedMapping(seed, mapOfRanges);
        if (res < currentMin)
        {
            currentMin = res;
        }
    }

    return currentMin;
}

static long Part2(List<long> seeds, Dictionary<MapKey, List<NumRange>> mapOfRanges)
{
    long currentMin = long.MaxValue;

    for (int i = 0; i < seeds.Count; i += 2)
    {
        var startNum = seeds[i];
        var numSeeds = seeds[i + 1];

        for (int j = 0; j < numSeeds; j++)
        {
            var res = GetSeedMapping(startNum + j, mapOfRanges);
            if (res < currentMin)
            {
                currentMin = res;
            }
        }
    }

    return currentMin;
}

static long GetSeedMapping(long seed, Dictionary<MapKey, List<NumRange>> mapOfRanges)
{
    static long GetFromMapOrDefault(long val, List<NumRange> ranges)
    {
        var range = ranges
            .SingleOrDefault(x => (x.Source <= val) && (val <= x.Source + x.Length - 1));

        if (range == null)
        {
            return val;
        }

        return range.Destination + (val - range.Source);
    }

    var key = mapOfRanges.Keys.Single(x => x.From == "seed");
    var lastVal = seed;

    while (key != null)
    {
        lastVal = GetFromMapOrDefault(lastVal, mapOfRanges[key]);
        key = mapOfRanges.Keys.SingleOrDefault(x => x.From == key.To);
    }

    return lastVal;
}

static (List<long> Seeds, Dictionary<MapKey, List<NumRange>> MapOfRanges) ParseInput(string[] lines)
{
    var seeds = new List<long>();
    var mapOfRanges = new Dictionary<MapKey, List<NumRange>>();

    for (long i = 0; i < lines.Length; i++)
    {
        var line = lines[i];

        if (string.IsNullOrEmpty(line))
        {
            continue;
        }

        if (line.StartsWith("seeds:"))
        {
            seeds.AddRange(line.Replace("seeds: ", string.Empty).Split().Select(long.Parse));
            continue;
        }

        if (line.Contains("map:"))
        {
            var keyParts = line
                .Replace(" map:", string.Empty)
                .Split("-to-");

            var key = new MapKey(keyParts.First(), keyParts.Last());
            mapOfRanges[key] = new List<NumRange>();

            long j = i + 1;
            while (j < lines.Length && !string.IsNullOrEmpty(lines[j]))
            {
                var mapLine = lines[j].Split().Select(long.Parse).ToArray();
                mapOfRanges[key].Add(new NumRange(mapLine[0], mapLine[1], mapLine[2]));
                j++;
            }

            i = j;

            continue;
        }
    }

    return (seeds, mapOfRanges);
}

record NumRange(long Destination, long Source, long Length);
record MapKey(string From, string To);