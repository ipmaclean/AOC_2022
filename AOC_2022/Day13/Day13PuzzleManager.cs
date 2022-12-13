using Newtonsoft.Json.Linq;

namespace AOC_2022.Day13
{
    public class Day13PuzzleManager : PuzzleManager
    {
        private Day13InputHelper _inputHelper;

        public Day13PuzzleManager()
        {
            _inputHelper = new Day13InputHelper(INPUT_FILE_NAME);
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
            var input = _inputHelper.Parse();
            var solution = 0;
            for (var i = 0; i < input.Count; i += 2)
            {
                var order = input[i].CompareTo(input[i + 1]);
                if (order == 0)
                {
                    throw new Exception("Could not identify order of arrays.");
                }
                if (order == -1)
                {
                    solution += (i / 2) + 1;
                }
            }
            Console.WriteLine($"The solution to part one is '{solution}'.");
            return Task.CompletedTask;
        }

        public override Task SolvePartTwo()
        {
            var input = _inputHelper.Parse();
            var divider1 = new Packet(JArray.Parse(@"[[2]]"));
            var divider2 = new Packet(JArray.Parse(@"[[6]]"));
            input.Add(divider1);
            input.Add(divider2);
            input.Sort();
            var solution = (input.IndexOf(divider1) + 1) * (input.IndexOf(divider2) + 1);
            Console.WriteLine($"The solution to part two is '{solution}'.");
            return Task.CompletedTask;
        }
    }
}
