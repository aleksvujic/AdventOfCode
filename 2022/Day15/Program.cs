using Common;

var seasonBeaconPairs = File.ReadAllLines(Constants.FILE_NAME)
    .Select(x =>
    {
        var parts = x
            .Replace("Sensor at ", "")
            .Replace(": closest beacon is at ", ", ")
            .Replace("x=", "")
            .Replace("y=", "")
            .Split(", ")
            .Select(int.Parse)
            .ToArray();

        return new SensorBeaconPair(
            new Position(parts[0], parts[1]),
            new Position(parts[2], parts[3])
        );
    })
    .ToList();

Console.WriteLine($"Part 1 result: {Part1(seasonBeaconPairs)}");
Console.WriteLine($"Part 2 result: {Part2(seasonBeaconPairs)}");

long Part1(List<SensorBeaconPair> seasonBeaconPairs)
{
    var rectangles = seasonBeaconPairs.Select(x => x.GetRectangle());
    var left = rectangles.Min(x => x.Left);
    var right = rectangles.Max(x => x.Right);

    var res = 0;
    var y = 2000000; // puzzle input
    for (var xCoord = left; xCoord <= right; xCoord++)
    {
        var currentPos = new Position(xCoord, y);
        if (seasonBeaconPairs.Any(x => x.Beacon != currentPos && x.IsInRange(currentPos)))
        {
            res++;
        }
    }

    return res;
}

long Part2(List<SensorBeaconPair> seasonBeaconPairs)
{
    var area = GetUncoveredAreas(seasonBeaconPairs, new Rectangle(0, 0, 4000001, 4000001)).First();
    return area.X * 4000000L + area.Y;
}

IEnumerable<Rectangle> GetUncoveredAreas(List<SensorBeaconPair> seasonBeaconPairs, Rectangle rectangle)
{
    if (rectangle.IsEmpty)
    {
        yield break;
    }

    foreach (var seasonBeaconPair in seasonBeaconPairs)
    {
        if (rectangle.Corners.All(x => seasonBeaconPair.IsInRange(x)))
        {
            yield break;
        }
    }

    if (rectangle.IsUnitSquare)
    {
        yield return rectangle;
        yield break;
    }

    foreach (var smallerRect in rectangle.SplitTo4Parts())
    {
        foreach (var area in GetUncoveredAreas(seasonBeaconPairs, smallerRect))
        {
            yield return area;
        }
    }
}

record struct Position(int X, int Y);

record struct SensorBeaconPair(Position Sensor, Position Beacon)
{
    public int Radius => ManhattanDistance(Sensor, Beacon);
    public bool IsInRange(Position pos) => ManhattanDistance(pos, Sensor) <= Radius;

    static int ManhattanDistance(Position pos1, Position pos2)
    {
        return Math.Abs(pos1.X - pos2.X) + Math.Abs(pos1.Y - pos2.Y);
    }

    public Rectangle GetRectangle()
    {
        return new Rectangle(Sensor.X - Radius, Sensor.Y - Radius, 2 * Radius + 1, 2 * Radius + 1);
    }

    public override string ToString()
    {
        return $"Sensor=({Sensor.X},{Sensor.Y}),Beacon=({Beacon.X},{Beacon.Y})";
    }
}

record struct Rectangle(int X, int Y, int Width, int Height)
{
    public int Top => Y;
    public int Bottom => Y + Height - 1;
    public int Left => X;
    public int Right => X + Width - 1;

    public bool IsEmpty = Width == 0 || Height == 0;
    public bool IsUnitSquare = Width == 1 && Height == 1;

    public Position[] Corners => new Position[4]
    {
        new Position(Left, Top),
        new Position(Right, Top),
        new Position(Right, Bottom),
        new Position(Left, Bottom)
    };

    public IEnumerable<Rectangle> SplitTo4Parts()
    {
        var width1 = Width / 2;
        var width2 = Width - width1;
        var height1 = Height / 2;
        var height2 = Height - height1;
        yield return new Rectangle(Left, Top, width1, height1);
        yield return new Rectangle(Left + width1, Top, width2, height1);
        yield return new Rectangle(Left, Top + height1, width1, height2);
        yield return new Rectangle(Left + width1, Top + height1, width2, height2);
    }
}