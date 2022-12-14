using Common;

int SAND_DEFAULT_COL_IDX = 500;
int SAND_DEFAULT_ROW_IDX = 0;

var inputLines = File.ReadAllLines(Constants.FILE_NAME);

Console.WriteLine($"Part 1 result: {Part1(inputLines)}");
Console.WriteLine($"Part 2 result: {Part2(inputLines)}");

int Part1(string[] input)
{
    var map = ParseInput(input);
    return SimulateFallingSand(map, hasBottom: false);
}

int Part2(string[] input)
{
    var map = ParseInput(input);
    return SimulateFallingSand(map, hasBottom: true) + 1;
}

int SimulateFallingSand(HashSet<Coordinate> map, bool hasBottom)
{
    var rockMaxRowIdx = map.ToList().Max(x => x.RowIdx);
    if (hasBottom)
    {
        rockMaxRowIdx += 2;
    }
    int sandUnitsCount = 0;

    // while sand doesn't start falling off the cliff
    while (true)
    {
        bool currentSandIsBlocked = false;
        var sandUnit = new Coordinate(SAND_DEFAULT_COL_IDX, SAND_DEFAULT_ROW_IDX);
        while (!currentSandIsBlocked)
        {
            bool availableNeighborFound = false;

            foreach (var neighbor in GetNeighbors(sandUnit))
            {
                if (hasBottom && neighbor.RowIdx == rockMaxRowIdx)
                {
                    break;
                }
                
                if (!map.Contains(neighbor))
                {
                    availableNeighborFound = true;
                    sandUnit = neighbor;
                    break;
                }
            }

            if (sandUnit.RowIdx > rockMaxRowIdx || sandUnit.RowIdx == SAND_DEFAULT_ROW_IDX)
            {
                return sandUnitsCount;
            }

            currentSandIsBlocked = !availableNeighborFound;

            if (currentSandIsBlocked)
            {
                sandUnitsCount++;
                map.Add(sandUnit);
            }
        }
    }

    throw new Exception("Should not happen");
}

IEnumerable<Coordinate> GetNeighbors(Coordinate coord)
{
    // bottom
    yield return new Coordinate(coord.ColIdx, coord.RowIdx + 1);

    // diagonal left
    yield return new Coordinate(coord.ColIdx - 1, coord.RowIdx + 1);

    // diagonal right
    yield return new Coordinate(coord.ColIdx + 1, coord.RowIdx + 1);
}

HashSet<Coordinate> ParseInput(string[] input)
{
    var map = new HashSet<Coordinate>();

    foreach (var coordinateList in input.Select(x => x.Split(" -> ")))
    {
        var _ = coordinateList.Zip(coordinateList.Skip(1), (a, b) =>
        {
            var first = a.Split(",");
            var second = b.Split(",");

            var start = new Coordinate(int.Parse(first[0]), int.Parse(first[1]));
            var end = new Coordinate(int.Parse(second[0]), int.Parse(second[1]));

            GetCoordinatesBetween(start, end).ForEach(x => map.Add(x));

            // return whatever
            return 0;
        })
        .ToList();
    }

    return map;
}

List<Coordinate> GetCoordinatesBetween(Coordinate start, Coordinate end)
{
    int[] GetNumbersBetween(int start, int end)
    {
        int s = Math.Min(start, end);
        int e = Math.Max(start, end);
        return Enumerable.Range(s, e - s + 1).ToArray();
    }
    
    if (start.ColIdx == end.ColIdx)
    {
        // draw line with increasing 'row'
        return GetNumbersBetween(start.RowIdx, end.RowIdx)
            .Select(x => new Coordinate(start.ColIdx, x))
            .ToList();
    }

    if (start.RowIdx == end.RowIdx)
    {
        // draw line with increasing 'col'
        return GetNumbersBetween(start.ColIdx, end.ColIdx)
            .Select(x => new Coordinate(x, start.RowIdx))
            .ToList();
    }

    throw new Exception("Line can't be drawn between two coordinates");
}

record Coordinate(int ColIdx, int RowIdx)
{
    public override string ToString()
    {
        return $"{ColIdx},{RowIdx}";
    }
}

record Line(Coordinate Start, Coordinate End);