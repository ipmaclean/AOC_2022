using System.Diagnostics;
using System.Text;

namespace AOC_2022.Day25
{
    public class Day25PuzzleManager : PuzzleManager
    {
        public List<string> Input { get; set; }

        private Dictionary<char, int> _snafuDigitToDecimal = new Dictionary<char, int>()
        {
            { '2', 2 },
            {'1', 1 },
            {'0', 0 },
            { '-', -1 },
            { '=', -2 }
        };

        private Dictionary<double, char> _decimalDigitToSnafu = new Dictionary<double, char>()
        {
            { 2, '2' },
            { 1, '1' },
            { 0, '0' },
            { -1, '-' },
            { -2, '=' }
        };

        public Day25PuzzleManager()
        {
            var inputHelper = new Day25InputHelper(INPUT_FILE_NAME);
            Input = inputHelper.Parse();
        }
        public override Task SolveBothParts()
        {
            var sw = new Stopwatch();
            sw.Start();
            SolvePartOne();
            sw.Stop();
            Console.WriteLine($"Part one: {sw.ElapsedMilliseconds}ms.");
            Console.WriteLine();
            sw.Restart();
            SolvePartTwo();
            sw.Stop();
            Console.WriteLine($"Part two: {sw.ElapsedMilliseconds}ms.");
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
                sum += _snafuDigitToDecimal[snafu[i]] * Math.Pow(5, (double)(snafu.Length - 1 - i));
            }
            return sum;
        }

        private string DecimalToSnafu(double decNumber)
        {
            var sb = new StringBuilder();
            // Find the largest power of 5 that when doubled
            // is greater than or equal to the decimal.
            var largestPower = 0;
            while (2 * Math.Pow(5, largestPower) < Math.Abs(decNumber))
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
                sb.Append(_decimalDigitToSnafu[closestMultiplierToZero]);
            }
            return sb.ToString();
        }

        public override Task SolvePartTwo()
        {
            var solution = "0";
            foreach (var snafu in Input)
            {
                solution = AddSnafu(solution, snafu);
            }
            Console.WriteLine($"The solution to part two is '{solution}'.");
            return Task.CompletedTask;
        }

        private string AddSnafu(string a, string b)
        {
            var shorterString = a.Length < b.Length ? a : b;
            var longerString = a.Length >= b.Length ? a : b;
            shorterString = shorterString.PadLeft(longerString.Length, '0');

            var sb = new StringBuilder();
            var carried = 0;
            for (var i = 0; i < shorterString.Length; i++)
            {
                var addition = _snafuDigitToDecimal[shorterString[shorterString.Length - i - 1]] +
                                _snafuDigitToDecimal[longerString[longerString.Length - i - 1]] +
                                    carried;
                carried = 0;
                if (addition % 3 != addition)
                {
                    carried = addition / Math.Abs(addition);
                }
                sb.Append(_decimalDigitToSnafu[Mod(addition + 2, 5) - 2]);
            }
            if (carried != 0)
            {
                sb.Append(_decimalDigitToSnafu[carried]);
            }
            return new string(sb.ToString().ToCharArray().Reverse().ToArray());
        }

        private static int Mod(int x, int m)
        {
            return (x % m + m) % m;
        }
    }
}
