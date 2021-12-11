using Common;

namespace Day11
{
    internal class Program
    {
        static void Main()
        {
            char[][] input = File.ReadAllLines(Constants.FILE_NAME)
                .Select(x => x.ToCharArray())
                .ToArray();

            Console.WriteLine($"Part 1 result: {FindCorruptedLines(input)}");
            Console.WriteLine($"Part 2 result: {CloseIncompleteLines(input)}");
        }
    }
}
