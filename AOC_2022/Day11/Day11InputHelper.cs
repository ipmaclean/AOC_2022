using System.Text.RegularExpressions;

namespace AOC_2022.Day11
{
    internal class Day11InputHelper : InputHelper<List<Monkey>>
    {
        public Day11InputHelper(string fileName) : base(fileName)
        {
        }

        public override List<Monkey> Parse()
        {
            var output = new List<Monkey>();
            using (var sr = new StreamReader(InputPath))
            {
                var numberRegex = new Regex(@"\d+");
                string ln;
                do
                {
                    ln = sr.ReadLine()!;

                    // Starting Items
                    ln = sr.ReadLine()!;
                    var startingItems = new Queue<long>();
                    var itemMatches = numberRegex.Matches(ln);
                    foreach (Match match in itemMatches)
                    {
                        startingItems.Enqueue(long.Parse(match.Value));
                    }

                    // Operation Delegate
                    ln = sr.ReadLine()!;
                    Func<long, long> operation;
                    var numberMatch = numberRegex.Match(ln);
                    if (numberMatch.Success)
                    {
                        operation = ln.Contains('+') ?
                            (long old) => old + long.Parse(numberMatch.Value):
                            (long old) => old * long.Parse(numberMatch.Value);
                    }
                    else
                    {
                        operation = ln.Contains('+') ?
                            (long old) => old + old:
                            (long old) => old * old;
                    }

                    // Divisibility Test Number
                    ln = sr.ReadLine()!;
                    var divisibilityTest = long.Parse(numberRegex.Match(ln).Value);

                    // True Monkey
                    ln = sr.ReadLine()!;
                    var trueMonkey = int.Parse(numberRegex.Match(ln).Value);

                    // False monkey
                    ln = sr.ReadLine()!;
                    var falseMonkey = int.Parse(numberRegex.Match(ln).Value);

                    var monkey = new Monkey(startingItems, operation, divisibilityTest, trueMonkey, falseMonkey);
                    output.Add(monkey);
                }
                while ((ln = sr.ReadLine()!) != null);
            }
            return output;
        }
    }
}
