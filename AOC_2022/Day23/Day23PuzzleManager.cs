using System.Diagnostics;

namespace AOC_2022.Day23
{
    public class Day23PuzzleManager : PuzzleManager
    {
        private Day23InputHelper _inputHelper;

        public Day23PuzzleManager()
        {
            _inputHelper = new Day23InputHelper(INPUT_FILE_NAME);
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
            var elves = _inputHelper.Parse();
            var directionCheckIndex = 0;

            var adjacentPositions = new List<(int x, int y)>()
            {
                (-1, -1),
                (-1, 0),
                (-1, 1),
                (0, -1),
                (0, 1),
                (1, -1),
                (1, 0),
                (1, 1)
            };
            var anyElfMoved = true;
            for (var i = 0; i < 10; i++)
            {
                (elves, directionCheckIndex, anyElfMoved) = SimulateRound(elves, directionCheckIndex, adjacentPositions);
            }
            var minElfX = elves.Keys.Min(elf => elf.X);
            var minElfY = elves.Keys.Min(elf => elf.Y);
            var maxElfX = elves.Keys.Max(elf => elf.X);
            var maxElfY = elves.Keys.Max(elf => elf.Y);

            var solution = (maxElfX - minElfX + 1) * (maxElfY - minElfY + 1) - elves.Count;
            Console.WriteLine($"The solution to part one is '{solution}'.");
            return Task.CompletedTask;
        }

        public override Task SolvePartTwo()
        {
            var elves = _inputHelper.Parse();
            var directionCheckIndex = 0;

            var adjacentPositions = new List<(int x, int y)>()
            {
                (-1, -1),
                (-1, 0),
                (-1, 1),
                (0, -1),
                (0, 1),
                (1, -1),
                (1, 0),
                (1, 1)
            };
            var anyElfMoved = true;
            var solution = 0;

            while (anyElfMoved)
            {
                (elves, directionCheckIndex, anyElfMoved) = SimulateRound(elves, directionCheckIndex, adjacentPositions);
                solution++;
            }

            Console.WriteLine($"The solution to part two is '{solution}'.");
            return Task.CompletedTask;
        }

        private (Dictionary<(int X, int Y), (int X, int Y)> Elves, int DirectionCheckIndex, bool AnyElfMoved) SimulateRound(
            Dictionary<(int X, int Y), (int X, int Y)> elves,
            int directionCheckIndex,
            List<(int x, int y)> adjacentPositions
            )
        {
            // Half one
            foreach (var elf in elves)
            {
                var hasAdjacentElf = false;
                foreach (var adjacentPosition in adjacentPositions)
                {
                    if (elves.ContainsKey((elf.Key.X + adjacentPosition.x, elf.Key.Y + adjacentPosition.y)))
                    {
                        hasAdjacentElf = true;
                        break;
                    }
                }
                if (!hasAdjacentElf)
                {
                    elves[elf.Key] = elf.Key;
                    continue;
                }
                FindProposal(elf.Key, elves, directionCheckIndex);
            }

            // Half two
            var newElves = new Dictionary<(int X, int Y), (int X, int Y)>();
            foreach (var elf in elves)
            {
                if (!newElves.ContainsKey(elf.Value))
                {
                    newElves.Add(elf.Value, (0, 0));
                }
                else
                {
                    // No more than two elves can propose the same space, and
                    // they must come from opposite directions. Therefore, if
                    // there is alreay an elf in the proposed space, you can
                    // 'push' them back one space to take them back to their
                    // original space.
                    newElves.Remove(elf.Value);
                    newElves.Add((2 * elf.Value.X - elf.Key.X, 2 * elf.Value.Y - elf.Key.Y), (0, 0));
                    newElves.Add(elf.Key, (0, 0));
                }
            }
            var anyElfMoved = elves.Keys.Any(x => !newElves.ContainsKey(x));
            directionCheckIndex = (directionCheckIndex + 1) % 4;
            return (newElves, directionCheckIndex, anyElfMoved);
        }

        private void FindProposal(
            (int X, int Y) elf,
            Dictionary<(int X, int Y), (int X, int Y)> elves,
            int directionCheckIndex)
        {
            for (var i = 0; i < 4; i++)
            {
                var elfFree = true;
                var vectorsToCheck = GetVectorsToCheck((directionCheckIndex + i) % 4);
                foreach (var vector in vectorsToCheck)
                {
                    if (elves.ContainsKey((elf.X + vector.x, elf.Y + vector.y)))
                    {
                        elfFree = false;
                        elves[elf] = elf;
                        break;
                    }
                }
                if (elfFree)
                {
                    elves[elf] = (elf.X + vectorsToCheck[1].x, elf.Y + vectorsToCheck[1].y);
                    break;
                }
            }
        }

        private List<(int x, int y)> GetVectorsToCheck(int directionCheckIndex)
            => directionCheckIndex switch
            {
                0 => new List<(int x, int y)>()
                {
                    (-1, -1), (0, -1), (1, -1)
                },
                1 => new List<(int x, int y)>()
                {
                    (-1, 1), (0, 1), (1, 1)
                },
                2 => new List<(int x, int y)>()
                {
                    (-1, -1), (-1, 0), (-1, 1)
                },
                3 => new List<(int x, int y)>()
                {
                    (1, -1), (1, 0), (1, 1)
                },
                _ => throw new ArgumentOutOfRangeException($"Unexpected directionCheckIndex value: {directionCheckIndex}."),
            };
    }
}
