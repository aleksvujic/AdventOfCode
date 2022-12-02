using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day6
{
    internal class Program
    {
        static void Main()
        {
            long[] fishInternalTimer = new long[9];
            foreach (string internalTimer in File.ReadLines(Constants.FILE_NAME).FirstOrDefault().Split(','))
            {
                fishInternalTimer[int.Parse(internalTimer)]++;
            }

            Console.WriteLine($"Part 1 result: {SimulateReproduction(fishInternalTimer.ToArray(), 80)}");
            Console.WriteLine($"Part 2 result: {SimulateReproduction(fishInternalTimer.ToArray(), 256)}");
        }

        static long SimulateReproduction(long[] fishInternalTimer, int numberOfDays)
        {
            int newFishValue = 8;

            for (int i = 0; i < numberOfDays; i++)
            {
                long previouslyZero = fishInternalTimer[0];
                for (int j = 1; j < fishInternalTimer.Length; j++)
                {
                    // shift values by 1 to the left
                    fishInternalTimer[j - 1] = fishInternalTimer[j];
                }

                // reset internal value of fish with 'newFishValue'
                fishInternalTimer[newFishValue] = 0;

                // update fish with internal values 6 and 8
                fishInternalTimer[newFishValue - 2] += previouslyZero;
                fishInternalTimer[newFishValue] += previouslyZero;
            }

            return fishInternalTimer.Sum();
        }
    }
}
