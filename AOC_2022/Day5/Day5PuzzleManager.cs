namespace AOC_2022.Day5
{
    public class Day5PuzzleManager : PuzzleManager
    {
        public List<Stack<char>> Stacks { get; set; }
        public List<List<char>> Lists { get; set; }
        public List<(int Amount, int From, int To)> Instructions { get; set; }
        public Day5PuzzleManager()
        {
            var inputHelper = new Day5InputHelper(INPUT_FILE_NAME);
            (Stacks, Lists, Instructions) = inputHelper.Parse();
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
            var solution = string.Empty;
            var stacks = CloneStacks();
            foreach (var instruction in Instructions)
            {
                for (var i = 0; i < instruction.Amount; i++)
                {
                    stacks[instruction.To].Push(
                        stacks[instruction.From].Pop()
                        );
                }
            }
            foreach (var stack in stacks)
            {
                solution += stack.Peek();
            }
            Console.WriteLine($"The solution to part one is '{solution}'.");
            return Task.CompletedTask;
        }

        public override Task SolvePartTwo()
        {
            var solution = string.Empty;
            var lists = CloneLists();
            foreach (var instruction in Instructions)
            {
                lists[instruction.To].AddRange(
                    lists[instruction.From].GetRange(
                        lists[instruction.From].Count - instruction.Amount, instruction.Amount
                        ));
                lists[instruction.From].RemoveRange(lists[instruction.From].Count - instruction.Amount, instruction.Amount);
            }
            foreach (var list in lists)
            {
                solution += list.Last();
            }
            Console.WriteLine($"The solution to part two is '{solution}'.");
            return Task.CompletedTask;
        }

        private List<Stack<char>> CloneStacks()
        {
            var stacks = new List<Stack<char>>();
            foreach (var stack in Stacks)
            {
                stacks.Add(new Stack<char>(stack));
            }
            return stacks;
        }

        private List<List<char>> CloneLists()
        {
            var lists = new List<List<char>>();
            foreach (var list in Lists)
            {
                lists.Add(new List<char>(list));
            }
            return lists;
        }
    }
}
