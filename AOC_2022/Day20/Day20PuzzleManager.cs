namespace AOC_2022.Day20
{
    public class Day20PuzzleManager : PuzzleManager
    {
        //new protected const string INPUT_FILE_NAME = "test.txt";
        private Day20InputHelper _inputHelper;

        public Day20PuzzleManager()
        {
            var inputHelper = new Day20InputHelper(INPUT_FILE_NAME);
            _inputHelper = inputHelper;
            inputHelper.Parse();
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
            var numbers = _inputHelper.Parse();
            var count = numbers.Count;
            for (var i = 0; i < count; i++)
            {
                var numberToMove = numbers.First(x => x.OriginalIndex == i);
                var currentIndex = numbers.IndexOf(numberToMove);
                numbers.Remove(numberToMove);
                var newIndex = Mod(currentIndex + numberToMove.Value, numbers.Count);
                numbers.Insert((int)newIndex, numberToMove);
            }

            var zero = numbers.First(x => x.Value == 0);
            var indexOfZero = numbers.IndexOf(zero);
            var solution = 0L;

            for (var i = 0; i < 3; i++)
            {
                indexOfZero = (indexOfZero + 1000) % count;
                solution += numbers[indexOfZero].Value;
            }

            Console.WriteLine($"The solution to part one is '{solution}'.");
            return Task.CompletedTask;
        }

        public override Task SolvePartTwo()
        {
            var numbers = _inputHelper.Parse();
            numbers = numbers.Select(x => new WrappedInt(x.Value * 811_589_153, x.OriginalIndex)).ToList();
            var count = numbers.Count;
            for (var i = 0; i < count; i++)
            {
                var numberToMove = numbers.First(x => x.OriginalIndex == i);
                var currentIndex = numbers.IndexOf(numberToMove);
                numbers.Remove(numberToMove);
                var newIndex = Mod(currentIndex + numberToMove.Value, numbers.Count);
                numbers.Insert((int)newIndex, numberToMove);
            }

            var zero = numbers.First(x => x.Value == 0);
            var indexOfZero = numbers.IndexOf(zero);
            var solution = 0L;

            for (var i = 0; i < 3; i++)
            {
                indexOfZero = (indexOfZero + 1000) % count;
                solution += numbers[indexOfZero].Value;
            }

            Console.WriteLine($"The solution to part two is '{solution}'.");
            return Task.CompletedTask;
        }

        public static long Mod(long a, long b)
        {
            var c = a % b;
            if ((c < 0 && b > 0) || (c > 0 && b < 0))
            {
                c += b;
            }
            return c;
        }
    }
}
