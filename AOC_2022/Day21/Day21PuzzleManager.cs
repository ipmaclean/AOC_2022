namespace AOC_2022.Day21
{
    public class Day21PuzzleManager : PuzzleManager
    {
        private Day21InputHelper _inputHelper;

        //new protected const string INPUT_FILE_NAME = "test.txt";

        public Day21PuzzleManager()
        {
            var inputHelper = new Day21InputHelper(INPUT_FILE_NAME);
            _inputHelper = inputHelper;
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
            // Try the recursive approach - if that overflows try the two list approach

            var monkeys = _inputHelper.Parse();
            var rootMonkey = monkeys.First(x => x.Name == "root");

            var solution = GetValue(rootMonkey, monkeys);
            Console.WriteLine($"The solution to part one is '{solution}'.");
            return Task.CompletedTask;
        }

        public override Task SolvePartTwo()
        {
            var monkeys = _inputHelper.Parse();
            var human = monkeys.First(x => x.Name == "humn");
            human.Number = null;


            var rootMonkey = monkeys.First(x => x.Name == "root");
            var childMonkeys = (monkeys.First(x => x.Name == rootMonkey.Monkeys!.Value.Item1), monkeys.First(x => x.Name == rootMonkey.Monkeys!.Value.Item2));

            var shouldBeEqualTo = -1L;
            var exceptionMonkey = rootMonkey;
            try
            {
                shouldBeEqualTo = GetValue(childMonkeys.Item1, monkeys);
            }
            catch
            {
                exceptionMonkey = childMonkeys.Item1;
            }
            try
            {
                shouldBeEqualTo = GetValue(childMonkeys.Item2, monkeys);
            }
            catch
            {
                exceptionMonkey = childMonkeys.Item2;
            }

            var solution = 0;
            Console.WriteLine($"The solution to part two is '{solution}'.");
            return Task.CompletedTask;
        }

        private long GetValue(Monkey monkey, List<Monkey> monkeys)
        {
            if (monkey.Number.HasValue)
            {
                return monkey.Number.Value;
            }
            var childMonkeys = (monkeys.First(x => x.Name == monkey.Monkeys!.Value.Item1), monkeys.First(x => x.Name == monkey.Monkeys!.Value.Item2));
            monkey.Number = monkey.Operation!(GetValue(childMonkeys.Item1, monkeys), GetValue(childMonkeys.Item2, monkeys));
            return monkey.Number.Value;
        }

        private void SetChildForEquality(Monkey monkey, long equalityValue, List<Monkey> monkeys)
        {
            var childMonkeys = (monkeys.First(x => x.Name == monkey.Monkeys!.Value.Item1), monkeys.First(x => x.Name == monkey.Monkeys!.Value.Item2));

            var shouldBeEqualTo = -1L;
            var exceptionMonkey = monkey;
            try
            {
                shouldBeEqualTo = GetValue(childMonkeys.Item1, monkeys);
            }
            catch
            {
                exceptionMonkey = childMonkeys.Item1;
            }
            try
            {
                shouldBeEqualTo = GetValue(childMonkeys.Item2, monkeys);
            }
            catch
            {
                exceptionMonkey = childMonkeys.Item2;
            }
        }
    }
}