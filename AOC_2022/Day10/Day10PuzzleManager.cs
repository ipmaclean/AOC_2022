namespace AOC_2022.Day10
{
    public class Day10PuzzleManager : PuzzleManager
    {
        public Day10PuzzleManager()
        {
            var inputHelper = new Day10InputHelper(INPUT_FILE_NAME);
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
            Console.WriteLine($"The solution to part one is '{solution}'.");
            return Task.CompletedTask;
        }

        public override Task SolvePartTwo()
        {
            var solution = 0;
            Console.WriteLine($"The solution to part two is '{solution}'.");
            return Task.CompletedTask;
        }
    }
}
