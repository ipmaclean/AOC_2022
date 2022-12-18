using System.Text.RegularExpressions;

namespace AOC_2022.Day18
{
    internal class Day18InputHelper : InputHelper<HashSet<(int X, int Y, int Z)>>
    {
        public Day18InputHelper(string fileName) : base(fileName)
        {
        }

        public override HashSet<(int X, int Y, int Z)> Parse()
        {
            var output = new HashSet<(int X, int Y, int Z)>();
            using (var sr = new StreamReader(InputPath))
            {
                string ln;
                var numberRegex = new Regex(@"\d+");
                while ((ln = sr.ReadLine()!) != null)
                {
                    var numberMatches = numberRegex.Matches(ln);
                    output.Add((int.Parse(numberMatches[0].Value), int.Parse(numberMatches[1].Value), int.Parse(numberMatches[2].Value)));
                }
            }
            return output;
        }
    }
}
