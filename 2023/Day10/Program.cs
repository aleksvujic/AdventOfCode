using Common;
using Map = System.Collections.Generic.Dictionary<Point, Pipe>;

Console.WriteLine($"Part 1 result: {Part1()}");
Console.WriteLine($"Part 2 result: {Part2()}");

long Part1()
{
    var map = ParseInput(File.ReadAllLines(Constants.FILE_NAME));
    return GetPath(map).Count / 2;
}

long Part2()
{
    var map = ParseInput(File.ReadAllLines(Constants.FILE_NAME));
    var path = GetPath(map);

    var minX = path.Min(p => p.X);
    var maxX = path.Max(p => p.X);
    var minY = path.Min(p => p.Y);
    var maxY = path.Max(p => p.Y);

    long res = 0;

    for (var y = minY; y <= maxY; y++)
    {
        var isInside = false;
        for (var x = minX; x <= maxX; x++)
        {
            var p = new Point(x, y);

            if (path.Contains(p))
            {
                if ("|F7".Contains(map[p].Symbol))
                {
                    isInside = !isInside;
                }
            }
            else if (isInside)
            {
                res++;
            }
        }
    }

    return res;
}

Map ParseInput(string[] lines)
{
    var map = new Map();

    for (int rowIdx = 0; rowIdx < lines.Length; rowIdx++)
    {
        for (int colIdx = 0; colIdx < lines[rowIdx].Length; colIdx++)
        {
            map[new Point(colIdx, rowIdx)] = new Pipe(lines[rowIdx][colIdx]);
        }
    }

    return map;
}

List<Point> GetPath(Map map)
{
    var start = map.First(x => x.Value.Symbol == 'S').Key;
    var path = new List<Point>() { start };
    var startPipe = GetPipeType(map, start);
    map[start] = startPipe;
    var prev = start;
    var curr = startPipe.Adjacent(start).First();
    path.Add(curr);

    while (curr != start)
    {
        var next = map[curr].Adjacent(curr).First(x => x != prev);
        prev = curr;
        curr = next;
        path.Add(curr);
    }

    return path;
}

Pipe GetPipeType(Map map, Point start)
{
    var up = map.TryGetValue(start.Up, out var upPipe) ? upPipe : null;
    var right = map.TryGetValue(start.Right, out var rightPipe) ? rightPipe : null;
    var down = map.TryGetValue(start.Down, out var downPipe) ? downPipe : null;
    var left = map.TryGetValue(start.Left, out var leftPipe) ? leftPipe : null;

    if (up != null && up.Adjacent(start.Up).Contains(start) && right != null && right.Adjacent(start.Right).Contains(start))
    {
        return new Pipe('L');
    }
    
    if (up != null && up.Adjacent(start.Up).Contains(start) && down != null && down.Adjacent(start.Down).Contains(start))
    {
        return new Pipe('|');
    }
    
    if (up != null && up.Adjacent(start.Up).Contains(start) && left != null && left.Adjacent(start.Left).Contains(start))
    {
        return new Pipe('J');
    }
    
    if (right != null && right.Adjacent(start.Right).Contains(start) && down != null && down.Adjacent(start.Down).Contains(start))
    {
        return new Pipe('F');
    }
    
    if (right != null && right.Adjacent(start.Right).Contains(start) && left != null && left.Adjacent(start.Left).Contains(start))
    {
        return new Pipe('-');
    }
    
    if (down != null && down.Adjacent(start.Down).Contains(start) && left != null && left.Adjacent(start.Left).Contains(start))
    {
        return new Pipe('7');
    }

    throw new Exception($"Unknown pipe at {start}");
}

record Pipe(char Symbol)
{
    public IEnumerable<Point> Adjacent(Point at)
    {
        return Symbol switch
        {
            'L' => [at.Up, at.Right],
            'J' => [at.Up, at.Left],
            '|' => [at.Up, at.Down],
            '-' => [at.Left, at.Right],
            '7' => [at.Left, at.Down],
            'F' => [at.Right, at.Down],
            'S' => [],
            '.' => [],
            _ => throw new Exception($"Unknown pipe: {Symbol}")
        };
    }
}

record Point(int X, int Y)
{
    public Point Left => new(X - 1, Y);
    public Point Right => new(X + 1, Y);
    public Point Up => new(X, Y - 1);
    public Point Down => new(X, Y + 1);

    public IEnumerable<Point> Neighbors => new[] { Up, Right, Down, Left };
}




//using Common;
//using System.Numerics;
//using Map = System.Collections.Generic.Dictionary<System.Numerics.Complex, char>;

//var StartSymbol = 'S';
//var Up = -Complex.ImaginaryOne;
//var Down = Complex.ImaginaryOne;
//var Left = -Complex.One;
//var Right = Complex.One;
//var AllDirections = new Complex[] { Up, Right, Down, Left };

//var Exits = new Dictionary<char, Complex[]>
//{
//    { '7', [ Left, Down ] },
//    { 'F', [ Right, Down ] },
//    { 'L', [ Up, Right ] },
//    { 'J', [ Up, Left ] },
//    { '|', [ Up, Down ] },
//    { '-', [ Left, Right ] },
//    { 'S', [ Up, Down, Left, Right ] },
//    { '.', [] },
//};

//var map = ParseInput(File.ReadAllLines(Constants.FILE_NAME));

//Console.WriteLine($"Part 1 result: {Part1(map)}");
//Console.WriteLine($"Part 2 result: {Part2(map)}");

//long Part1(Map map)
//{
//    return GetLoopPositions(map).Count / 2;
//}

//long Part2(Map map)
//{
//    var loopPositions = GetLoopPositions(map);
//    return map.Keys.Count(x => IsInsideMap(map, x, loopPositions));
//}

//bool IsInsideMap(Map map, Complex pos, HashSet<Complex> loopPositions)
//{
//    if (loopPositions.Contains(pos))
//    {
//        return false;
//    }

//    var isInside = false;
//    pos += Left;
//    while (map.ContainsKey(pos))
//    {
//        if (Exits[map[pos]].Contains(Up) && loopPositions.Contains(pos))
//        {
//            isInside = !isInside;
//        }
//        pos += Left;
//    }

//    return isInside;
//}

//HashSet<Complex> GetLoopPositions(Map map)
//{
//    var res = new HashSet<Complex>();
//    var currentPos = map.Keys.Single(x => map[x] == StartSymbol);
//    var currentDir = AllDirections.First(x => Exits[map[currentPos + x]].Contains(-x));

//    while (true)
//    {
//        res.Add(currentPos);
//        currentPos += currentDir;
//        if (map[currentPos] == StartSymbol)
//        {
//            break;
//        }
//        currentDir = Exits[map[currentPos]].Single(x => x != -currentDir);
//    }

//    return res;
//}

//Map ParseInput(string[] lines)
//{
//    var map = new Map();

//    for (int rowIdx = 0; rowIdx < lines.Length; rowIdx++)
//    {
//        for (int colIdx = 0; colIdx < lines[rowIdx].Length; colIdx++)
//        {
//            map[new Complex(colIdx, rowIdx)] = lines[rowIdx][colIdx];
//        }
//    }

//    return map;
//}