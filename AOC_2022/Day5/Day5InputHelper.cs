using System.Text.RegularExpressions;

namespace AOC_2022.Day5
{
    internal class Day5InputHelper
        : InputHelper<(List<Stack<char>>, List<List<char>>, List<(int Amount, int From, int To)>)>
    {
        public Day5InputHelper(string fileName) : base(fileName)
        {
        }

        public override (List<Stack<char>>, List<List<char>>, List<(int Amount, int From, int To)>) Parse()
        {
            var tempStacks = new List<List<char>>();
            var instructions = new List<(int, int, int)>();
            using (var sr = new StreamReader(InputPath))
            {
                string ln;
                var numberRegex = new Regex(@"\d+");
                while ((ln = sr.ReadLine()!) != String.Empty)
                {
                    if (ln[1] == '1')
                    {
                        continue;
                    }
                    var stackNumber = 0;
                    for (var i = 1; i < ln.Length; i += 4)
                    {
                        if (tempStacks.Count < stackNumber + 1)
                        {
                            tempStacks.Add(new List<char>());
                        }
                        if (ln[i] != ' ')
                        {
                            tempStacks[stackNumber].Add(ln[i]);
                        }
                        stackNumber++;
                    }
                }

                while ((ln = sr.ReadLine()!) != null)
                {
                    var matches = numberRegex.Matches(ln);
                    instructions.Add((int.Parse(matches[0].Value), int.Parse(matches[1].Value) - 1, int.Parse(matches[2].Value) - 1));
                }
            }
            var stacks = new List<Stack<char>>();
            foreach (var tempStack in tempStacks)
            {
                stacks.Add(new Stack<char>(tempStack));
                tempStack.Reverse();
            }
            return (stacks, tempStacks, instructions);
        }
    }
}
