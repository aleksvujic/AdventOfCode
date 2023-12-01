using Common;

var rounds = File.ReadAllLines(Constants.FILE_NAME)
    .Select(x => new Round(x[0], x[2]))
    .ToList();

var myChoiceScores = new Dictionary<char, int>()
{
    { 'X', 1 },
    { 'Y', 2 },
    { 'Z', 3 },
};

var roundRules = new Dictionary<Round, ERoundResult>()
{
    { new Round('A', 'X'), ERoundResult.Draw },
    { new Round('A', 'Y'), ERoundResult.Win },
    { new Round('A', 'Z'), ERoundResult.Loose },

    { new Round('B', 'X'), ERoundResult.Loose },
    { new Round('B', 'Y'), ERoundResult.Draw },
    { new Round('B', 'Z'), ERoundResult.Win },

    { new Round('C', 'X'), ERoundResult.Win },
    { new Round('C', 'Y'), ERoundResult.Loose },
    { new Round('C', 'Z'), ERoundResult.Draw }
};

Part1(rounds);
Part2(rounds);

void Part1(List<Round> rounds)
{
    int score = 0;
    foreach (var round in rounds)
    {
        score += (int)roundRules[round] + myChoiceScores[round.Second];
    }

    Console.WriteLine($"Total score part 1: {score}");
}

void Part2(List<Round> rounds)
{
    char FindNecessaryMoveForResult(char opponent, ERoundResult roundResult)
    {
        return roundRules
            .FirstOrDefault(x => x.Value == roundResult && x.Key.First == opponent)
            .Key.Second;
    }
    
    int score = 0;
    foreach (var round in rounds)
    {
        var expectedRes = round.Second switch
        {
            'X' => ERoundResult.Loose, // we need to loose
            'Y' => ERoundResult.Draw, // we need to end in a draw
            'Z' => ERoundResult.Win, // we need to win
            _ => throw new Exception($"Round {round.Second} is not recognized"),
        };

        score += (int)expectedRes + myChoiceScores[FindNecessaryMoveForResult(round.First, expectedRes)];
    }

    Console.WriteLine($"Total score part 2: {score}");
}

record Round(char First, char Second);

enum ERoundResult
{
    Loose = 0,
    Draw = 3,
    Win = 6
}