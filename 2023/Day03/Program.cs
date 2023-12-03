using Common;
using System.Text.RegularExpressions;

internal class Program
{
    static void Main()
    {
        var lines = File.ReadAllLines(Constants.FILE_NAME);

        var symbols = GetParts(lines, new Regex(@"[^.0-9]"));
        var numbers = GetParts(lines, new Regex(@"\d+"));

        var res1 = numbers
            .Where(x => symbols.Any(y => AreAdjecent(y, x)))
            .Sum(x => x.IntVal);

        Console.WriteLine($"Part 1 result: {res1}");

        var gears = GetParts(lines, new Regex(@"\*"));

        var res2 = gears
            .Select(gear => numbers.Where(y => AreAdjecent(gear, y)))
            .Where(x => x.Count() == 2)
            .Select(x => x.First().IntVal * x.Last().IntVal)
            .Sum();

        Console.WriteLine($"Part 2 result: {res2}");
    }

    static Part[] GetParts(string[] rows, Regex regexp)
    {
        var parts = new List<Part>();
        for (int i = 0; i < rows.Length; i++)
        {
            foreach (Match match in regexp.Matches(rows[i]).Cast<Match>())
            {
                parts.Add(new Part(match.Value, i, match.Index));
            }
        }
        return parts.ToArray();
    }

    static bool AreAdjecent(Part part1, Part part2)
    {
        return
            part1.J <= part2.J + part2.Val.Length &&
            part2.J <= part1.J + part1.Val.Length &&
            Math.Abs(part1.I - part2.I) <= 1;
    }

    record Part(string Val, int I, int J)
    {
        public int IntVal => int.Parse(Val);
    };
}
