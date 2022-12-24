using System.Diagnostics;

namespace AOC_2022.Day23
{
    public class Day23PuzzleManager : PuzzleManager
    {
        //new protected const string INPUT_FILE_NAME = "test2.txt";

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
            for (var i = 0; i < 10; i++)
            {
                directionCheckIndex = SimulateRound(elves, directionCheckIndex, adjacentPositions).DirectionCheckIndex;
            }
            var minElfX = elves.Min(elf => elf.Coords.X);
            var minElfY = elves.Min(elf => elf.Coords.Y);
            var maxElfX = elves.Max(elf => elf.Coords.X);
            var maxElfY = elves.Max(elf => elf.Coords.Y);

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
                (directionCheckIndex, anyElfMoved) = SimulateRound(elves, directionCheckIndex, adjacentPositions);
                solution++;
            }

            Console.WriteLine($"The solution to part two is '{solution}'.");
            return Task.CompletedTask;
        }

        private (int DirectionCheckIndex, bool AnyElfMoved) SimulateRound(HashSet<Elf> elves, int directionCheckIndex, List<(int x, int y)> adjacentPositions)
        {
            var anyElfMoved = false;
            // Half one
            foreach (var elf in elves)
            {
                var hasAdjacentElf = false;
                foreach (var adjacentPosition in adjacentPositions)
                {
                    if (elves.Any(e => e.Coords == (elf.Coords.X + adjacentPosition.x, elf.Coords.Y + adjacentPosition.y)))
                    {
                        hasAdjacentElf = true;
                        break;
                    }
                }
                if (!hasAdjacentElf)
                {
                    continue;
                }
                FindProposal(elf, elves, directionCheckIndex);
            }

            // Half two
            foreach (var elf in elves)
            {
                if (elf.Proposal is null)
                {
                    continue;
                }
                var elvesWithSameProposal = elves.Where(x => x.Proposal == elf.Proposal).ToList();
                if (elvesWithSameProposal.Count > 1)
                {
                    foreach (var elfWithSameProposal in elvesWithSameProposal)
                    {
                        elfWithSameProposal.Proposal = null;
                    }
                    continue;
                }
                anyElfMoved = true;
                elf.Coords = elf.Proposal.Value;
                elf.Proposal = null;
            }

            directionCheckIndex = (directionCheckIndex + 1) % 4;
            return (directionCheckIndex, anyElfMoved);
        }

        private void FindProposal(
            Elf elf,
            HashSet<Elf> elves,
            int directionCheckIndex)
        {
            for (var i = 0; i < 4; i++)
            {
                var vectorsToCheck = GetVectorsToCheck((directionCheckIndex + i) % 4);
                var elfFree = true;
                foreach (var vector in vectorsToCheck)
                {
                    if (elves.Any(e => e.Coords == (elf.Coords.X + vector.x, elf.Coords.Y + vector.y)))
                    {
                        elfFree = false;
                        break;
                    }
                }
                if (elfFree)
                {
                    elf.Proposal = (elf.Coords.X + vectorsToCheck[1].x, elf.Coords.Y + vectorsToCheck[1].y);
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
