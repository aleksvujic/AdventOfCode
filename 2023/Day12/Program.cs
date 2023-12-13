using Common;

Console.WriteLine($"Part 1 result: {Solve(part2: false)}");
Console.WriteLine($"Part 1 result: {Solve(part2: true)}");

long Solve(bool part2)
{
    var springs = ParseInput(part2: part2);

    long res = 0;

    foreach (var spring in springs)
    {
        res += spring.GetNumValidConditions();
    }

    return res;
}

SpringsStatePair[] ParseInput(bool part2)
{
    return File.ReadAllLines(Constants.FILE_NAME)
        .Select(x =>
        {
            var split = x.Split();
            var conditions = split.First();
            var desiredState = split.Last();

            if (part2)
            {
                conditions = string.Join('?', Enumerable.Repeat($"{conditions}", 5));
                desiredState = string.Join(',', Enumerable.Repeat($"{desiredState}", 5));
            }

            return new SpringsStatePair(conditions, desiredState);
        })
        .ToArray();
}

record SpringsStatePair(string Springs, string DesiredState)
{
    public long GetNumValidConditions()
    {
        return GetNumValidConditions(Springs, DesiredState);
    }

    private long GetNumValidConditions(string springs, string groups)
    {
        if (GetNumGroups(groups) == 0)
        {
            return springs.Contains('#') ? 0 : 1;
        }
        else if (string.IsNullOrEmpty(springs) || GetFirstGroup(groups) > springs.Length)
        {
            return 0;
        }

        var firstChar = springs.First();
        switch (firstChar)
        {
            case '.':
                return GetNumValidConditions(springs[1 ..], groups);
            case '?':
                return
                    GetNumValidConditions(ReplaceAt(springs, 0, '.'), groups) +
                    GetNumValidConditions(ReplaceAt(springs, 0, '#'), groups);
            case '#':
                var groupLen = GetFirstGroup(groups);
                var group = springs[..groupLen];
                if (group.All(x => x != '.'))
                {
                    if (springs.Length == groupLen)
                    {
                        return GetNumGroups(groups) == 1 ? 1 : 0;
                    }

                    if (springs[groupLen] != '#')
                    {
                        return GetNumValidConditions($".{springs[(groupLen + 1)..]}", RemoveFirstGroup(groups));
                    }
                }

                return 0;
        }
        
        return 0;
    }

    private int GetNumGroups(string groups)
    {
        if (string.IsNullOrEmpty(groups))
        {
            return 0;
        }

        return groups.Split(",").Count();
    }

    private string RemoveFirstGroup(string groups)
    {
        if (string.IsNullOrEmpty(groups))
        {
            throw new Exception();
        }

        return string.Join(',', groups.Split(",").Skip(1));
    }

    int GetFirstGroup(string groups)
    {
        if (string.IsNullOrEmpty(groups))
        {
            throw new Exception();
        }

        return int.Parse(groups.Split(",").First());
    }

    private string ReplaceAt(string input, int index, char newChar)
    {
        char[] chars = input.ToCharArray();
        chars[index] = newChar;
        return new string(chars);
    }
};