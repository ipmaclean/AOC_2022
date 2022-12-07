namespace AOC_2022.Day7
{
    public class Day7PuzzleManager : PuzzleManager
    {
        public List<Folder> Folders { get; set; }

        public Day7PuzzleManager()
        {
            var inputHelper = new Day7InputHelper(INPUT_FILE_NAME);
            Folders = inputHelper.Parse();
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
            Console.WriteLine($"The solution to part one is '{Folders.Where(x => x.Size <= 100_000).Sum(x => x.Size)}'.");
            return Task.CompletedTask;
        }

        public override Task SolvePartTwo()
        {
            var spaceToFree = Folders.Max(x => x.Size) - 40_000_000;

            Console.WriteLine($"The solution to part two is '{Folders.OrderBy(x => x.Size).First(x => x.Size >= spaceToFree).Size}'.");
            return Task.CompletedTask;
        }
    }
}
