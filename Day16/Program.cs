using Common;

namespace Day16
{
    internal class Program
    {
        static void Main()
        {
            string hexInput = File.ReadAllLines(Constants.FILE_NAME).First();
            string binaryString = String.Join(String.Empty,
                hexInput.Select(
                    c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')
                )
            );

            Console.WriteLine($"Part 1 result: {GetTotalVersion(GetPacket(new Reader(binaryString)))}");
            Console.WriteLine($"Part 2 result: {Evaluate(GetPacket(new Reader(binaryString)))}");
        }

        private static long GetTotalVersion(Packet packet)
        {
            return packet.Version + packet.NestedPackets.Select(GetTotalVersion).Sum();
        }

        private static long Evaluate(Packet packet)
        {
            var parts = packet.NestedPackets
                .Select(Evaluate)
                .ToList();

            return packet.TypeId switch
            {
                0 => parts.Sum(),
                1 => parts.Aggregate(1L, (acc, x) => acc * x),
                2 => parts.Min(),
                3 => parts.Max(),
                4 => packet.Data,
                5 => parts[0] > parts[1] ? 1 : 0,
                6 => parts[0] < parts[1] ? 1 : 0,
                7 => parts[0] == parts[1] ? 1 : 0,
                _ => throw new NotImplementedException($"Type ID {packet.TypeId} is not recognized!")
            };
        }

        private static Packet GetPacket(Reader reader)
        {
            int version = reader.ReadInt(3);
            int typeId = reader.ReadInt(3);
            var packets = new List<Packet>();
            long data = 0;

            if (typeId == 4)
            {
                // literal value, 5 bit chunks
                while (true)
                {
                    bool last = reader.ReadInt(1) == 0;
                    data = data * 16 + reader.ReadInt(4);
                    if (last)
                    {
                        break;
                    }
                }
            } else if (reader.ReadInt(1) == 0)
            {
                // operator
                int length = reader.ReadInt(15);
                var newReaderSubpackages = reader.GetReader(length);
                while (newReaderSubpackages.HasChars())
                {
                    packets.Add(GetPacket(newReaderSubpackages));
                }
            }
            else
            {
                // operator containing 'packetCount' number of subpackages
                int subPackagesCount = reader.ReadInt(11);
                for (int i = 0; i < subPackagesCount; i++)
                {
                    packets.Add(GetPacket(reader));
                }
            }

            return new Packet(version, typeId, data, packets.ToArray());
        }
    }

    class Reader
    {
        private int _currentIndex = 0;
        private readonly string _binaryString;
        
        public Reader(string binaryString)
        {
            _binaryString = binaryString;
        }

        public int ReadInt(int len)
        {
            int res = Convert.ToInt32(_binaryString[_currentIndex..(_currentIndex + len)], 2);
            _currentIndex += len;
            return res;
        }

        public Reader GetReader(int len)
        {
            var reader = new Reader(_binaryString[_currentIndex..(_currentIndex + len)]);
            _currentIndex += len;
            return reader;
        }

        public bool HasChars()
        {
            return _currentIndex < _binaryString.Length;
        }
    }

    record Packet(int Version, int TypeId, long Data, Packet[] NestedPackets);
}
