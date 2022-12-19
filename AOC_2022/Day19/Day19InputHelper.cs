using System.Text.RegularExpressions;

namespace AOC_2022.Day19
{
    internal class Day19InputHelper : InputHelper<List<Blueprint>>
    {
        public Day19InputHelper(string fileName) : base(fileName)
        {
        }

        public override List<Blueprint> Parse()
        {
            var output = new List<Blueprint>();
            using (var sr = new StreamReader(InputPath))
            {
                var numberRegex = new Regex(@"\d+");
                string ln;
                while ((ln = sr.ReadLine()!) != null)
                {
                    var matches = numberRegex.Matches(ln);
                    var blueprint = new Blueprint(
                        int.Parse(matches[0].Value),
                        int.Parse(matches[1].Value),
                        int.Parse(matches[2].Value),
                        (int.Parse(matches[3].Value), int.Parse(matches[4].Value)),
                        (int.Parse(matches[5].Value), int.Parse(matches[6].Value))
                        );
                    output.Add(blueprint);
                }
            }
            return output;
        }
    }
}
