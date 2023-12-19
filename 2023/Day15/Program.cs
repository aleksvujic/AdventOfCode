using Common;
using System.Text.RegularExpressions;

Console.WriteLine($"Part 1 result: {Part1()}");
Console.WriteLine($"Part 2 result: {Part2()}");

long Part1()
{
    var sequences = ParseInput();
    return sequences
        .Sum(x => x.FullHash);
}

long Part2()
{
    List<List<Lens>> GetEmptyBoxes()
    {
        var res = new List<List<Lens>>();
        for (int i = 0; i < 256; i++)
        {
            res.Add([]);
        }
        return res;
    }

    var sequences = ParseInput();
    var boxes = GetEmptyBoxes();

    foreach (var sequence in sequences)
    {
        var box = boxes[sequence.LabelHash];
        var lensIdx = box.FindIndex(x => x.Label == sequence.Label);

        switch (sequence.OperationType)
        {
            case EOperationType.Add:
                var newLens = new Lens(sequence.Label, sequence.FocalLength!.Value);

                if (lensIdx < 0)
                {
                    box.Add(newLens);
                }
                else
                {
                    box[lensIdx] = newLens;
                }

                break;
            case EOperationType.Remove:
                if (lensIdx >= 0)
                {
                    box.RemoveAt(lensIdx);
                }

                break;
        }
    }

    return boxes
        .Select((lenses, boxIdx) =>
            lenses.Select((lens, lensIdx) =>
                (boxIdx + 1) * (lensIdx + 1) * lens.FocalLength
            )
        )
        .SelectMany(x => x)
        .Sum();
}

Operation[] ParseInput()
{
    return File.ReadAllText(Constants.FILE_NAME)
        .Split(',', StringSplitOptions.RemoveEmptyEntries)
        .Select(Operation.Parse)
        .ToArray();
}

enum EOperationType
{
    Add,
    Remove
}

record Lens(string Label, int FocalLength);

record Operation(string RawLabel, string Label, int? FocalLength, EOperationType OperationType)
{
    public int FullHash => GetHash(RawLabel);
    public int LabelHash => GetHash(Label);

    public static Operation Parse(string input)
    {
        var label = Regex.Replace(input, @"[^a-z]", string.Empty);
        int? focalLength = null;
        EOperationType operationType;

        if (input.Contains('='))
        {
            focalLength = int.Parse(input.Split('=').Last());
            operationType = EOperationType.Add;
        }
        else
        {
            operationType = EOperationType.Remove;
        }
        
        return new Operation(input, label, focalLength, operationType);
    }

    int GetHash(string input)
    {
        int hash = 0;

        for (int i = 0; i < input.Length; i++)
        {
            hash += input[i];
            hash *= 17;
            hash %= 256;
        }

        return hash;
    }
}