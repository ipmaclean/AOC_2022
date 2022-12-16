using System.Text.RegularExpressions;

namespace AOC_2022.Day16
{
    internal class Day16InputHelper : InputHelper<List<Valve>>
    {
        public Day16InputHelper(string fileName) : base(fileName)
        {
        }

        public override List<Valve> Parse()
        {
            var output = new List<Valve>();

            var numberRegex = new Regex(@"\d+");
            var valveNameRegex = new Regex(@"[A-Z]{2}");
            using (var sr = new StreamReader(InputPath))
            {
                string ln;
                while ((ln = sr.ReadLine()!) != null)
                {
                    var rate = int.Parse(numberRegex.Match(ln).Value);
                    var valves = valveNameRegex.Matches(ln);
                    var currentValve = valves[0].Value;
                    var connectedValves = new List<string>();
                    for (var i = 1; i < valves.Count; i++)
                    {
                        connectedValves.Add(valves[i].Value);
                    }
                    var valve = new Valve(currentValve, rate, connectedValves);
                    output.Add(valve);
                }
            }
            return output;
        }
    }
}
