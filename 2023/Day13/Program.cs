using Common;

Console.WriteLine($"Part 1 result: {Part1()}");
Console.WriteLine($"Part 2 result: {Part2()}");

long Part1()
{
    var patterns = ParseInput();

    long colsRes = 0;
    long rowsRes = 0;
    foreach (var pattern in patterns)
    {
        colsRes += GetVerticalSymmetry(pattern);
        rowsRes += GetHorizontalSymmetry(pattern);
    }

    return colsRes + 100 * rowsRes;
}

long Part2()
{
    var patterns = ParseInput();

    long colsRes = 0;
    long rowsRes = 0;
    foreach (var pattern in patterns)
    {
        var originalVerticalSymmetry = GetVerticalSymmetry(pattern);
        var originalHorizontalSymmetry = GetHorizontalSymmetry(pattern);
        var smudgeFound = false;
        
        for (int rowIdx = 0; rowIdx < pattern.Length; rowIdx++)
        {
            for (int colIdx = 0; colIdx < pattern[rowIdx].Length; colIdx++)
            {
                var origSymbol = pattern[rowIdx][colIdx];
                
                // change symbol
                pattern[rowIdx][colIdx] = origSymbol == '.' ? '#' : '.';
                
                var newVerticalSymmetry = GetVerticalSymmetry(pattern, excludeRes: originalVerticalSymmetry);
                var newHorizontalSymmetry = GetHorizontalSymmetry(pattern, excludeRes: originalHorizontalSymmetry);

                if (newVerticalSymmetry > 0 && originalVerticalSymmetry != newVerticalSymmetry)
                {
                    colsRes += newVerticalSymmetry;
                    smudgeFound = true;
                }
                else if (newHorizontalSymmetry > 0 && originalHorizontalSymmetry != newHorizontalSymmetry)
                {
                    rowsRes += newHorizontalSymmetry;
                    smudgeFound = true;
                }

                // restore symbol value
                pattern[rowIdx][colIdx] = origSymbol;

                if (smudgeFound)
                {
                    break;
                }
            }

            if (smudgeFound)
            {
                break;
            }
        }
    }

    return colsRes + 100 * rowsRes;
}

long GetVerticalSymmetry(char[][] pattern, long? excludeRes = null)
{
    for (int colIdx = 1; colIdx < pattern[0].Length; colIdx++)
    {
        var symmetryFound = true;

        for (int offset = 0; offset < Math.Min(colIdx, pattern[0].Length - colIdx); offset++)
        {
            for (int rowIdx = 0; rowIdx < pattern.Length; rowIdx++)
            {
                if (pattern[rowIdx][colIdx - offset - 1] != pattern[rowIdx][colIdx + offset])
                {
                    // move to next colIdx
                    symmetryFound = false;
                    break;
                }
            }

            if (!symmetryFound)
            {
                break;
            }
        }

        if (symmetryFound && (!excludeRes.HasValue || excludeRes.Value != colIdx))
        {
            return colIdx;
        }
    }

    return 0;
}

long GetHorizontalSymmetry(char[][] pattern, long? excludeRes = null)
{
    for (int rowIdx = 1; rowIdx < pattern.Length; rowIdx++)
    {
        var symmetryFound = true;

        for (int offset = 0; offset < Math.Min(rowIdx, pattern.Length - rowIdx); offset++)
        {
            for (int colIdx = 0; colIdx < pattern[rowIdx].Length; colIdx++)
            {
                if (pattern[rowIdx - offset - 1][colIdx] != pattern[rowIdx + offset][colIdx])
                {
                    // move to next rowIdx
                    symmetryFound = false;
                    break;
                }
            }

            if (!symmetryFound)
            {
                break;
            }
        }

        if (symmetryFound && (!excludeRes.HasValue || excludeRes.Value != rowIdx))
        {
            return rowIdx;
        }
    }

    return 0;
}

char[][][] ParseInput()
{
    var lines = File.ReadAllLines(Constants.FILE_NAME);

    List<char[][]> patterns = [];
    List<char[]> pattern = [];
    for (int i = 0; i < lines.Length; i++)
    {
        var line = lines[i];

        if (!string.IsNullOrEmpty(line))
        {
            pattern.Add(line.ToCharArray());
        }

        var flush = string.IsNullOrEmpty(line) || i == lines.Length - 1;
        if (flush)
        {
            patterns.Add(pattern.ToArray());
            pattern = [];
        }
    }

    return patterns.ToArray();
}