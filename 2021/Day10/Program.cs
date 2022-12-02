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

        static readonly Dictionary<char, int> _closingCharacterReward = new()
        {
            { ')', 1 },
            { ']', 2 },
            { '}', 3 },
            { '>', 4 }
        };

        static void Main()
        {
            char[][] input = File.ReadAllLines(Constants.FILE_NAME)
                .Select(x => x.ToCharArray())
                .ToArray();

            Console.WriteLine($"Part 1 result: {FindCorruptedLines(input)}");
            Console.WriteLine($"Part 2 result: {CloseIncompleteLines(input)}");
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

        static long CloseIncompleteLines(char[][] input)
        {
            List<long> scores = new();

            foreach (char[] line in input)
            {
                // stack data structure is based on LIFO
                Stack<char> stack = new();
                bool corruptedLine = false;

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
                            corruptedLine = true;
                            break;
                        }
                    }
                }

                // ignore corrupted lines
                if (corruptedLine)
                {
                    continue;
                }

                // try to fix the line
                long score = 0;
                while (stack.Count > 0)
                {
                    char symbol = stack.Pop();
                    score = score * 5 + _closingCharacterReward[_openingAndClosingPairs[symbol]];
                }
                scores.Add(score);
            }

            // return middle value
            return scores.OrderBy(x => x).ToList()[scores.Count / 2];
        }
    }
}
