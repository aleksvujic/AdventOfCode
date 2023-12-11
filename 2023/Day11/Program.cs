using Common;
using Map = System.Collections.Generic.List<System.Collections.Generic.List<char>>;

char EMPTY = '.';
char GALAXY = '#';

Console.WriteLine($"Part 1 result: {Solve(1)}");
Console.WriteLine($"Part 1 result: {Solve(999999)}");

Map ParseMap()
{
    return File.ReadAllLines(Constants.FILE_NAME)
        .Select(x => x.ToCharArray().ToList())
        .ToList();
}

int[] GetEmptyIdxs(Map map, EDimension dimension)
{
    return dimension switch
    {
        EDimension.Row => Enumerable.Range(0, map.Count)
            .Where(rowIdx => map[rowIdx].All(x => x == EMPTY))
            .ToArray(),
        EDimension.Column => Enumerable.Range(0, map[0].Count)
            .Where(colIdx => map.All(x => x[colIdx] == EMPTY))
            .ToArray(),
        _ => throw new Exception(),
    };
}

Position[] GetGalaxies(Map map)
{
    var galaxies = new List<Position>();
    for (int rowIdx = 0; rowIdx < map.Count; rowIdx++)
    {
        for (int colIdx = 0; colIdx < map[rowIdx].Count; colIdx++)
        {
            if (map[rowIdx][colIdx] == GALAXY)
            {
                galaxies.Add(new Position(rowIdx, colIdx));
            }
        }
    }
    return galaxies.ToArray();
}

long Solve(int expansion)
{
    var map = ParseMap();

    var emptyRowIdxs = GetEmptyIdxs(map, EDimension.Row).ToHashSet();
    var emptyColIdxs = GetEmptyIdxs(map, EDimension.Column).ToHashSet();

    var galaxies = GetGalaxies(map);

    long res = 0;
    for (int i = 0; i < galaxies.Length; i++)
    {
        for (int j = i + 1; j < galaxies.Length; j++)
        {
            var g1 = galaxies[i];
            var g2 = galaxies[j];

            res += GetDistance(g1, g2, emptyRowIdxs, emptyColIdxs, expansion);
        }
    }

    return res;
}

long GetDistance(Position p1, Position p2, HashSet<int> emptyRowIdxs, HashSet<int> emptyColIdxs, int expansion)
{
    var xDist = Math.Abs(p1.ColIdx - p2.ColIdx);
    var xExpandedRowsCount = Enumerable.Range(Math.Min(p1.ColIdx, p2.ColIdx), xDist)
        .Count(emptyColIdxs.Contains);

    var yDist = Math.Abs(p1.RowIdx - p2.RowIdx);
    var yExpandedRowsCount = Enumerable.Range(Math.Min(p1.RowIdx, p2.RowIdx), yDist)
        .Count(emptyRowIdxs.Contains);

    return xDist + yDist + expansion * (xExpandedRowsCount + yExpandedRowsCount);
}

enum EDimension
{
    Row,
    Column
}

record Position(int RowIdx, int ColIdx);