using Common;

Console.WriteLine($"Part 1 result: {Part1()}");
Console.WriteLine($"Part 2 result: {Part2()}");

long Part1()
{
    var input = ParseInput();
    return Dijkstra(input, 0, 3);
}

long Part2()
{
    var input = ParseInput();
    return Dijkstra(input, 4, 10);
}

long Dijkstra(int[][] input, int minSteps, int maxSteps)
{
    IEnumerable<State> GetNeighbors(State state)
    {
        if (state.NumMoves < maxSteps)
        {
            // continue straight
            int offI = 0;
            int offJ = 0;
            switch (state.Direction)
            {
                case EDirection.Up:
                    offI = -1;
                    break;
                case EDirection.Right:
                    offJ = 1;
                    break;
                case EDirection.Down:
                    offI = 1;
                    break;
                case EDirection.Left:
                    offJ = -1;
                    break;
                default:
                    throw new Exception();
            }

            yield return state with
            {
                I = state.I + offI,
                J = state.J + offJ,
                NumMoves = state.NumMoves + 1
            };
        }

        if (state.NumMoves >= minSteps)
        {
            // can also turn left and right
            switch (state.Direction)
            {
                case EDirection.Up or EDirection.Down:
                    yield return new State(state.I, state.J - 1, EDirection.Left, 1);
                    yield return new State(state.I, state.J + 1, EDirection.Right, 1);
                    break;
                case EDirection.Right or EDirection.Left:
                    yield return new State(state.I - 1, state.J, EDirection.Up, 1);
                    yield return new State(state.I + 1, state.J, EDirection.Down, 1);
                    break;
                default:
                    throw new Exception();
            }
        }
    }

    var visited = new HashSet<State>();
    var pq = new PriorityQueue<State, int>();

    var finish = new Pos(input.Length - 1, input[0].Length - 1);
    
    pq.Enqueue(new State(0, 0, EDirection.Right, 0), 0);
    pq.Enqueue(new State(0, 0, EDirection.Down, 0), 0);

    while (pq.TryDequeue(out State state, out int heatLoss))
    {
        if (state.GetPos() == finish && state.NumMoves >= minSteps)
        {
            return heatLoss;
        }

        foreach (var neighbor in GetNeighbors(state))
        {
            if (neighbor.I >= 0 && neighbor.I < input.Length &&
                neighbor.J >= 0 && neighbor.J < input[0].Length &&
                !visited.Contains(neighbor))
            {
                visited.Add(neighbor);
                pq.Enqueue(neighbor, heatLoss + input[neighbor.I][neighbor.J]);
            }
        }
    }

    throw new Exception("Finish not found");
}

int[][] ParseInput()
{
    return File.ReadAllLines(Constants.FILE_NAME)
        .Select(x => x.ToCharArray().Select(x => x - '0').ToArray())
        .ToArray();
}

enum EDirection
{
    Up,
    Right,
    Down,
    Left
}

record Pos(int I, int J);

record State(int I, int J, EDirection Direction, int NumMoves)
{
    public Pos GetPos()
    {
        return new Pos(I, J);
    }
};
