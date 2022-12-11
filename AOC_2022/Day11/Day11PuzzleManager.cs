namespace AOC_2022.Day11
{
    public class Day11PuzzleManager : PuzzleManager
    {
        //new protected const string INPUT_FILE_NAME = "test.txt";

        private Day11InputHelper _inputHelper { get; set; }

        public Day11PuzzleManager()
        {
            _inputHelper = new Day11InputHelper(INPUT_FILE_NAME);
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
            Solve(isPartOne: true);
            return Task.CompletedTask;
        }

        public override Task SolvePartTwo()
        {
            Solve(isPartOne: false);
            return Task.CompletedTask;
        }

        private void Solve(bool isPartOne)
        {
            var monkeys = _inputHelper.Parse();

            var numberOfRounds = isPartOne ? 20 : 10_000;

            for (var i = 0; i < numberOfRounds; i++)
            {
                foreach (var monkey in monkeys)
                {
                    while (monkey.Items.Any())
                    {
                        (var item, var monkeyToThrowTo) = monkey.InspectAndThrow(isPartOne);
                        monkeys[monkeyToThrowTo].AddItem(item);
                    }
                }
            }

            var sortedMonkeys = monkeys.OrderByDescending(x => x.InspectionCount).ToList();
            var puzzlePart = isPartOne ? "one" : "two";
            Console.WriteLine($"The solution to part {puzzlePart} is '{sortedMonkeys[0].InspectionCount * sortedMonkeys[1].InspectionCount}'.");
        }
    }
}
