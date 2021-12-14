using Common;

namespace Day14
{
    internal class Program
    {
        static void Main()
        {
            string[] lines = File.ReadAllLines(Constants.FILE_NAME);

            string pattern = lines.First();
            Dictionary<string, string> insertions = lines
                .Where(x => x.Contains("->"))
                .Select(x => x.Split(" -> "))
                .ToDictionary(x => x[0], x => x[1]);

            Console.WriteLine($"Part 1 result: {Solve(pattern, insertions, 10)}");
            Console.WriteLine($"Part 2 result: {Solve(pattern, insertions, 40)}");
        }

        static long Solve(string pattern, Dictionary<string, string> insertions, int steps)
        {
            Dictionary<string, long> counter = GetCounter(pattern);
            
            for (int i = 0; i < steps; i++)
            {
                var next = new Dictionary<string, long>();
                foreach (var (pair, value) in counter)
                {
                    if (!insertions.ContainsKey(pair))
                    {
                        continue;
                    }

                    string middle = insertions[pair];
                    var key = pair[0] + middle;
                    next[key] = next.ContainsKey(key) ? next[key] + value : value;
                    key = middle + pair[1];
                    next[key] = next.ContainsKey(key) ? next[key] + value : value;
                }
                counter = next;
            }

            return GetScore(pattern, counter);
        }

        static private Dictionary<string, long> GetCounter(string pattern)
        {
            var counter = new Dictionary<string, long>();
            for (var i = 0; i < pattern.Length - 1; i++)
            {
                var key = pattern.Substring(i, 2);
                counter[key] = counter.ContainsKey(key) ? counter[key] + 1 : 1;
            }
            return counter;
        }

        static private long GetScore(string pattern, Dictionary<string, long> counter)
        {
            long[] allCounts = new long[26];
            foreach (var (pair, val) in counter)
            {
                allCounts[pair[0] - 'A'] += val;
            }
            allCounts[pattern[pattern.Length - 1] - 'A'] += 1;
            var filteredCounts = allCounts.Where(x => x > 0);
            return filteredCounts.Max() - filteredCounts.Min();
        }
    }
}
