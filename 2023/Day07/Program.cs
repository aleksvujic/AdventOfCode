using Common;

var input = File.ReadAllLines(Constants.FILE_NAME)
    .Select(x => x.Split());

var hands1 = input
    .Select(x => Hand.Parse(x.First(), x.Last(), true))
    .ToArray();

Console.WriteLine($"Part 1 result: {GetResult(hands1)}");

var hands2 = input
    .Select(x => Hand.Parse(x.First(), x.Last(), false))
    .ToArray();

Console.WriteLine($"Part 2 result: {GetResult(hands2)}");

long GetResult(Hand[] hands)
{
    return hands
        .GroupBy(x => x.HandType)
        .OrderBy(x => x.Key)
        .SelectMany(x => x.OrderBy(y => y, new HandComparer()))
        .Select((x, idx) => (idx + 1) * x.Bid)
        .Sum();
}

record Hand(int[] Cards, int Bid, bool IsPart1)
{
    private static int _part1JokerVal = 11;
    private static int _part2JokerVal = 1;
    
    private static Dictionary<char, int> GetCardStrengths(bool isPart1)
    {
        return new Dictionary<char, int>()
        {
            { 'A', 14 },
            { 'K', 13 },
            { 'Q', 12 },
            { 'J', isPart1 ? _part1JokerVal : _part2JokerVal },
            { 'T', 10 },
            { '9', 9 },
            { '8', 8 },
            { '7', 7 },
            { '6', 6 },
            { '5', 5 },
            { '4', 4 },
            { '3', 3 },
            { '2', 2 },
        };
    }
    
    public static Hand Parse(string cards, string bid, bool isPart1)
    {
        var cardsArray = cards.ToCharArray()
            .Select(x => GetCardStrengths(isPart1)[x])
            .ToArray();
        
        return new Hand(cardsArray, int.Parse(bid), isPart1);
    }

    public int HandType
    {
        get
        {
            static bool CheckOccurences(Dictionary<int, int> groups, int[] frequencies)
            {
                return groups.Count == frequencies.Length && groups.Values.All(frequencies.Contains);
            }

            var bestHandType = int.MinValue;
            for (int jokerVal = (IsPart1 ? _part1JokerVal : GetCardStrengths(IsPart1).Values.Min()); jokerVal <= (IsPart1 ? _part1JokerVal : GetCardStrengths(IsPart1).Values.Max()); jokerVal++)
            {
                // replace joker with new value
                var groups = Cards
                    .Select(x => x == (IsPart1 ? _part1JokerVal : _part2JokerVal) ? jokerVal : x)
                    .GroupBy(x => x)
                    .ToDictionary(x => x.Key, x => x.Count());

                int res;

                // Five of a kind, where all five cards have the same label: AAAAA
                if (CheckOccurences(groups, new int[] { 5 }))
                    res = 6;
                // Four of a kind, where four cards have the same label and one card has a different label: AA8AA
                else if (CheckOccurences(groups, new int[] { 1, 4 }))
                    res = 5;
                // Full house, where three cards have the same label, and the remaining two cards share a different label: 23332
                else if (CheckOccurences(groups, new int[] { 2, 3 }))
                    res = 4;
                // Three of a kind, where three cards have the same label, and the remaining two cards are each different from any other card in the hand: TTT98
                else if (CheckOccurences(groups, new int[] { 1, 1, 3 }))
                    res = 3;
                // Two pair, where two cards share one label, two other cards share a second label, and the remaining card has a third label: 23432
                else if (CheckOccurences(groups, new int[] { 1, 2, 2 }))
                    res = 2;
                // One pair, where two cards share one label, and the other three cards have a different label from the pair and each other: A23A4
                else if (CheckOccurences(groups, new int[] { 1, 1, 1, 2 }))
                    res = 1;
                // High card, where all cards' labels are distinct: 23456
                else if (CheckOccurences(groups, new int[] { 1, 1, 1, 1, 1 }))
                    res = 0;
                else
                    res = -1;

                if (res > bestHandType)
                {
                    bestHandType = res;
                }
            }

            return bestHandType;
        }
    }
}

class HandComparer : IComparer<Hand>
{
    public int Compare(Hand? x, Hand? y)
    {
        for (int i = 0; i < x.Cards.Length; i++)
        {
            if (x.Cards[i] > y.Cards[i])
            {
                return 1;
            }
            else if (x.Cards[i] < y.Cards[i])
            {
                return -1;
            }
        }

        return 0;
    }
}