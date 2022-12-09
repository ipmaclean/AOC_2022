namespace AOC_2022.Day9
{
    public class Day9PuzzleManager : PuzzleManager
    {
        protected const string INPUT_FILE_NAME = "test2.txt";

        public List<(char Direction, int Distance)> Instructions { get; set; }
        public Day9PuzzleManager()
        {
            var inputHelper = new Day9InputHelper(INPUT_FILE_NAME);
            Instructions = inputHelper.Parse();
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
            var headPosition = (0, 0);
            var tailPosition = (0, 0);
            var visitiedByTail = new HashSet<(int, int)>() { tailPosition };

            foreach (var instruction in Instructions)
            {
                headPosition = MoveHead(instruction, headPosition);
                tailPosition = CatchUpTail(headPosition, tailPosition, visitiedByTail);
            }

            Console.WriteLine($"The solution to part one is '{visitiedByTail.Count}'.");
            return Task.CompletedTask;
        }
        public override Task SolvePartTwo()
        {
            var positions = new (int, int)[10];
            for (var i = 0; i < 10; i++)
            {
                positions[i] = (0, 0);
            }
            var visitiedByTail = new HashSet<(int, int)>() { positions[positions.Length - 1] };

            foreach (var instruction in Instructions)
            {
                positions[0] = MoveHead(instruction, positions[0]);
                for (var i = 0; i < positions.Length - 1; i++)
                {
                    var isEndOfRope = i == positions.Length - 2;
                    positions[i + 1] = CatchUpTail(positions[i], positions[i + 1], visitiedByTail, isEndOfRope);
                }
            }

            Console.WriteLine($"The solution to part two is '{visitiedByTail.Count}'.");
            return Task.CompletedTask;
        }

        private (int, int) MoveHead((char Direction, int Distance) instruction, (int X, int Y) headPosition)
            => instruction.Direction switch
            {
                'U' => (headPosition.X, headPosition.Y + instruction.Distance),
                'R' => (headPosition.X + instruction.Distance, headPosition.Y),
                'D' => (headPosition.X, headPosition.Y - instruction.Distance),
                'L' => (headPosition.X - instruction.Distance, headPosition.Y),
                _ => throw new ArgumentOutOfRangeException(nameof(instruction.Direction), $"Unexpected direction value: {instruction.Direction}"),
            };

        private (int, int) CatchUpTail((int X, int Y) headPosition, (int X, int Y) tailPosition, HashSet<(int, int)> visitiedByTail, bool isEndOfRope = true)
        {
            while (!TailHasCaughtUp(headPosition, tailPosition))
            {
                var xUnitVector = UnitVectorOrZero(headPosition.X - tailPosition.X);
                var yUnitVector = UnitVectorOrZero(headPosition.Y - tailPosition.Y);
                tailPosition.X += xUnitVector;
                tailPosition.Y += yUnitVector;
                if (isEndOfRope)
                {
                    TryAddToHashSet(visitiedByTail, tailPosition);
                }
            }
            return tailPosition;
        }

        private bool TailHasCaughtUp((int X, int Y) headPosition, (int X, int Y) tailPosition)
        {
            var xDistance = Math.Abs(headPosition.X - tailPosition.X);
            var yDistance = Math.Abs(headPosition.Y - tailPosition.Y);
            return xDistance <= 1 && yDistance <= 1;
        }

        private int UnitVectorOrZero(int i)
            => i == 0 ? 0 : i / Math.Abs(i);

        private void TryAddToHashSet(HashSet<(int, int)> hashSet, (int, int) position)
        {
            if (!hashSet.Contains(position))
            {
                hashSet.Add(position);
            }
        }
    }
}
