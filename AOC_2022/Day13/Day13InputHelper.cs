using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AOC_2022.Day13
{
    internal class Day13InputHelper : InputHelper<List<Packet>>
    {
        public Day13InputHelper(string fileName) : base(fileName)
        {
        }

        public override List<Packet> Parse()
        {
            var output = new List<Packet> ();
            using (var sr = new StreamReader(InputPath))
            {
                string ln;
                do
                {
                    var left = JArray.Parse(sr.ReadLine()!);
                    output.Add(new Packet(left));
                    var right = JArray.Parse(sr.ReadLine()!);
                    output.Add(new Packet(right));
                }
                while ((ln = sr.ReadLine()!) != null);
            }
            return output;
        }
    }
}
