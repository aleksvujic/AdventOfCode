using Common;

namespace Day08
{
    internal class Program
    {
        static private readonly int[] allowedLengths = new int[] { 2, 3, 4, 7 };

        static void Main()
        {
            string[] input = File.ReadAllLines(Constants.FILE_NAME);

            Console.WriteLine($"Part 1 result: {Part1(input)}");
            Console.WriteLine($"Part 2 result: {Part2(input)}");
        }

        static int Part1(string[] input)
        {
            int count = 0;
            
            foreach (string line in input)
            {
                // extract whole words on the right of '|' symbol
                string[] numbers = line.Split('|')[1].Trim().Split(' ');
                foreach (string number in numbers)
                {
                    if (allowedLengths.Contains(number.Length))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        static int Part2(string[] input)
        {
            int count = 0;
            
            foreach (string line in input)
            {
                string[] parts = line.Split(" | ");
                HashSet<char>[] patterns = parts[0].Split(' ').Select(x => x.ToHashSet()).ToArray();

                // check what segments belong to each one of 10 digits (0-9)
                var digits = new HashSet<char>[10];

                // digits 1 and 4 can be determined by length
                digits[1] = patterns.Single(pattern => pattern.Count() == "cf".Length);
                digits[4] = patterns.Single(pattern => pattern.Count() == "bcdf".Length);

                digits[0] = Lookup(patterns, digits, 6, 2, 3);
                digits[2] = Lookup(patterns, digits, 5, 1, 2);
                digits[3] = Lookup(patterns, digits, 5, 2, 3);
                digits[5] = Lookup(patterns, digits, 5, 1, 3);
                digits[6] = Lookup(patterns, digits, 6, 1, 3);
                digits[7] = Lookup(patterns, digits, 3, 2, 2);
                digits[8] = Lookup(patterns, digits, 7, 2, 4);
                digits[9] = Lookup(patterns, digits, 6, 2, 4);

                count += parts[1]
                    .Split(' ')
                    .Aggregate(0, (n, digit) => n * 10 + DecodeDigit(digits, digit));
            }

            return count;
        }

        private static HashSet<char> Lookup(HashSet<char>[] patterns, HashSet<char>[] digits, int segmentCount, int commonWithOne, int commonWithFour)
        {
            // triplet (segmentCount, commonWithOne, commonWithFour) uniquely defines all other digits
            return patterns.Single(pattern =>
                pattern.Count == segmentCount &&
                pattern.Intersect(digits[1]).Count() == commonWithOne &&
                pattern.Intersect(digits[4]).Count() == commonWithFour
            );
        }

        private static int DecodeDigit(HashSet<char>[] digits, string s)
        {
            return Enumerable.Range(0, 10).Single(x => digits[x].SetEquals(s));
        }
    }
}
