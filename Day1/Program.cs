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
            string inputFile = "input.txt";
            List<int> entries = File.ReadAllLines(inputFile)
                .Select(x => Int32.Parse(x))
                .ToList();

            for (int i = 0; i < entries.Count; i++)
            {
                for (int j = i + 1; j < entries.Count; j++)
                {
                    if (entries[i] + entries[j] == 2020)
                    {
                        Console.WriteLine(entries[i] * entries[j]);
                        return;
                    }
                }
            }
        }
    }
}
