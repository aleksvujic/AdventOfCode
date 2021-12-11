using Common;

namespace Day11
{
    internal class Program
    {
        static void Main()
        {
            string[] lines = File.ReadAllLines(Constants.FILE_NAME);

            Console.WriteLine($"Part 1 result: {RunSimulation(lines).Take(100).Sum()}");
            Console.WriteLine($"Part 2 result: {RunSimulation(lines).TakeWhile(x => x != 100).Count() + 1}");
        }

        static IEnumerable<int> RunSimulation(string[] lines)
        {
            Dictionary<Pos, int> map = GetMap(lines);

            while (true)
            {
                var queue = new Queue<Pos>();
                var flashed = new HashSet<Pos>();

                // increase the energy level of each by 1
                foreach (var key in map.Keys)
                {
                    map[key]++;
                    if (map[key] == 10)
                    {
                        queue.Enqueue(key);
                    }
                }

                // visit all items that are added to the queue
                while (queue.Any())
                {
                    // dequeue position and add it to flashed list
                    var pos = queue.Dequeue();
                    flashed.Add(pos);

                    // get only positions that are on the map (some indices fall out of it)
                    foreach (Pos neighbor in GetNeighbors(pos).Where(x => map.ContainsKey(x)))
                    {
                        map[neighbor]++;
                        if (map[neighbor] == 10)
                        {
                            queue.Enqueue(neighbor);
                        }
                    }
                }

                // reset values of flashed positions to 0
                foreach (Pos flashedPos in flashed)
                {
                    map[flashedPos] = 0;
                }

                // return number of octopuses that have been flashed during this step
                yield return flashed.Count;
            }
        }

        static Dictionary<Pos, int> GetMap(string[] lines)
        {
            // dictionary ensures that we can use 'ContainsKey' to check for positions on the map
            return new Dictionary<Pos, int>(
                from y in Enumerable.Range(0, lines[0].Length)
                from x in Enumerable.Range(0, lines.Length)
                select new KeyValuePair<Pos, int>(new Pos(x, y), lines[y][x] - '0')
            );
        }

        static IEnumerable<Pos> GetNeighbors(Pos pos)
        {
            int[] displacements = new int[] { -1, 0, 1 };
            var neighbors = new List<Pos>();

            foreach (int dx in displacements)
            {
                foreach (int dy in displacements)
                {
                    if (dx != 0 || dy != 0)
                    {
                        neighbors.Add(new Pos(pos.X + dx, pos.Y + dy));
                    }
                }
            }

            return neighbors;
        }
    }

    record Pos(int X, int Y);
}
