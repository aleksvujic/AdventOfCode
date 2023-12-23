using Common;
using Map = char[][];

Console.WriteLine($"Part 1 result: {Part1()}");
Console.WriteLine($"Part 1 result: {Part2()}");

long Part1()
{
    var map = ParseInput();
    return GetNumEnergizedCells(map, new Beam(new Pos(0, 0), EDirection.Right));
}

long Part2()
{
    var map = ParseInput();

    var top = Enumerable.Range(0, map[0].Length)
        .Select(j => new Beam(new Pos(0, j), EDirection.Down));
    var bottom = Enumerable.Range(0, map[0].Length)
        .Select(j => new Beam(new Pos(map.Length - 1, j), EDirection.Up));
    var left = Enumerable.Range(0, map.Length)
        .Select(i => new Beam(new Pos(i, 0), EDirection.Right));
    var right = Enumerable.Range(0, map.Length)
        .Select(i => new Beam(new Pos(i, map[i].Length - 1), EDirection.Left));

    var allStartingBeams = top.Concat(bottom).Concat(left).Concat(right);
    
    return allStartingBeams
        .Max(beam => GetNumEnergizedCells(map, beam));
}

long GetNumEnergizedCells(Map map, Beam beam)
{
    var seen = new HashSet<Beam>();
    var queue = new Queue<Beam>([beam]);
    
    while (queue.Any())
    {
        var b = queue.Dequeue();
        seen.Add(b);

        var newBeams = new List<Beam>();

        switch (map[b.Pos.I][b.Pos.J])
        {
            case '.':
                // empty space - pass through
                switch (b.Direction)
                {
                    case EDirection.Up:
                        newBeams.Add(new Beam(new Pos(b.Pos.I - 1, b.Pos.J), b.Direction));
                        break;
                    case EDirection.Right:
                        newBeams.Add(new Beam(new Pos(b.Pos.I, b.Pos.J + 1), b.Direction));
                        break;
                    case EDirection.Down:
                        newBeams.Add(new Beam(new Pos(b.Pos.I + 1, b.Pos.J), b.Direction));
                        break;
                    case EDirection.Left:
                        newBeams.Add(new Beam(new Pos(b.Pos.I, b.Pos.J - 1), b.Direction));
                        break;
                    default:
                        throw new Exception();
                }
                break;
            case '/':
                // mirror - reflect 90 degrees
                switch (b.Direction)
                {
                    case EDirection.Up:
                        newBeams.Add(new Beam(new Pos(b.Pos.I, b.Pos.J + 1), EDirection.Right));
                        break;
                    case EDirection.Right:
                        newBeams.Add(new Beam(new Pos(b.Pos.I - 1, b.Pos.J), EDirection.Up));
                        break;
                    case EDirection.Down:
                        newBeams.Add(new Beam(new Pos(b.Pos.I, b.Pos.J - 1), EDirection.Left));
                        break;
                    case EDirection.Left:
                        newBeams.Add(new Beam(new Pos(b.Pos.I + 1, b.Pos.J), EDirection.Down));
                        break;
                    default:
                        throw new Exception();
                }
                break;
            case '\\':
                // mirror - reflect 90 degrees
                switch (b.Direction)
                {
                    case EDirection.Up:
                        newBeams.Add(new Beam(new Pos(b.Pos.I, b.Pos.J - 1), EDirection.Left));
                        break;
                    case EDirection.Right:
                        newBeams.Add(new Beam(new Pos(b.Pos.I + 1, b.Pos.J), EDirection.Down));
                        break;
                    case EDirection.Down:
                        newBeams.Add(new Beam(new Pos(b.Pos.I, b.Pos.J + 1), EDirection.Right));
                        break;
                    case EDirection.Left:
                        newBeams.Add(new Beam(new Pos(b.Pos.I - 1, b.Pos.J), EDirection.Up));
                        break;
                    default:
                        throw new Exception();
                }
                break;
            case '|':
                // splitter
                switch (b.Direction)
                {
                    case EDirection.Right or EDirection.Left:
                        newBeams.Add(new Beam(new Pos(b.Pos.I - 1, b.Pos.J), EDirection.Up));
                        newBeams.Add(new Beam(new Pos(b.Pos.I + 1, b.Pos.J), EDirection.Down));
                        break;
                    case EDirection.Up:
                        newBeams.Add(new Beam(new Pos(b.Pos.I - 1, b.Pos.J), b.Direction));
                        break;
                    case EDirection.Down:
                        newBeams.Add(new Beam(new Pos(b.Pos.I + 1, b.Pos.J), b.Direction));
                        break;
                    default:
                        throw new Exception();
                }
                break;
            case '-':
                // splitter
                switch (b.Direction)
                {
                    case EDirection.Up or EDirection.Down:
                        newBeams.Add(new Beam(new Pos(b.Pos.I, b.Pos.J - 1), EDirection.Left));
                        newBeams.Add(new Beam(new Pos(b.Pos.I, b.Pos.J + 1), EDirection.Right));
                        break;
                    case EDirection.Right:
                        newBeams.Add(new Beam(new Pos(b.Pos.I, b.Pos.J + 1), b.Direction));
                        break;
                    case EDirection.Left:
                        newBeams.Add(new Beam(new Pos(b.Pos.I, b.Pos.J - 1), b.Direction));
                        break;
                    default:
                        throw new Exception();
                }
                break;
            default:
                throw new Exception();
        }

        foreach (var newBeam in newBeams)
        {
            if (newBeam.Pos.I >= 0 && newBeam.Pos.I < map.Length &&
                newBeam.Pos.J >= 0 && newBeam.Pos.J < map[0].Length &&
                !seen.Contains((newBeam)))
            {
                //queue.Enqueue(newBeam);
                queue.Enqueue(newBeam);
            }
        }
    }

    return seen
        .Select(x => x.Pos)
        .Distinct()
        .Count();
}

Map ParseInput()
{
    return File.ReadAllLines(Constants.FILE_NAME)
        .Select(x => x.ToCharArray())
        .ToArray();
}

record Pos(int I, int J);
record Beam(Pos Pos, EDirection Direction);

enum EDirection
{
    Up,
    Right,
    Down,
    Left
}
