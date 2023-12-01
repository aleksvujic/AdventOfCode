using Common;

namespace Day01
{
    internal class Program
    {
        static void Main()
        {
            List<int> measurements = File.ReadAllLines(Constants.FILE_NAME)
                .Select(x => int.Parse(x))
                .ToList();

            Console.WriteLine($"Increased measurements: {CountIncreases(measurements, 1)}");
            Console.WriteLine($"Increased measurements with sliding window of size 3: {CountIncreases(measurements, 3)}");
        }

        static int CountIncreases(List<int> measurements, int slidingWindowSize)
        {
            int previousSlidingWindowSum = -1;
            int increases = 0;

            for (int i = 0; i < measurements.Count - slidingWindowSize + 1; i++)
            {
                // calculate sum of all elements in siding window
                int slidingWindowSum = 0;
                for (int j = 0; j < slidingWindowSize; j++)
                {
                    slidingWindowSum += measurements[i + j];
                }

                // skip first measurement, comparison is not defined
                if (i > 0 && slidingWindowSum > previousSlidingWindowSum)
                {
                    increases++;
                }

                previousSlidingWindowSum = slidingWindowSum;
            }
            
            return increases;
        }
    }
}
