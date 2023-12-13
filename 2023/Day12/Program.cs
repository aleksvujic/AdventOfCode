using Common;

namespace Day04
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine($"Part 1 result: {Solve(part2: false)}");
            Console.WriteLine($"Part 1 result: {Solve(part2: true)}");
        }

        static long Solve(bool part2)
        {
            var springs = ParseInput(part2: part2);

            long res = 0;

            foreach (var spring in springs)
            {
                res += spring.GetNumValidConditions();
            }

            return res;
        }

        static SpringsStatePair[] ParseInput(bool part2)
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
    }

    record SpringsStatePair(string Springs, string DesiredState)
    {
        private static readonly Dictionary<(string, string), long> Cache = [];

        static void AddToCache(string springs, string groups, long res)
        {
            Cache[(springs, groups)] = res;
        }

        public long GetNumValidConditions()
        {
            return GetNumValidConditions(Springs, DesiredState);
        }

        private long GetNumValidConditions(string springs, string groups)
        {
            if (Cache.ContainsKey((springs, groups)))
            {
                return Cache[(springs, groups)];
            }

            if (GetNumGroups(groups) == 0)
            {
                var res1 = springs.Contains('#') ? 0 : 1;
                AddToCache(springs, groups, res1);
                return res1;
            }
            else if (string.IsNullOrEmpty(springs) || GetFirstGroup(groups) > springs.Length)
            {
                var res2 = 0;
                AddToCache(springs, groups, res2);
                return res2;
            }

            var firstChar = springs.First();
            switch (firstChar)
            {
                case '.':
                    var res3 = GetNumValidConditions(springs[1..], groups);
                    AddToCache(springs, groups, res3);
                    return res3;
                case '?':
                    var res4 =
                        GetNumValidConditions(ReplaceAt(springs, 0, '.'), groups) +
                        GetNumValidConditions(ReplaceAt(springs, 0, '#'), groups);
                    AddToCache(springs, groups, res4);
                    return res4;

                case '#':
                    var groupLen = GetFirstGroup(groups);
                    var group = springs[..groupLen];
                    if (group.All(x => x != '.'))
                    {
                        if (springs.Length == groupLen)
                        {
                            var res5 = GetNumGroups(groups) == 1 ? 1 : 0;
                            AddToCache(springs, groups, res5);
                            return res5;
                        }

                        if (springs[groupLen] != '#')
                        {
                            var res6 = GetNumValidConditions($".{springs[(groupLen + 1)..]}", RemoveFirstGroup(groups));
                            AddToCache(springs, groups, res6);
                            return res6;
                        }
                    }

                    var res7 = 0;
                    AddToCache(springs, groups, res7);
                    return res7;
            }

            var res8 = 0;
            AddToCache(springs, groups, res8);
            return res8;
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
}