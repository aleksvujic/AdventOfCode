using Common;

namespace Day03
{
    class Program
    {
        static void Main()
        {
            string[] fileLines = File.ReadAllLines(Constants.FILE_NAME);

            // represent bits as 2D array of characters
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
                char[] binaryNumber = GetBinaryNumberForColumn(numbers, i);

                // extract most and least common bits
                var counts = binaryNumber
                    .GroupBy(x => x)
                    .OrderBy(x => x.Count())
                    .Select(x => x.Key)
                    .ToList();

                mostCommonBits += counts.Last();
                leastCommonBits += counts.First();
            }

            // convert both binary strings to integer
            return Convert.ToInt32(mostCommonBits, 2) * Convert.ToInt32(leastCommonBits, 2);
        }

        static int Part2(char[][] numbers)
        {
            return FilterValues(numbers, Criteria.MostCommon) * FilterValues(numbers, Criteria.LeastCommon);
        }

        static int FilterValues(char[][] numbers, Criteria criteria)
        {
            char[][] filteredValues = numbers
                .Select(x => x.ToArray())
                .ToArray();

            int currentColumnIndex = 0;
            while (filteredValues.Length > 1)
            {
                char[] binaryNumber = GetBinaryNumberForColumn(filteredValues, currentColumnIndex);

                // count occurences of each bit
                var groupedBinaryNumber = binaryNumber
                    .GroupBy(x => x);


                // decide which character to use based on criteria
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

                // remove values that don't have selected char at current column index
                filteredValues = filteredValues
                    .Where(x => x[currentColumnIndex] == selectedCharCriteria)
                    .ToArray();

                currentColumnIndex++;
            }

            return Convert.ToInt32(new string(filteredValues[0]), 2);
        }

        static char[] GetBinaryNumberForColumn(char[][] numbers, int colIndex)
        {
            // get all chars in one column from 2D array
            char[] binaryNumber = new char[numbers.Length];
            for (int j = 0; j < numbers.Length; j++)
            {
                binaryNumber[j] = numbers[j][colIndex];
            }
            return binaryNumber;
        }

        enum Criteria
        {
            MostCommon,
            LeastCommon
        }
    }
}
