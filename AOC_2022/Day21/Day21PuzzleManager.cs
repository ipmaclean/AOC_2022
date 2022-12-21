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

            long? shouldBeEqualTo = null;
            Monkey numberlessMonkey;

            var child1Number = GetValue(childMonkeys.Item1, monkeys);
            var child2Number = GetValue(childMonkeys.Item2, monkeys);
            if (!child1Number.HasValue)
            {
                numberlessMonkey = childMonkeys.Item1;
                shouldBeEqualTo = child2Number;
            }
            else
            {
                numberlessMonkey = childMonkeys.Item2;
                shouldBeEqualTo = child1Number;
            }
            SetChildNumber(numberlessMonkey, shouldBeEqualTo!.Value, monkeys);

            return Task.CompletedTask;
        }

        private long? GetValue(Monkey monkey, List<Monkey> monkeys)
        {
            if (monkey.Number.HasValue)
            {
                return monkey.Number.Value;
            }
            if (!monkey.Number.HasValue && !monkey.Monkeys.HasValue)
            {
                return null;
            }
            var childMonkeys = (monkeys.First(x => x.Name == monkey.Monkeys!.Value.Item1), monkeys.First(x => x.Name == monkey.Monkeys!.Value.Item2));
            var childMonkeyValues = (GetValue(childMonkeys.Item1, monkeys), GetValue(childMonkeys.Item2, monkeys));
            if (!childMonkeyValues.Item1.HasValue || !childMonkeyValues.Item2.HasValue)
            {
                return null;
            }
            monkey.Number = monkey.Operation!(childMonkeyValues.Item1.Value, childMonkeyValues.Item2.Value);
            return monkey.Number;
        }

        private void SetChildNumber(Monkey monkey, long number, List<Monkey> monkeys)
        {
            if (!monkey.Monkeys.HasValue)
            {
                Console.WriteLine($"The solution to part two is '{number}'.");
                return;
            }

            var childMonkeys = (monkeys.First(x => x.Name == monkey.Monkeys!.Value.Item1), monkeys.First(x => x.Name == monkey.Monkeys!.Value.Item2));

            var child1Value = GetValue(childMonkeys.Item1, monkeys);
            var child2Value = GetValue(childMonkeys.Item2, monkeys);


            if (!child1Value.HasValue)
            {
                var numberForChild = monkey.InvOperation1!(number, child2Value!.Value);
                SetChildNumber(childMonkeys.Item1, numberForChild, monkeys);
            }
            else
            {
                var numberForChild = monkey.InvOperation2!(number, child1Value!.Value);
                SetChildNumber(childMonkeys.Item2, numberForChild, monkeys);
            }
        }
    }
}