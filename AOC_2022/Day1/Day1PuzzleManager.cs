namespace AOC_2022.Day1
{
    public class Day1PuzzleManager : PuzzleManager
    {
        public List<List<int>> ElfInventories { get; set; }
        public Day1PuzzleManager()
        {
            var inputHelper = new Day1InputHelper(INPUT_FILE_NAME);
            ElfInventories = inputHelper.Parse();
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
            Console.WriteLine($"The solution to part one is '{ElfInventories.Max(x => x.Sum())}'.");
            return Task.CompletedTask;
        }

        public override Task SolvePartTwo()
        {
            // It would be more performant to implement something like part one
            // three times, but as it's not a large list sorting it all is fine.
            ElfInventories = ElfInventories.OrderByDescending(x => x.Sum()).ToList();
            var solution = 0;
            for (var i = 0; i < 3; i++)
            {
                solution += ElfInventories[i].Sum();
            }
            Console.WriteLine($"The solution to part two is '{solution}'.");
            return Task.CompletedTask;
        }
    }
}
