using System.Diagnostics;

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
            var sw = new Stopwatch();
            sw.Start();
            SolvePartOne();
            sw.Stop();
            Console.WriteLine($"Part one: {sw.ElapsedMilliseconds}ms.");
            Console.WriteLine();
            sw.Restart();
            SolvePartTwo();
            sw.Stop();
            Console.WriteLine($"Part two: {sw.ElapsedMilliseconds}ms.");
            return Task.CompletedTask;
        }

        public override Task SolvePartOne()
        {
            var cycleLength = Lcm(Input[0].Length - 2, Input.Count - 2);
            var valley = ParseValley(cycleLength);
            var startCoord = (1, 0);
            var endCoord = (valley.GetLength(0) - 2, valley.GetLength(1) - 1);
            var solutionState = FindShortestTime(startCoord, endCoord, 0, cycleLength, valley);
            Console.WriteLine($"The solution to part one is '{solutionState.Time}'.");
            return Task.CompletedTask;
        }

        public override Task SolvePartTwo()
        {
            var cycleLength = Lcm(Input[0].Length - 2, Input.Count - 2);
            var valley = ParseValley(cycleLength);
            var startCoord = (1, 0);
            var endCoord = (valley.GetLength(0) - 2, valley.GetLength(1) - 1);
            var solutionState1 = FindShortestTime(startCoord, endCoord, 0, cycleLength, valley);
            var solutionState2 = FindShortestTime(endCoord, startCoord, solutionState1.Time, cycleLength, valley);
            var solutionState3 = FindShortestTime(startCoord, endCoord, solutionState2.Time, cycleLength, valley);

            Console.WriteLine($"The solution to part two is '{solutionState3.Time}'.");
            return Task.CompletedTask;
        }

        private static (int X, int Y, int Time) FindShortestTime(
            (int X, int Y) startCoord, 
            (int X, int Y) endCoord, 
            int startTime,
            int cycleLength,
            HashSet<int>[,] valley
            )
        {
            var statesToVisit = new Queue<(int X, int Y, int Time)>();
            var visitedStates = new HashSet<string>() { GetStateHash(startCoord, startTime) };
            statesToVisit.Enqueue((startCoord.X, startCoord.Y, startTime));
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

                    if (nextCoord == endCoord)
                    {
                        return (nextCoord.X, nextCoord.Y, currentState.Time + 1);
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
