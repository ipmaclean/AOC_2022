using System.Text.RegularExpressions;

namespace AOC_2022.Day9
{
    internal class Day9InputHelper : InputHelper<List<(char, int)>>
    {
        public Day9InputHelper(string fileName) : base(fileName)
        {
        }

        public override List<(char, int)> Parse()
        {
            var output = new List<(char, int)>();
            using (var sr = new StreamReader(InputPath))
            {
                var numberRegex = new Regex(@"\d+");
                string ln;
                while ((ln = sr.ReadLine()!) != null)
                {
                    var match = numberRegex.Match(ln).Value;
                    output.Add((ln[0], int.Parse(match)));
                }
            }
            return output;
        }
    }
}
