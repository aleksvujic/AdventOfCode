using Common;
using System.Text.RegularExpressions;

namespace Day04
{
    internal class Program
    {
        static void Main()
        {
            var allLines = File.ReadAllLines(Constants.FILE_NAME);

            var res1 = ParseValues(allLines);

            Console.WriteLine($"Part 1: {res1.Sum()}");

            // strings can overlap, this avoids any edge cases (ex. eightwo = 82)
            var replacementsMap = new Dictionary<string, string>()
            {
                { "one", "o1e" },
                { "two", "t2o" },
                { "three", "th3ee" },
                { "four", "fo4r" },
                { "five", "fi5e" },
                { "six", "s6x" },
                { "seven", "se7en" },
                { "eight", "ei8ht" },
                { "nine", "ni9e" },
            };

            var replacedCalibrationValues = allLines
                .Select(x => replacementsMap.Aggregate(x, (current, dictEntry) => current.Replace(dictEntry.Key, dictEntry.Value)));

            var res2 = ParseValues(replacedCalibrationValues.ToArray());

            Console.WriteLine($"Part 2: {res2.Sum()}");
        }

        private static List<int> ParseValues(string[] values)
        {
            return values
                .Select(x => Regex.Replace(x, "[^0-9]", ""))
                .Select(x => x.Any() ? int.Parse($"{x.First()}{x.Last()}") : 0)
                .ToList();
        }
    }
}