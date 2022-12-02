using Common;

namespace Day15
{
    internal class Program
    {
        static void Main()
        {
            string[] lines = File.ReadAllLines(Constants.FILE_NAME);

            Console.WriteLine($"Part 1 result: {GetLowestTotalRisk(GetMap(lines))}");
            Console.WriteLine($"Part 2 result: {GetLowestTotalRisk(GetScaledMap(GetMap(lines)))}");
        }

        private static long GetLowestTotalRisk(Dictionary<Pos, int> map)
        {
            // solve with Dijkstra algorithm
            var topLeft = new Pos(0, 0);
            var bottomRight = new Pos(map.Keys.MaxBy(x => x.X)!.X, map.Keys.MaxBy(x => x.Y)!.Y);

            // store cumulative risk for each point, visit points in order of lowest cumulative risk
            var pq = new PriorityQueue<Pos, int>();
            var totalRiskMap = new Dictionary<Pos, int>
            {
                [topLeft] = 0
            };

            pq.Enqueue(topLeft, 0);

            while (pq.Count > 0)
            {
                var p = pq.Dequeue();

                foreach (Pos neighbor in GetNeighbors(p))
                {
                    if (map.ContainsKey(neighbor) && !totalRiskMap.ContainsKey(neighbor))
                    {
                        int totalRisk = map[neighbor] + totalRiskMap[p];
                        totalRiskMap[neighbor] = totalRisk;
                        if (neighbor == bottomRight)
                        {
                            break;
                        }
                        pq.Enqueue(neighbor, totalRisk);
                    }
                }
            }
            
            return totalRiskMap[bottomRight];
        }

        private static Dictionary<Pos, int> GetMap(string[] lines)
        {
            // dictionary ensures that we can use 'ContainsKey' to check for positions on the map
            return new Dictionary<Pos, int>(
                from y in Enumerable.Range(0, lines[0].Length)
                from x in Enumerable.Range(0, lines.Length)
                select new KeyValuePair<Pos, int>(new Pos(x, y), lines[y][x] - '0')
            );
        }

        private static Dictionary<Pos, int> GetScaledMap(Dictionary<Pos, int> map)
        {
            int numCols = map.Keys.MaxBy(x => x.X)!.X + 1;
            int numRows = map.Keys.MaxBy(x => x.Y)!.Y + 1;
            var res = new Dictionary<Pos, int>();

            foreach (int y in Enumerable.Range(0, numRows * 5))
            {
                foreach (int x in Enumerable.Range(0, numCols * 5))
                {
                    // original map
                    int tileRiskLevel = map[new Pos(y % numCols, x % numRows)];

                    // risk level increases
                    int tileDistance = (y / numRows) + (x / numCols);
                    int newRiskLevel = (tileRiskLevel + tileDistance - 1) % 9 + 1;
                    res.Add(new Pos(x, y), newRiskLevel);
                }
            }

            return res;
        }

        private static IEnumerable<Pos> GetNeighbors(Pos pos)
        {
            return new List<Pos>()
            {
                new Pos(pos.X - 1, pos.Y),
                new Pos(pos.X + 1, pos.Y),
                new Pos(pos.X, pos.Y - 1),
                new Pos(pos.X, pos.Y + 1),
            };
        }
    }

    record Pos(int X, int Y);
}
