using Common;
using System.Text.Json;

var packetPairs = File.ReadAllLines(Constants.FILE_NAME)
    .Chunk(3)
    .Select(x => new PacketPair(Packet.Parse(x[0]), Packet.Parse(x[1])))
    .ToList();

Console.WriteLine($"Part 1 result: {Part1(packetPairs)}");
Console.WriteLine($"Part 2 result: {Part2(packetPairs)}");

int Part1(List<PacketPair> packetPairs)
{
    int sum = 0;

    for (int i = 0; i < packetPairs.Count; i++)
    {
        (Packet left, Packet right) = packetPairs[i];
        if (left.CompareTo(right) < 0)
        {
            sum += (i + 1);
        }
    }

    return sum;
}

int Part2(List<PacketPair> packetPairs)
{
    var allPackets = packetPairs
        .Select(x => new List<Packet>() { x.Left, x.Right })
        .SelectMany(x => x)
        .ToList();

    // add 2 divider packets
    var packet2 = Packet.Parse("[[2]]");
    var packet6 = Packet.Parse("[[6]]");
    allPackets.AddRange(new List<Packet>() { packet2, packet6 });

    // this uses CompareTo in the background
    allPackets.Sort();

    return (allPackets.IndexOf(packet2) + 1) * (allPackets.IndexOf(packet6) + 1);
}

public record PacketPair(Packet Left, Packet Right);

public class Packet : IComparable<Packet>
{
    private readonly int WRONG_ORDER = 1;
    private readonly int RIGHT_ORDER = -1;
    private readonly int UNSURE_ORDER = 0;
    
    public static Packet Parse(string input) => Parse(JsonSerializer.Deserialize<JsonElement>(input));

    private static Packet Parse(JsonElement jsonEl)
    {
        return jsonEl.ValueKind switch
        {
            JsonValueKind.Array => new ArrayPacket(jsonEl.EnumerateArray().Select(Parse).ToList()),
            JsonValueKind.Number => new NumberPacket(jsonEl.GetInt32()),
            _ => throw new Exception($"Value kind {jsonEl.ValueKind} is not recognized")
        };
    }

    public int CompareTo(Packet? other)
    {
        int CompareArrayPacket(ArrayPacket l, ArrayPacket r)
        {
            for (int i = 0; i < l.Packets.Count; i++)
            {
                if (r.Packets.Count <= i)
                {
                    return WRONG_ORDER;
                }

                var compareRes = l.Packets[i].CompareTo(r.Packets[i]);
                if (compareRes != UNSURE_ORDER)
                {
                    return compareRes;
                }
            }

            if (l.Packets.Count == r.Packets.Count)
            {
                return UNSURE_ORDER;
            }
            
            return RIGHT_ORDER;
        }
        
        Packet left = this;
        Packet right = other!;

        return (left, right) switch
        {
            (not null, null) => WRONG_ORDER,
            (null, not null) => RIGHT_ORDER,
            (ArrayPacket lap, ArrayPacket rap) => CompareArrayPacket(lap, rap),
            (NumberPacket lnp, NumberPacket rnp) => lnp.Value < rnp.Value ? RIGHT_ORDER : (lnp.Value > rnp.Value ? WRONG_ORDER : UNSURE_ORDER),
            (NumberPacket lnp, ArrayPacket rap) => new ArrayPacket(new List<Packet>() { lnp }).CompareTo(rap),
            (ArrayPacket lap, NumberPacket rnp) => lap.CompareTo(new ArrayPacket(new List<Packet>() { rnp })),
            _ => UNSURE_ORDER
        };
    }
}

public class NumberPacket : Packet
{
    public NumberPacket(int value)
    {
        Value = value;
    }

    public int Value { get; set; }

    public override string ToString() => Value.ToString();
}

public class ArrayPacket : Packet
{
    public ArrayPacket(List<Packet> packets)
    {
        Packets = packets;
    }

    public List<Packet> Packets { get; set; }

    public override string ToString() => $"[{string.Join(",", Packets)}]";
}
