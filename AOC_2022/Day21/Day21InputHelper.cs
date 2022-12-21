using System.Text.RegularExpressions;

namespace AOC_2022.Day21
{
    internal class Day21InputHelper : InputHelper<List<Monkey>>
    {
        public Day21InputHelper(string fileName) : base(fileName)
        {
        }

        public override List<Monkey> Parse()
        {
            var output = new List<Monkey>();
            using (var sr = new StreamReader(InputPath))
            {
                var nameRegex = new Regex(@"[a-z]{4}");
                var operationRegex = new Regex(@"[+\-*/]");
                var numberRegex = new Regex(@"\d+");
                string ln;
                while ((ln = sr.ReadLine()!) != null)
                {
                    var nameMatches = nameRegex.Matches(ln);
                    if (nameMatches.Count > 1)
                    {
                        var operation = operationRegex.Match(ln).Value;
                        Func<long, long, long> operationFunc;
                        Func<long, long, long> operationInverse1;
                        Func<long, long, long> operationInverse2;
                        switch (operation)
                        {
                            case "+":
                                operationFunc = (long a, long b) => a + b;
                                operationInverse1 = (long value, long child2) => value - child2;
                                operationInverse2 = (long value, long child1) => value - child1;
                                break;
                            case "-":
                                operationFunc = (long a, long b) => a - b;
                                operationInverse1 = (long value, long child2) => child2 + value;
                                operationInverse2 = (long value, long child1) => child1 - value;
                                break;
                            case "*":
                                operationFunc = (long a, long b) => a * b;
                                operationInverse1 = (long value, long child2) => value / child2;
                                operationInverse2 = (long value, long child1) => value / child1;
                                break;
                            case "/":
                                operationFunc = (long a, long b) => a / b;
                                operationInverse1 = (long value, long child2) => child2 * value;
                                operationInverse2 = (long value, long child1) => child1 / value;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException($"Unexpected operation value: {operation}");
                        };
                        var monkey = new Monkey(
                            nameMatches[0].Value,
                            null,
                            operationFunc, 
                            (nameMatches[1].Value, nameMatches[2].Value),
                            operationInverse1,
                            operationInverse2
                            );
                        output.Add(monkey);
                    }
                    else
                    {
                        var numberMatch = numberRegex.Match(ln).Value;
                        var monkey = new Monkey(
                            nameMatches[0].Value,
                            long.Parse(numberMatch),
                            null,
                            null,
                            null,
                            null);
                        output.Add(monkey);
                    }
                }
            }
            return output;
        }
    }
}
