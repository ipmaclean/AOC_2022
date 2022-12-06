namespace AOC_2022.Day6
{
    public class Day6PuzzleManager : PuzzleManager
    {
        public string Input { get; set; }
        public Day6PuzzleManager()
        {
            var inputHelper = new Day6InputHelper(INPUT_FILE_NAME);
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
            Console.WriteLine($"The solution to part one is '{FindDistinct(4)}'.");
            return Task.CompletedTask;
        }

        public override Task SolvePartTwo()
        {
            Console.WriteLine($"The solution to part two is '{FindDistinct(14)}'.");
            return Task.CompletedTask;
        }

        private int FindDistinct(int numberOfDistinct)
        {
            for (var i = 0; i < Input.Length; i++)
            {
                if (Input.Substring(i, numberOfDistinct).Distinct().Count() == numberOfDistinct)
                {
                    return i + numberOfDistinct;
                }
            }
            return 0;
        }
    }
}
