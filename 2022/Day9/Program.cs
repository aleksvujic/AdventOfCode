using Common;

var commands = File.ReadAllLines(Constants.FILE_NAME)
    .Select(x =>
    {
        var parts = x.Split();
        return new MoveCommand(Enum.Parse<EDirection>(parts[0]), int.Parse(parts[1]));
    })
    .ToList();

Console.WriteLine($"Part 1 solution: {CountPositions(commands, 2)}");
Console.WriteLine($"Part 2 solution: {CountPositions(commands, 10)}");

int CountPositions(List<MoveCommand> commands, int numKnots)
{
    // add all knots to the rope
    var rope = new List<Knot>();
    for (int i = 0; i < numKnots; i++)
    {
        rope.Add(new Knot(0, 0));
    }

    var visitedPositions = new HashSet<(int rowIdx, int colIdx)>();

    foreach (var command in commands)
    {
        // calculate delta x coordinate
        int dx = command.Direction switch
        {
            EDirection.R => 1,
            EDirection.L => -1,
            _ => 0
        };

        // calculate delta y coordinate
        int dy = command.Direction switch
        {
            EDirection.U => 1,
            EDirection.D => -1,
            _ => 0
        };

        for (int i = 0; i < command.Distance; i++)
        {
            // move first knot
            rope[0] = new Knot(rope[0].RowIdx + dx, rope[0].ColIdx + dy);

            // move other knots accordingly
            for (int j = 1; j < rope.Count; j++)
            {
                var current = rope[j];
                var prev = rope[j - 1];

                // check if distance is greater than 1
                if (Math.Abs(current.RowIdx - prev.RowIdx) > 1 || Math.Abs(current.ColIdx - prev.ColIdx) > 1)
                {
                    rope[j] = new Knot(
                        current.RowIdx + Math.Sign(prev.RowIdx - current.RowIdx),
                        current.ColIdx + Math.Sign(prev.ColIdx - current.ColIdx)
                    );
                }
            }

            // mark position as visited
            var lastKnot = rope[^1];
            visitedPositions.Add((lastKnot.RowIdx, lastKnot.ColIdx));
        }
    }
    
    return visitedPositions.Count;
}

record MoveCommand(EDirection Direction, int Distance);
record Knot(int RowIdx, int ColIdx);

enum EDirection
{
    U, // up
    R, // right
    D, // down
    L, // left
}