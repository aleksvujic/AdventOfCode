using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day02
{
    class Program
    {
        static void Main()
        {
            List<Move> moves = File.ReadAllLines(Constants.FILE_NAME)
                .Select(x =>
                {
                    // format of line is (example): "forward 5" 
                    string[] parts = x.Split(' ');

                    // convert direction to enum
                    _ = Enum.TryParse(CapitalizeFirstLetter(parts[0]), out Direction direction);
                    
                    // create new struct, parse number to int
                    return new Move(direction, int.Parse(parts[1]));
                })
                .ToList();

            Console.WriteLine($"Simulation 1 result: {Simulation1(moves)}");
            Console.WriteLine($"Simulation 2 result: {Simulation2(moves)}");
        }

        private static int Simulation1(List<Move> moves)
        {
            int horizontalPos = 0;
            int depth = 0;

            foreach (Move move in moves)
            {
                switch (move.Direction)
                {
                    case Direction.Forward:
                        horizontalPos += move.Units;
                        break;
                    case Direction.Down:
                        depth += move.Units;
                        break;
                    case Direction.Up:
                        depth -= move.Units;
                        break;
                    default:
                        throw new Exception("Direction not recognized");
                }
            }

            return horizontalPos * depth;
        }

        private static int Simulation2(List<Move> moves)
        {
            int horizontalPos = 0;
            int depth = 0;
            int aim = 0;

            foreach (Move move in moves)
            {
                switch (move.Direction)
                {
                    case Direction.Forward:
                        horizontalPos += move.Units;
                        depth += aim * move.Units;
                        break;
                    case Direction.Down:
                        aim += move.Units;
                        break;
                    case Direction.Up:
                        aim -= move.Units;
                        break;
                    default:
                        throw new Exception("Direction not recognized");
                }
            }

            return horizontalPos * depth;
        }

        private static string CapitalizeFirstLetter(string input)
        {
            // convert input so that its first letter is capitalized (ex. house => House)
            return input switch
            {
                null => throw new ArgumentNullException(nameof(input)),
                "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
                _ => string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1))
            };
        }
    }

    public readonly struct Move
    {
        public Direction Direction { get; init; }

        public int Units { get; init; }
        
        public Move(Direction direction, int units)
        {
            Direction = direction;
            Units = units;
        }
    }

    public enum Direction
    {
        Forward,
        Down,
        Up
    }
}
