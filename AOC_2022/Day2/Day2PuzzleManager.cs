namespace AOC_2022.Day2
{
    public class Day2PuzzleManager : PuzzleManager
    {
        public List<RpsGame> Games { get; set; }

        public Day2PuzzleManager()
        {
            var inputHelper = new Day2InputHelper(INPUT_FILE_NAME);
            Games = inputHelper.Parse();
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
            foreach (var game in Games)
            {
                solution += game.CalculateScorePartOne();
            }
            Console.WriteLine($"The solution to part one is '{solution}'.");
            return Task.CompletedTask;
        }

        public override Task SolvePartTwo()
        {
            var solution = 0;
            foreach (var game in Games)
            {
                solution += game.CalculateScorePartTwo();
            }
            Console.WriteLine($"The solution to part two is '{solution}'.");
            return Task.CompletedTask;
        }
    }
}
