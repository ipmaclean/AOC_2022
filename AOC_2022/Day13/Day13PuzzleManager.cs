using Newtonsoft.Json.Linq;

namespace AOC_2022.Day13
{
    public class Day13PuzzleManager : PuzzleManager
    {
        //new protected const string INPUT_FILE_NAME = "test.txt";

        public List<(JArray left, JArray right)> Input { get; set; }
        public Day13PuzzleManager()
        {
            var inputHelper = new Day13InputHelper(INPUT_FILE_NAME);
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
            var solution = 0;
            for (var i = 0; i < Input.Count; i++)
            {
                var inCorrectOrder = CompareArrays(Input[i].left, Input[i].right);
                if (!inCorrectOrder.HasValue)
                {
                    throw new Exception("Could not identify order of arrays.");
                }
                if (inCorrectOrder.Value)
                {
                    solution += i + 1;
                }
            }
            Console.WriteLine($"The solution to part one is '{solution}'.");
            return Task.CompletedTask;
        }

        private bool? CompareArrays(JArray left, JArray right)
        {
            for (var i = 0; i < left.Count; i++)
            {
                if (right.Count <= i)
                {
                    return false;
                }
                // Case 1
                if (left[i].Type == JTokenType.Integer &&
                    right[i].Type == JTokenType.Integer)
                {
                    if ((int)left[i] == (int)right[i])
                    {
                        continue;
                    }
                    return (int)left[i] < (int)right[i];
                }
                // Case 2
                if (left[i].Type == JTokenType.Array &&
                    right[i].Type == JTokenType.Array)
                {
                    var comparison = CompareArrays((JArray)left[i], (JArray)right[i]);
                    if (!comparison.HasValue)
                    {
                        continue;
                    }
                    return comparison.Value;
                }
                // Case 3
                else
                {
                    JArray leftArray;
                    JArray rightArray;
                    if (left[i].Type == JTokenType.Array)
                    {
                        leftArray = (JArray)left[i];
                    }
                    else
                    {
                        leftArray = new JArray((int)left[i]);
                    }
                    if (right[i].Type == JTokenType.Array)
                    {
                        rightArray = (JArray)right[i];
                    }
                    else
                    {
                        rightArray = new JArray((int)right[i]);

                    }
                    var comparison = CompareArrays(leftArray, rightArray);
                    if (!comparison.HasValue)
                    {
                        continue;
                    }
                    return comparison.Value;
                }
            }
            if (right.Count > left.Count)
            {
                return true;
            }
            return null;
        }

        public override Task SolvePartTwo()
        {
            var solution = 0;
            Console.WriteLine($"The solution to part two is '{solution}'.");
            return Task.CompletedTask;
        }
    }
}
