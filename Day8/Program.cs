using Common;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day8
{
    internal class Program
    {
        private static readonly string wholeWordRegex = @"(\w+)";

        static void Main()
        {
            string[] input = File.ReadAllLines(Constants.FILE_NAME);

            Console.WriteLine($"Part 1 result: {CountDigits(input)}");
        }

        static int CountDigits(string[] input)
        {
            int count = 0;
            int[] allowedLengths = new int[] { 2, 3, 4, 7 };
            
            foreach (string line in input)
            {
                // get numbers on the right side of '|' symbol
                string numbers = line.Split('|')[1];

                // extract whole words
                MatchCollection matches = Regex.Matches(numbers, wholeWordRegex);
                foreach (Match match in matches)
                {
                    var matchValue = match.Value;
                    if (allowedLengths.Contains(matchValue.Length))
                    {
                        count++;
                    }
                }
            }

            return count;
        }
    }
}
