using Common;

namespace Day17
{
    internal class Program
    {
        static void Main()
        {
            var input = File.ReadAllLines(Constants.FILE_NAME)
                .First()
                .Replace("target area: ", "")
                .Split(", ")
                .Select(x => x.Replace("x=", "").Replace("y=", ""))
                .Select(x => x.Split(".."))
                .SelectMany(x => x)
                .Select(int.Parse)
                .ToArray();

            var rect = new Rect(input[0], input[1], input[2], input[3]);

            Console.WriteLine($"Part 1 result: {Solve(rect).Max()}");
            Console.WriteLine($"Part 2 result: {Solve(rect).Count()}");
        }

        static IEnumerable<int> Solve(Rect target)
        {
            // bounds for initial horizontal and vertical speeds
            int vxMin = 0;
            int vxMax = target.MaxX;
            int vyMin = target.MinY;
            int vyMax = -target.MinY;
            
            for (int vx0 = vxMin; vx0 <= vxMax; vx0++)
            {
                for (int vy0 = vyMin; vy0 <= vyMax; vy0++)
                {
                    int x = 0;
                    int y = 0;
                    int vx = vx0;
                    int vy = vy0;
                    int maxY = 0;

                    // simulate steps while there is still chance to hit the target
                    while (x <= target.MaxX && y >= target.MinY)
                    {
                        x += vx;
                        y += vy;
                        vx = Math.Max(0, vx - 1);
                        vy -= 1;
                        maxY = Math.Max(y, maxY);
                        
                        if (target.IsPointInside(new Point(x, y)))
                        {
                            // point is inside the target
                            yield return maxY;
                            break;
                        }
                    }
                }
            }
        }
    }

    class Rect
    {
        public int MinX { get; set; }
        public int MaxX { get; set; }
        public int MinY { get; set; }
        public int MaxY { get; set; }
        
        public Rect(int minX, int maxX, int minY, int maxY)
        {
            MinX = minX;
            MaxX = maxX;
            MinY = minY;
            MaxY = maxY;
        }

        public bool IsPointInside(Point point)
        {
            return MinX <= point.X && point.X <= MaxX && MinY <= point.Y && point.Y <= MaxY;
        }
    }

    record Point(int X, int Y);
}
