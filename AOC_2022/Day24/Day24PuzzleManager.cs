﻿using AOC_2022.Day22;

namespace AOC_2022.Day24
{
    public class Day24PuzzleManager : PuzzleManager
    {
        //new protected const string INPUT_FILE_NAME = "test.txt";

        public List<string> Input { get; set; }

        public Day24PuzzleManager()
        {
            var inputHelper = new Day24InputHelper(INPUT_FILE_NAME);
            Input = inputHelper.Parse();
        }
        public override Task SolveBothParts()
        {
            SolvePartOne();
            Console.WriteLine();
            SolvePartTwo();
            return Task.CompletedTask;
        }

        public override Task SolvePartOne()
        {
            var cycleLength = Lcm(Input[0].Length - 2, Input.Count - 2);
            var valley = ParseValley(cycleLength);
            Console.WriteLine($"The solution to part one is '{FindShortestTime(cycleLength, valley)}'.");
            return Task.CompletedTask;
        }

        private static int FindShortestTime(int cycleLength, HashSet<int>[,] valley)
        {
            var statesToVisit = new Queue<(int X, int Y, int Time)>();
            var visitedStates = new HashSet<string>() { GetStateHash((1, 0), 0) };
            statesToVisit.Enqueue((1, 0, 0));
            var directions = new (int X, int Y)[]
            {
                (1, 0),
                (0, 1),
                (-1, 0),
                (0, -1),
                (0, 0)
            };

            while (statesToVisit.TryDequeue(out var currentState))
            {
                foreach (var direction in directions)
                {
                    (int X, int Y) nextCoord = (currentState.X + direction.X, currentState.Y + direction.Y);

                    if (nextCoord.X < 0 || nextCoord.X > valley.GetLength(0) - 1 ||
                        nextCoord.Y < 0 || nextCoord.Y > valley.GetLength(1) - 1 ||
                        valley[nextCoord.X, nextCoord.Y].Contains((currentState.Time + 1) % cycleLength) ||
                        visitedStates.Contains(GetStateHash(nextCoord, (currentState.Time + 1) % cycleLength)))
                    {
                        continue;
                    }

                    if (nextCoord.X == valley.GetLength(0) - 2 && nextCoord.Y == valley.GetLength(1) - 1)
                    {
                        return currentState.Time + 1;
                    }

                    visitedStates.Add(GetStateHash(nextCoord, (currentState.Time + 1) % cycleLength));
                    statesToVisit.Enqueue((nextCoord.X, nextCoord.Y, currentState.Time + 1));
                }
            }
            throw new InvalidOperationException("Could not find a shortest path.");
        }

        private static string GetStateHash((int X, int Y) coords, int time)
            => $"({coords.X},{coords.Y}),{time}";

        private HashSet<int>[,] ParseValley(int cycleLength)
        {
            var valley = new HashSet<int>[Input[0].Length, Input.Count];
            for (var y = 0; y < Input.Count; y++)
            {
                for (var x = 0; x < Input[0].Length; x++)
                {
                    valley[x, y] = new HashSet<int>();
                }
            }

            for (var y = 0; y < Input.Count; y++)
            {
                for (var x = 0; x < Input[0].Length; x++)
                {
                    // Walls are always impassible
                    if ((y == 0 && x != 1) ||
                        (y == valley.GetLength(1) - 1 && x != valley.GetLength(0) - 2) ||
                        x == 0 ||
                        x == valley.GetLength(0) - 1)
                    {
                        for (var i = 0; i < cycleLength; i++)
                        {
                            valley[x, y].Add(i);
                        }
                        continue;
                    }

                    var blizzardUnitVector = GetBlizzardUnitVector(Input[y][x]);
                    if (blizzardUnitVector == (0, 0))
                    {
                        continue;
                    }
                    SetBlizzardTimes(valley, (x, y), blizzardUnitVector, cycleLength);
                }
            }
            return valley;
        }

        private void SetBlizzardTimes(
            HashSet<int>[,] valley,
            (int X, int Y) currentCoords,
            (int X, int Y) blizzardUnitVector,
            int cycleLength
            )
        {
            for (var i = 0; i < cycleLength; i++)
            {
                // Need to account for the walls
                var position = valley[
                    Mod(currentCoords.X - 1 + blizzardUnitVector.X * i, valley.GetLength(0) - 2) + 1,
                    Mod(currentCoords.Y - 1 + blizzardUnitVector.Y * i, valley.GetLength(1) - 2) + 1
                    ];
                if (!position.Contains(i))
                {
                    position.Add(i);
                }
            }
        }

        public override Task SolvePartTwo()
        {
            var solution = 0;
            Console.WriteLine($"The solution to part two is '{solution}'.");
            return Task.CompletedTask;
        }

        private static (int X, int Y) GetBlizzardUnitVector(char tile) => tile switch
        {
            '^' => (0, -1),
            '<' => (-1, 0),
            '>' => (1, 0),
            'v' => (0, 1),
            _ => (0, 0)
        };

        private static int Gfc(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        private static int Lcm(int a, int b)
        {
            return (a / Gfc(a, b)) * b;
        }

        private static int Mod(int x, int m)
        {
            return (x % m + m) % m;
        }
    }
}
