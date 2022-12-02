using Common;

namespace Day9
{
    internal class Program
    {
        static void Main()
        {
            // parse input into grid
            var grid = new Grid(File.ReadAllLines(Constants.FILE_NAME));

            grid.MarkLowestPoints();

            Console.WriteLine($"Part 1 result: {grid.GetRiskLevels()}");
            Console.WriteLine($"Part 2 result: {grid.GetBasins()}");
        }
    }

    class Grid
    {
        public Point[][] Mesh { get; }

        public Grid(string[] lines)
        {
            Mesh = lines.Select((x, idx1) => x
                .ToCharArray()
                .Select((y, idx2) => new Point()
                {
                    // x is horizontal
                    X = idx2,
                    // y is vertical
                    Y = idx1,
                    Value = y - '0',
                    IsLowestPoint = false
                })
                .ToArray())
            .ToArray();
        }

        public void MarkLowestPoints()
        {
            for (int i = 0; i < Mesh.Length; i++)
            {
                for (int j = 0; j < Mesh[i].Length; j++)
                {
                    bool smallerThanTop = i == 0 || Mesh[i][j].Value < Mesh[i - 1][j].Value;
                    bool smallerThanRight = j == Mesh[i].Length - 1 || Mesh[i][j].Value < Mesh[i][j + 1].Value;
                    bool smallerThanBottom = i == Mesh.Length - 1 || Mesh[i][j].Value < Mesh[i + 1][j].Value;
                    bool smallerThanLeft = j == 0 || Mesh[i][j].Value < Mesh[i][j - 1].Value;

                    if (smallerThanTop && smallerThanRight && smallerThanBottom && smallerThanLeft)
                    {
                        Mesh[i][j].IsLowestPoint = true;
                    }
                }
            }
        }

        public int GetRiskLevels()
        {
            int riskLevel = 0;
            for (int i = 0; i < Mesh.Length; i++)
            {
                for (int j = 0; j < Mesh[i].Length; j++)
                {
                    if (Mesh[i][j].IsLowestPoint)
                    {
                        riskLevel += 1 + Mesh[i][j].Value;
                    }
                }
            }
            return riskLevel;
        }

        public int GetBasins()
        {
            // get lowest points
            var lowestPoints = Mesh
                .SelectMany(x => x)
                .Where(x => x.IsLowestPoint);

            var basinSizes = new List<int>();
            foreach (Point lowestPoint in lowestPoints)
            {
                // initialize queue which will hold points that need to be visited
                var queue = new Queue<Point>();
                var seen = new List<Point>();

                // add lowest point to queue and visited
                queue.Enqueue(lowestPoint);
                seen.Add(lowestPoint);

                int res = 0;
                while (queue.Any())
                {
                    Point point = queue.Dequeue();
                    res++;

                    // get all non-visited neighbors and add them to the queue
                    var neighbors = GetNeighbors(point)
                        .Where(x => x.Value != 9)
                        .Where(x => !seen.Contains(x));

                    foreach (var neighbor in neighbors)
                    {
                        queue.Enqueue(neighbor);
                        seen.Add(neighbor);
                    }
                }

                basinSizes.Add(res);
            }

            // find 3 largest basins and multiply their sizes together
            return basinSizes
                .OrderByDescending(x => x)
                .Take(3)
                .Aggregate(1, (x, y) => x * y);
        }

        private List<Point> GetNeighbors(Point point)
        {
            var neighbors = new List<Point>();

            // top neighbor
            if (point.Y > 0)
            {
                neighbors.Add(Mesh[point.Y - 1][point.X]);
            }

            // right neighbor
            if (point.X < Mesh[0].Length - 1)
            {
                neighbors.Add(Mesh[point.Y][point.X + 1]);
            }

            // bottom neighbor
            if (point.Y < Mesh.Length - 1)
            {
                neighbors.Add(Mesh[point.Y + 1][point.X]);
            }

            // left neighbor
            if (point.X > 0)
            {
                neighbors.Add(Mesh[point.Y][point.X - 1]);
            }

            return neighbors;
        }
    }

    class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Value { get; set; }
        public bool IsLowestPoint { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is not Point)
            {
                return false;
            }

            Point point = (Point)obj;
            return this.X == point.X && this.Y == point.Y;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var result = 0;
                result = (result * 397) ^ X;
                result = (result * 397) ^ Y;
                result = (result * 397) ^ Value;
                result = (result * 397) ^ (IsLowestPoint ? 1 : 0);
                return result;
            }
        }
    }
}
