namespace AOC_2022.Day3
{
    public class Day3PuzzleManager : PuzzleManager
    {
        public List<Backpack> Backpacks { get; set; }

        public Day3PuzzleManager()
        {
            var inputHelper = new Day3InputHelper(INPUT_FILE_NAME);
            Backpacks = inputHelper.Parse();
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
            foreach (var backpack in Backpacks)
            {
                solution += GetPriority(backpack.FindSharedItem());
            }
            Console.WriteLine($"The solution to part one is '{solution}'.");
            return Task.CompletedTask;
        }

        private int GetPriority(char item)
            => item >= 'a' ? item - 'a' + 1 : item - 'A' + 27;

        public override Task SolvePartTwo()
        {
            var solution = 0;
            for (var i = 0; i < Backpacks.Count; i += 3)
            {
                solution += GetPriority(
                    FindSharedItem(Backpacks[i], Backpacks[i + 1], Backpacks[i + 2])
                    );
            }
            Console.WriteLine($"The solution to part two is '{solution}'.");
            return Task.CompletedTask;
        }

        private char FindSharedItem(Backpack elf1, Backpack elf2, Backpack elf3)
            => elf1.AllItems.ToCharArray().Intersect(
                   elf2.AllItems.ToCharArray().Intersect(
                       elf3.AllItems.ToCharArray()
                   )).Single();
    }
}
