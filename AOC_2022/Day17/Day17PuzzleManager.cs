using System.Diagnostics;

namespace AOC_2022.Day17
{
    public class Day17PuzzleManager : PuzzleManager
    {
        //new protected const string INPUT_FILE_NAME = "test.txt";

        public Day17PuzzleManager()
        {
            var inputHelper = new Day17InputHelper(INPUT_FILE_NAME);
            Directions = inputHelper.Parse();
        }

        public string Directions { get; set; }

        public override Task SolveBothParts()
        {
            SolvePartOne();
            Console.WriteLine();
            SolvePartTwo();
            return Task.CompletedTask;
        }

        public override Task SolvePartOne()
        {
            Console.WriteLine($"The solution to part one is '{DropBlocks(2022)}'.");
            return Task.CompletedTask;
        }

        public override Task SolvePartTwo()
        {
            // The pattern of dropped blocks will repeat.
            // When top 'layer' of blocks is the same and index of directions is the same and block type is the same???

            // If we find the LCM of the number of types of blocks (5) and the length of
            // the input directions then that should give us a height we can just multiply.
            var repeatingCount = Lcm(5, Directions.Length);

            

            var sw = new Stopwatch();
            sw.Start();

            var highestBlockHeight = DropBlocks(repeatingCount);

            sw.Stop();
            Console.WriteLine($"{sw.ElapsedMilliseconds}ms");

            var solution = 0;
            Console.WriteLine($"The solution to part two is '{solution}'.");
            return Task.CompletedTask;
        }

        private int DropBlocks(int numberOfBlocksToDrop)
        {
            var directionsIndex = 0;
            var blockSelectorIndex = 0;
            var existingBlocks = new HashSet<(int x, int y)>();
            var blocksDropped = 0;
            var highestBlockHeight = 0;

            while (blocksDropped++ < numberOfBlocksToDrop)
            {
                var block = new Block(blockSelectorIndex, highestBlockHeight);
                blockSelectorIndex = (blockSelectorIndex + 1) % 5;

                var blockMoving = true;
                while (blockMoving)
                {
                    block.TryMoveHorizontal(Directions[directionsIndex], existingBlocks);
                    directionsIndex = (directionsIndex + 1) % Directions.Length;
                    blockMoving = block.TryMoveDown(existingBlocks);
                }
                existingBlocks.UnionWith(block.Coords);
                highestBlockHeight = Math.Max(highestBlockHeight, block.Coords.Max(coord => coord.Y));
            }

            return highestBlockHeight;
        }

        public static int Lcm(int num1, int num2)
        {
            int x, y = 0;
            x = num1;
            y = num2;
            while (num1 != num2)
            {
                if (num1 > num2)
                {
                    num1 = num1 - num2;
                }
                else
                {
                    num2 = num2 - num1;
                }
            }
            return (x * y) / num1;
        }
    }
}
