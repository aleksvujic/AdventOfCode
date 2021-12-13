using Common;

namespace Day13
{
    internal class Program
    {
        static void Main()
        {
            string[] lines = File.ReadAllLines(Constants.FILE_NAME);

            // get positions of dots
            Pos[] dotPositions = lines
                .Where(x => x.Contains(','))
                .Select(x => x.Split(','))
                .Select(x => new Pos(int.Parse(x[0]), int.Parse(x[1])))
                .ToArray();

            // get all fold instructions
            Fold[] folds = lines
                .Where(x => x.Contains('='))
                .Select(x => x.Replace("fold along ", string.Empty))
                .Select(x => x.Split('='))
                .Select(x =>
                {
                    _ = Enum.TryParse(x[0].ToUpper(), out FoldDirection foldDirection);
                    return new Fold(foldDirection, int.Parse(x[1]));
                })
                .ToArray();

            Console.WriteLine($"Part 1 result: {Solve(dotPositions, folds, part2: false, CountDots)}");
            Console.WriteLine($"Part 2 result: {Solve(dotPositions, folds, part2: true, Get8CapitalLetters)}");
        }

        static T Solve<T>(Pos[] dotPositions, Fold[] folds, bool part2, Func<char[][], T> getResult)
        {
            var map = GetMap(dotPositions);

            for (int i = 0; i < folds.Length; i++)
            {
                // perform fold
                map = Fold(map, folds[i]);

                if (!part2)
                {
                    break;
                }
            }

            return getResult(map);
        }

        static char[][] Fold(char[][] map, Fold fold)
        {
            char[][] newMap = fold.FoldDirection switch
            {
                FoldDirection.X => GetEmptyMap(map.Length, map[0].Length / 2),
                FoldDirection.Y => GetEmptyMap(map.Length / 2, map[0].Length),
                _ => throw new Exception("Fold direction not recognized!"),
            };

            for (int i = 0; i < newMap.Length; i++)
            {
                for (int j = 0; j < newMap[i].Length; j++)
                {
                    char thisSideOfTheFold = map[i][j];
                    char charOnTheOtherSideOfFold = fold.FoldDirection switch
                    {
                        FoldDirection.X => map[i][map[i].Length - 1 - j],
                        FoldDirection.Y => map[map.Length - 1 - i][j],
                        _ => throw new Exception("Fold direction not recognized!"),
                    };

                    newMap[i][j] = thisSideOfTheFold == '.' ? charOnTheOtherSideOfFold : thisSideOfTheFold;
                }
            }
            
            return newMap;
        }

        static int CountDots(char[][] map)
        {
            return map
                .SelectMany(x => x)
                .Count(x => x == '#');
        }

        static string Get8CapitalLetters(char[][] map)
        {
            int charWidth = 5;
            int charHeight = 6;

            int width = map[0].Length;
            int height = map.Length;

            var dict = new Dictionary<long, string>
            {
                // 5 x 6
                {0x19297A52, "A"},
                {0x3252F4A4, "A"},
                {0x725C94B8, "B"},
                {0x32508498, "C"},
                {0x7A1C843C, "E"},
                {0x7A1C8420, "F"},
                {0x3D0E4210, "F"},
                {0x3250B49C, "G"},
                {0x252F4A52, "H"},
                {0x4A5E94A4, "H"},
                {0x0C210A4C, "J"},
                {0x18421498, "J"},
                {0x4A98A524, "K"},
                {0x2108421E, "L"},
                {0x4210843C, "L"},
                {0x7252E420, "P"},
                {0x7252E524, "R"},
                {0x4A529498, "U"},
                {0x462A2108, "Y"},
                {0x3C22221E, "Z"},
                {0x7844443C, "Z"},

                // 8x10
                {0x909F109090909F0, "B"},
                {0x1010101010108F0, "C"},
                {0x1010139090918E8, "G"},
                {0x10101010111110E0, "J"},
                {0x8102040810101F8, "Z"},

                {0, string.Empty}
            };

            string res = string.Empty;

            for (int ch = 0; ch < Math.Ceiling(width / (double)charWidth); ch++)
            {
                var hash = 0L;
                var stChar = string.Empty;
                for (int irow = 0; irow < charHeight; irow++)
                {
                    for (int i = 0; i < charWidth; i++)
                    {
                        int icol = (ch * charWidth) + i;
                        char point = irow < height && icol < map[irow].Length ? map[irow][icol] : ' ';
                        stChar += point;
                        if (point != '.')
                        {
                            hash += 1;
                        }
                        hash <<= 1;
                    }
                    stChar += "\n";
                }

                if (!dict.ContainsKey(hash))
                {
                    throw new Exception($"Unrecognized letter with hash: 0x{hash:X}\n{stChar}");
                }

                res += dict[hash];
            }

            return res;
        }

        static char[][] GetMap(Pos[] dotPositions)
        {
            if (dotPositions == null)
            {
                throw new Exception("Positions of dots must not be null!");
            }
            
            // get dimensions of grid
            int maxX = dotPositions?.MaxBy(x => x.X)?.X + 1 ?? 0;
            int maxY = dotPositions?.MaxBy(x => x.Y)?.Y + 1?? 0;

            char[][] map = GetEmptyMap(maxY, maxX);

            foreach (Pos pos in dotPositions!)
            {
                map[pos.Y][pos.X] = '#';
            }

            return map;
        }

        static char[][] GetEmptyMap(int lenY, int lenX)
        {
            char[][] map = new char[lenY][];
            for (int i = 0; i < map.Length; i++)
            {
                map[i] = Enumerable.Repeat('.', lenX).ToArray();
            }
            return map;
        }
    }

    enum FoldDirection
    {
        X,
        Y
    }

    record Pos(int X, int Y);
    record Fold(FoldDirection FoldDirection, int FoldAt);
}
