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
        }
    }

    class Grid
    {
        public Point[][] Mesh { get; }

        public Grid(string[] lines)
        {
            Mesh = lines.Select(x => x
                .ToCharArray()
                .Select(y => new Point()
                {
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
                    bool smallerThanTop = i == 0 || Mesh[i][j].Value < Mesh[i-1][j].Value;
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
    }

    class Point
    {
        public int Value { get; set; }
        public bool IsLowestPoint { get; set; }
    }
}
