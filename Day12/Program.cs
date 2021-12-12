using Common;
using System.Collections.Immutable;

namespace Day12
{
    internal class Program
    {
        static void Main()
        {
            Connection[] connections = File.ReadAllLines(Constants.FILE_NAME)
                .Select(x =>
                {
                    var res = x.Split('-');
                    return new Connection(res[0], res[1]);
                })
                .ToArray();

            Console.WriteLine($"Part 1 result: {Solve(connections, part2: false)}");
            Console.WriteLine($"Part 2 result: {Solve(connections, part2: true)}");
        }

        static int Solve(Connection[] connections, bool part2)
        {
            var graph = new UndirectedGraph();
            foreach (Connection conn in connections)
            {
                graph.AddEdge(new Node(conn.From), new Node(conn.To));
            }

            return graph.Explore(part2: part2);
        }
    }

    public class Node
    {
        public string Name { get; set; }
        public bool IsStart => Name == "start";
        public bool IsEnd => Name == "end";
        public bool IsBigCave => char.IsUpper(Name[0]);

        public Node(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Node)
            {
                return false;
            }

            Node node = (Node)obj;
            return Name == node.Name;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var result = 0;
                result = (result * 397) ^ Name.GetHashCode();
                result = (result * 397) ^ (IsStart ? 1 : 0);
                result = (result * 397) ^ (IsEnd ? 1 : 0);
                result = (result * 397) ^ (IsBigCave ? 1 : 0);
                return result;
            }
        }
    }

    public class UndirectedGraph
    {
        // adjacency lists
        private readonly Dictionary<Node, List<Node>> _adj;

        private Node StartNode
        {
            get
            {
                return _adj.Keys.First(x => x.IsStart);
            }
        }

        public UndirectedGraph()
        {
            // initialize dicitonary
            _adj = new Dictionary<Node, List<Node>>();
        }

        public void AddEdge(Node v, Node w)
        {
            // add keys if they don't exist yet
            if (!_adj.ContainsKey(v))
            {
                _adj.Add(v, new List<Node>());
            }

            if (!_adj.ContainsKey(w))
            {
                _adj.Add(w, new List<Node>());
            }

            // add to adjacency lists
            _adj[v].Add(w);
            _adj[w].Add(v);
        }

        public List<Node> GetAdjacency(Node v)
        {
            return _adj[v];
        }

        public int Explore(bool part2)
        {
            return PathCount(StartNode, ImmutableHashSet.Create<Node>(StartNode), false, part2);
        }

        private int PathCount(Node currentCave, ImmutableHashSet<Node> visitedCaves, bool anySmallCaveVisitedTwice, bool part2)
        {
            if (currentCave.IsEnd)
            {
                return 1;
            }

            int count = 0;
            foreach (Node adj in GetAdjacency(currentCave))
            {
                if (adj.IsBigCave || !visitedCaves.Contains(adj))
                {
                    count += PathCount(adj, visitedCaves.Add(adj), anySmallCaveVisitedTwice, part2);
                }
                else if (part2 && !adj.IsBigCave && !adj.IsStart && !anySmallCaveVisitedTwice)
                {
                    count += PathCount(adj, visitedCaves, true, part2);
                }
            }
            return count;
        }
    }

    record Connection(string From, string To);
}
