using Common;

var lines = File.ReadAllLines(Constants.FILE_NAME);

char START_SYMBOL = 'S';
char ALTERNATIVE_START_SYMBOL = 'a';
char FINISH_SYMBOL = 'E';

Console.WriteLine($"Part 1 result: {Part1(lines)}");
Console.WriteLine($"Part 2 result: {Part2(lines)}");

int Part1(IEnumerable<string> lines)
{
    var grid = ParseInput(lines);
    var start = FindCoordinatesInGrid(grid, START_SYMBOL).First();
    var finish = FindCoordinatesInGrid(grid, FINISH_SYMBOL).First();
    ReplaceStartAndFinishWithHeights(grid);
    var res = FindShortestPath(grid, start, finish);
    return res.Count - 1;
}

int Part2(IEnumerable<string> lines)
{
    var grid = ParseInput(lines);
    var finish = FindCoordinatesInGrid(grid, FINISH_SYMBOL).First();
    ReplaceStartAndFinishWithHeights(grid);

    var pathLengths = new List<int>();
    foreach (var start in FindCoordinatesInGrid(grid, START_SYMBOL).Concat(FindCoordinatesInGrid(grid, ALTERNATIVE_START_SYMBOL)))
    {
        var shortestPath = FindShortestPath(grid, start, finish);
        if (shortestPath != null)
        {
            pathLengths.Add(shortestPath.Count - 1);
        }
    }

    return pathLengths.Min();
}

int[][] ParseInput(IEnumerable<string> lines)
{
    return lines
        .Select(x => x.Select(y => y - '0').ToArray())
        .ToArray();
}

IEnumerable<Coordinate> FindCoordinatesInGrid(int[][] grid, char searchChar)
{
    for (int i = 0; i < grid.Length; i++)
    {
        for (int j = 0; j < grid[i].Length; j++)
        {
            if (grid[i][j] == (searchChar - '0'))
            {
                yield return new Coordinate(i, j);
            }
        }
    }
}

void ReplaceStartAndFinishWithHeights(int[][] grid)
{
    for (int i = 0; i < grid.Length; i++)
    {
        for (int j = 0; j < grid[i].Length; j++)
        {
            if (grid[i][j] == START_SYMBOL - '0')
            {
                grid[i][j] = 'a' - '0';
            }
            else if (grid[i][j] == FINISH_SYMBOL - '0')
            {
                grid[i][j] = 'z' - '0';
            }
        }
    }
}

List<Coordinate> FindShortestPath(int[][] grid, Coordinate start, Coordinate end)
{
    var visited = new HashSet<Coordinate>();
    var queue = new Queue<List<Coordinate>>();
    queue.Enqueue(new List<Coordinate>() { start });
    
    while (queue.Count > 0)
    {
        var path = queue.Dequeue();
        var coord = path.Last();

        if (coord == end)
        {
            return path;
        }

        if (visited.Contains(coord))
        {
            continue;
        }

        visited.Add(coord);

        foreach ((int dx, int dy) in new[] { (-1, 0), (1, 0), (0, -1), (0, 1) })
        {
            var neighbor = new Coordinate(coord.IdxRow + dx, coord.IdxCol + dy);

            if (neighbor.IdxRow < 0 || neighbor.IdxRow >= grid.Length || neighbor.IdxCol < 0 || neighbor.IdxCol >= grid[0].Length)
            {
                // of the bounds of the matrix
                continue;
            }

            if (grid[neighbor.IdxRow][neighbor.IdxCol] - grid[coord.IdxRow][coord.IdxCol] > 1)
            {
                // climb too steep - should only go up 1 unit at a time
                continue;
            }

            if (!visited.Contains(neighbor))
            {
                var newPath = new List<Coordinate>(path)
                {
                    neighbor
                };
                queue.Enqueue(newPath);
            }
        }
    }

    return null;
}

record Coordinate(int IdxRow, int IdxCol);
