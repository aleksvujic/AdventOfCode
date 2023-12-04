using Common;
using System.Text.RegularExpressions;

var cards = File.ReadAllLines(Constants.FILE_NAME)
    .Select(Card.ParseLine)
    .ToArray();

Console.WriteLine($"Part 1 result = {cards.Sum(x => x.Points)}");

var queue = new Queue<Card>();
Array.ForEach(cards, queue.Enqueue);
var dequeueCount = 0;
while (queue.Count > 0)
{
    var card = queue.Dequeue();
    dequeueCount++;
    if (card.NumPlayerWinningNumbers > 0)
    {
        for (int i = 0; i < card.NumPlayerWinningNumbers; i++)
        {
            queue.Enqueue(cards[card.Index + i]);
        }
    }
}

Console.WriteLine($"Part 2 result = {dequeueCount}");

record Card(int Index, int[] WinningNumbers, int[] PlayerNumbers)
{
    public static Card ParseLine(string line)
    {
        var index = int.Parse(new Regex(@"(\d+):").Match(line).Groups[1].Value);
        
        var parts = Regex.Replace(line, @"Card\s*\d+:", string.Empty)
            .Split("|")
            .Select(x => x
                .Trim()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray()
            );

        return new Card(index, parts.First(), parts.Last());
    }

    public int NumPlayerWinningNumbers => WinningNumbers.Intersect(PlayerNumbers).Count();

    public int Points
    {
        get
        {
            if (NumPlayerWinningNumbers == 0)
            {
                return 0;
            }

            return (int)Math.Pow(2, NumPlayerWinningNumbers - 1);
        }
    }
};