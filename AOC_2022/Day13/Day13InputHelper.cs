using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AOC_2022.Day13
{
    internal class Day13InputHelper : InputHelper<List<(JArray left, JArray right)>>
    {
        public Day13InputHelper(string fileName) : base(fileName)
        {
        }

        public override List<(JArray left, JArray right)> Parse()
        {
            var output = new List<(JArray left, JArray right)>();
            using (var sr = new StreamReader(InputPath))
            {
                string ln;
                do
                {
                    var left = JArray.Parse(sr.ReadLine()!);
                    var right = JArray.Parse(sr.ReadLine()!);
                    output.Add((left, right));
                }
                while ((ln = sr.ReadLine()!) != null);
            }
            return output;
        }
    }
}
