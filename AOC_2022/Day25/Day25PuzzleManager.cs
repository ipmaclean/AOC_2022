using AOC_2022.Day22;
using System.Text;

namespace AOC_2022.Day25
{
    public class Day25PuzzleManager : PuzzleManager
    {
        public List<string> Input { get; set; }

        public Day25PuzzleManager()
        {
            var inputHelper = new Day25InputHelper(INPUT_FILE_NAME);
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
            var sum = 0d;
            foreach (var snafu in Input)
            {
                sum += SnafuToDecimal(snafu);
            }
            Console.WriteLine($"The solution to part one is '{DecimalToSnafu(sum)}'.");
            return Task.CompletedTask;
        }

        private double SnafuToDecimal(string snafu)
        {
            var sum = 0d;
            for (var i = snafu.Length - 1; i >= 0; i--)
            {
                sum += SnafuDigitToDecimal(snafu[i]) * Math.Pow(5, (double)(snafu.Length - 1 - i));
            }
            return sum;
        }

        private string DecimalToSnafu(double decNumber)
        {
            var sb = new StringBuilder();
            // Find the largest power of 5 that when doubled
            // is greater than or equal to the decimal.
            var largestPower = 0;
            while (2 * Math.Pow(5, largestPower) < decNumber)
            {
                largestPower++;
            }
            for (var i = largestPower; i >= 0; i--)
            {
                var closestMultiplierToZero = 0d;
                var closestValueToZero = double.MaxValue;
                for (var multiplier = 2; multiplier >= -2; multiplier--)
                {
                    var remainder = decNumber - multiplier * Math.Pow(5, (double)i);
                    if (Math.Abs(remainder) < Math.Abs(closestValueToZero))
                    {
                        closestValueToZero = remainder;
                        closestMultiplierToZero = multiplier;
                    }
                }
                decNumber = closestValueToZero;
                sb.Append(DecimalDigitToSnafu((int)closestMultiplierToZero));
            }
            return sb.ToString();
        }

        private static double SnafuDigitToDecimal(char digit) => digit switch
        {
            '2' => 2,
            '1' => 1,
            '0' => 0,
            '-' => -1,
            '=' => -2,
            _ => throw new ArgumentOutOfRangeException($"Unexpected digit value: {digit}"),
        };

        private static char DecimalDigitToSnafu(int digit) => digit switch
        {
            2 => '2',
            1 => '1',
            0 => '0',
            -1 => '-',
            -2 => '=',
            _ => throw new ArgumentOutOfRangeException($"Unexpected digit value: {digit}"),
        };

        public override Task SolvePartTwo()
        {
            Console.WriteLine($"Well done, you've saved Christmas!");
            return Task.CompletedTask;
        }
    }
}
