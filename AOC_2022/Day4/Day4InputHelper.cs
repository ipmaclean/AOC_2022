using System.Text.RegularExpressions;

namespace AOC_2022.Day4
{
    internal class Day4InputHelper : InputHelper<List<ElfTeam>>
    {
        public Day4InputHelper(string fileName) : base(fileName)
        {
        }

        public override List<ElfTeam> Parse()
        {
            var output = new List<ElfTeam>();
            using (var sr = new StreamReader(InputPath))
            {
                var numberRegex = new Regex(@"\d+");
                string ln;
                while ((ln = sr.ReadLine()!) != null)
                {
                    var matches = numberRegex.Matches(ln);
                    var elves = new ElfTeam(
                        (int.Parse(matches[0].Value), int.Parse(matches[1].Value)),
                        (int.Parse(matches[2].Value), int.Parse(matches[3].Value))
                    );
                    output.Add(elves);
                }
            }
            return output;
        }
    }
}
