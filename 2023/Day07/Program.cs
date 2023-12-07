using Common;

var hands = File.ReadAllLines(Constants.FILE_NAME)
    .Select(x => x.Split())
    .Select(x => Hand.Parse(x.First(), x.Last()))
    .ToArray();

Console.WriteLine($"Part 1 result: {Part1(hands)}");

long Part1(Hand[] hands)
{
    return hands
        .GroupBy(x => x.HandType)
        .OrderBy(x => x.Key)
        .SelectMany(x => x.OrderBy(y => y, new HandComparer()))
        .Select((x, idx) => (idx + 1) * x.Bid)
        .Sum();
}

record Hand(int[] Cards, int Bid)
{
    private static readonly Dictionary<char, int> _cardStrength = new()
    {
        { 'A', 14 },
        { 'K', 13 },
        { 'Q', 12 },
        { 'J', 11 },
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
    
    public static Hand Parse(string cards, string bid)
    {
        var cardsArray = cards.ToCharArray()
            .Select(x => _cardStrength[x])
            .ToArray();
        
        return new Hand(cardsArray, int.Parse(bid));
    }

    public int HandType
    {
        get
        {
            static bool CheckOccurences(Dictionary<int, int> groups, int[] frequencies)
            {
                return groups.Count == frequencies.Length && groups.Values.All(frequencies.Contains);
            }

            var groups = Cards
                .GroupBy(x => x)
                .ToDictionary(x => x.Key, x => x.Count());

            // Five of a kind, where all five cards have the same label: AAAAA
            if (CheckOccurences(groups, new int[] { 5 }))
                return 6;
            // Four of a kind, where four cards have the same label and one card has a different label: AA8AA
            else if (CheckOccurences(groups, new int[] { 1, 4 }))
                return 5;
            // Full house, where three cards have the same label, and the remaining two cards share a different label: 23332
            else if (CheckOccurences(groups, new int[] { 2, 3 }))
                return 4;
            // Three of a kind, where three cards have the same label, and the remaining two cards are each different from any other card in the hand: TTT98
            else if (CheckOccurences(groups, new int[] { 1, 1, 3 }))
                return 3;
            // Two pair, where two cards share one label, two other cards share a second label, and the remaining card has a third label: 23432
            else if (CheckOccurences(groups, new int[] { 1, 2, 2 }))
                return 2;
            // One pair, where two cards share one label, and the other three cards have a different label from the pair and each other: A23A4
            else if (CheckOccurences(groups, new int[] { 1, 1, 1, 2 }))
                return 1;
            // High card, where all cards' labels are distinct: 23456
            else if (CheckOccurences(groups, new int[] { 1, 1, 1, 1, 1 }))
                return 0;
            else
                return -1;
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