using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day3
{
    class Program
    {
        static void Main()
        {
            string[] fileLines = File.ReadAllLines(Constants.FILE_NAME);

            char[][] numbers = new char[fileLines.Length][];
            for (int i = 0; i < fileLines.Length; i++)
            {
                numbers[i] = fileLines[i]
                    .ToCharArray();
            }

            Console.WriteLine($"Part 1 result: {Part1(numbers)}");
            Console.WriteLine($"Part 2 result: {Part2(numbers)}");
        }

        static int Part1(char[][] numbers)
        {
            string mostCommonBits = string.Empty;   
            string leastCommonBits = string.Empty;

            for (int i = 0; i < numbers[0].Length; i++)
            {
                char[] binaryNumber = new char[numbers.Length];
                for (int j = 0; j < numbers.Length; j++)
                {
                    binaryNumber[j] = numbers[j][i];
                }

                var counts = binaryNumber
                    .GroupBy(x => x)
                    .OrderBy(x => x.Count())
                    .Select(x => x.Key)
                    .ToList();

                mostCommonBits += counts.Last();
                leastCommonBits += counts.First();
            }

            return Convert.ToInt32(mostCommonBits, 2) * Convert.ToInt32(leastCommonBits, 2);
        }

        static int Part2(char[][] numbers)
        {
            return FilterValues(numbers, Criteria.MostCommon) * FilterValues(numbers, Criteria.LeastCommon);
        }

        static int FilterValues(char[][] numbers, Criteria criteria)
        {
            List<char[]> filteredValues = numbers.ToList();

            int currentPos = 0;
            while (filteredValues.Count > 1)
            {
                char[] binaryNumber = new char[filteredValues.Count];
                for (int j = 0; j < filteredValues.Count; j++)
                {
                    binaryNumber[j] = filteredValues[j][currentPos];
                }

                var groupedBinaryNumber = binaryNumber
                    .GroupBy(x => x);

                var selectedCharCriteria = criteria switch
                {
                    Criteria.MostCommon => groupedBinaryNumber
                        .OrderByDescending(x => x.Count())
                        .ThenByDescending(x => x.Key)
                        .Select(x => x.Key)
                        .First(),
                    Criteria.LeastCommon => groupedBinaryNumber
                        .OrderBy(x => x.Count())
                        .ThenBy(x => x.Key)
                        .Select(x => x.Key)
                        .First(),
                    _ => throw new Exception("Criteria not recognized"),
                };

                filteredValues = filteredValues
                    .Where(x => x[currentPos] == selectedCharCriteria)
                    .ToList();

                currentPos++;
            }

            return Convert.ToInt32(new string(filteredValues[0]), 2);
        }

        enum Criteria
        {
            MostCommon,
            LeastCommon
        }
    }
}
