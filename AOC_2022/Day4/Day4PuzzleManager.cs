namespace AOC_2022.Day4
{
    public class Day4PuzzleManager : PuzzleManager
    {
        public List<ElfTeam> ElfPairs { get; set; }

        public Day4PuzzleManager()
        {
            var inputHelper = new Day4InputHelper(INPUT_FILE_NAME);
            ElfPairs = inputHelper.Parse();
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
            var solution = 0;
            foreach (var elfPair in ElfPairs)
            {
                if (elfPair.IsFullyContained())
                {
                    solution++;
                }
            }
            Console.WriteLine($"The solution to part one is '{solution}'.");
            return Task.CompletedTask;
        }

        public override Task SolvePartTwo()
        {
            var solution = 0;
            foreach (var elfPair in ElfPairs)
            {
                if (elfPair.HasOverlap())
                {
                    solution++;
                }
            }
            Console.WriteLine($"The solution to part two is '{solution}'.");
            return Task.CompletedTask;
        }
    }
}
