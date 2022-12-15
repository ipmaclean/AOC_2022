using System.Text.RegularExpressions;

namespace AOC_2022.Day15
{
    internal class Day15InputHelper : InputHelper<List<Sensor>>
    {
        public Day15InputHelper(string fileName) : base(fileName)
        {
        }

        public override List<Sensor> Parse()
        {
            var output = new List<Sensor>();
            using (var sr = new StreamReader(InputPath))
            {
                string ln;
                var numberRegex = new Regex(@"-*\d+");
                while ((ln = sr.ReadLine()!) != null)
                {
                    var matches = numberRegex.Matches(ln);
                    var sensor = new Sensor(
                        ((long.Parse(matches[0].Value)), (long.Parse(matches[1].Value))),
                        ((long.Parse(matches[2].Value)), (long.Parse(matches[3].Value)))
                        );
                    output.Add(sensor);
                }
            }
            return output;
        }
    }
}
