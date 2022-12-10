using System.Text.RegularExpressions;

namespace AOC_2022.Day10
{
    internal class Day10InputHelper : InputHelper<List<Input>>
    {
        public Day10InputHelper(string fileName) : base(fileName)
        {
        }

        public override List<Input> Parse()
        {
            var output = new List<Input>();
            using (var sr = new StreamReader(InputPath))
            {
                string ln;
                var numberRegex = new Regex(@"-*\d+");
                while ((ln = sr.ReadLine()!) != null)
                {
                    if (ln == "noop")
                    {
                        output.Add(new Input(Instruction.NoOp));
                    }
                    else if (ln.StartsWith("addx"))
                    {
                        output.Add(
                            new Input(
                                Instruction.AddX,
                                int.Parse(numberRegex.Match(ln).Value)
                            ));
                    }
                }
            }
            return output;
        }
    }
}
