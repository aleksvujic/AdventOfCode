using Common;

namespace Day10
{
    internal class Program
    {
        static readonly HashSet<char> openingSymbols = new() { '(', '[', '{', '<' };

        static readonly HashSet<char> closingSymbols = new() { ')', ']', '}', '>' };

        static readonly Dictionary<char, char> _openingAndClosingPairs = openingSymbols
            .Zip(closingSymbols)
            .ToDictionary(x => x.First, x => x.Second);

        static readonly Dictionary<char, int> _illegalCharacterPenalty = new()
        {
            { ')', 3 },
            { ']', 57 },
            { '}', 1197 },
            { '>', 25137 }
        };

        static void Main()
        {
            char[][] input = File.ReadAllLines(Constants.FILE_NAME)
                .Select(x => x.ToCharArray())
                .ToArray();

            Console.WriteLine($"Part 1 result: {FindCorruptedLines(input)}");
        }

        static int FindCorruptedLines(char[][] input)
        {
            int errorScore = 0;

            foreach (char[] line in input)
            {
                // stack data structure is based on LIFO
                Stack<char> stack = new();

                foreach (char symbol in line)
                {
                    if (openingSymbols.Contains(symbol))
                    {
                        // opening symbol - add it to stack
                        stack.Push(symbol);
                    }
                    else if (closingSymbols.Contains(symbol))
                    {
                        if (_openingAndClosingPairs[stack.Peek()] == symbol)
                        {
                            // closing symbol matches with top of the stack, chunk correctly closed
                            stack.Pop();
                        }
                        else
                        {
                            // closing symbol doesn't match with top of the stack, report error
                            errorScore += _illegalCharacterPenalty[symbol];
                            break;
                        }
                    }
                }
            }
            
            return errorScore;
        }
    }
}
