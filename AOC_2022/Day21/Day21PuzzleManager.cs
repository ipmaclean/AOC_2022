namespace AOC_2022.Day21
{
    public class Day21PuzzleManager : PuzzleManager
    {
        private Day21InputHelper _inputHelper;

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
            SetChildNumber(exceptionMonkey, shouldBeEqualTo, monkeys);

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

        private void SetChildNumber(Monkey monkey, long number, List<Monkey> monkeys)
        {
            if (!monkey.Monkeys.HasValue)
            {
                Console.WriteLine($"The solution to part two is '{number}'.");
                return;
            }

            var childMonkeys = (monkeys.First(x => x.Name == monkey.Monkeys!.Value.Item1), monkeys.First(x => x.Name == monkey.Monkeys!.Value.Item2));

            long? child1Value = null;
            long? child2Value = null;

            try
            {
                child1Value = GetValue(childMonkeys.Item1, monkeys);
            }
            catch { }
            try
            {
                child2Value = GetValue(childMonkeys.Item2, monkeys);
            }
            catch { }

            if (!child1Value.HasValue && !child2Value.HasValue)
            {
                throw new InvalidOperationException("Neither child monkey had a value.");
            }

            var numberForChild = !child1Value.HasValue ?
                monkey.InvOperation1!(number, child2Value!.Value) :
                monkey.InvOperation2!(number, child1Value!.Value);

            if (!child1Value.HasValue)
            {
                SetChildNumber(childMonkeys.Item1, numberForChild, monkeys);
            }
            else
            {
                SetChildNumber(childMonkeys.Item2, numberForChild, monkeys);
            }
        }
    }
}