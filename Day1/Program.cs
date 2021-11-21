using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day1
{
    internal class Program
    {
        static void Main()
        {
            int requiredSum = 2020;

            List<int> entries = File.ReadAllLines(Constants.FILE_NAME)
                .Select(x => Int32.Parse(x))
                .ToList();

            for (int i = 0; i < entries.Count; i++)
            {
                for (int j = i + 1; j < entries.Count; j++)
                {
                    // finding 2 numbers that sum up to 2020
                    if (entries[i] + entries[j] == requiredSum)
                    {
                        Console.WriteLine($"Two numbers result: {entries[i] * entries[j]}");
                    }

                    for (int k = j + 1; k < entries.Count; k++)
                    {
                        // finding 3 numbers that sum up to 2020
                        if (entries[i] + entries[j] + entries[k] == requiredSum)
                        {
                            Console.WriteLine($"Three numbers result: {entries[i] * entries[j] * entries[k]}");
                        }
                    }
                }
            }
        }
    }
}
