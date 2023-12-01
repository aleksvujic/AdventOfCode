using Common;
using System.Drawing;

namespace Day05
{
    internal class Program
    {
        static void Main()
        {
            // read input to array of tuples
            Tuple<Point, Point>[] pointPairs = File.ReadAllLines(Constants.FILE_NAME)
                .Select(x =>
                {
                    int[] numbers = x
                        .Split(", ->".ToArray(), StringSplitOptions.RemoveEmptyEntries)
                        .Select(int.Parse)
                        .ToArray();
                    
                    return new Tuple<Point, Point>(
                        new Point(numbers[0], numbers[1]),
                        new Point(numbers[2], numbers[3])
                    );
                })
                .ToArray();

            Console.WriteLine($"Part 1 result: {GetResult(pointPairs, skipDiagonals: true)}");
            Console.WriteLine($"Part 2 result: {GetResult(pointPairs, skipDiagonals: false)}");
        }

        static int GetResult(Tuple<Point, Point>[] pointPairs, bool skipDiagonals)
        {
            List<Vec> coveredPoints = new();
            foreach ((Point start, Point end) in pointPairs)
            {
                // calculate line properties
                Vec displacement = new(end.X - start.X, end.Y - start.Y);
                int length = 1 + Math.Max(Math.Abs(displacement.X), Math.Abs(displacement.Y));
                Vec dir = new(Math.Sign(displacement.X), Math.Sign(displacement.Y));

                // represent line as a set of points
                var points = Enumerable.Range(0, length)
                    .Select(i => new Vec(start.X + i * dir.X, start.Y + i * dir.Y))
                    .Where(x => !skipDiagonals || dir.X == 0 || dir.Y == 0);

                coveredPoints.AddRange(points);
            }

            return GetDiagramResult(coveredPoints);
        }

        static int GetDiagramResult(IEnumerable<Vec> coveredPoints)
        {
            // count number of times each Vec appears in 'coveredPoints'
            return coveredPoints
                .GroupBy(x => x)
                .Where(x => x.Count() >= 2)
                .Select(x => x.Key)
                .Count();
        }
    }

    record Vec(int X, int Y);
}
