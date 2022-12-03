namespace AOC_2022.Day3
{
    internal class Day3InputHelper : InputHelper<List<Backpack>>
    {
        public Day3InputHelper(string fileName) : base(fileName)
        {
        }

        public override List<Backpack> Parse()
        {
            var output = new List<Backpack>();
            using (var sr = new StreamReader(InputPath))
            {
                string ln;
                while ((ln = sr.ReadLine()!) != null)
                {
                    var backpack = new Backpack(
                        ln.Substring(0, ln.Length / 2),
                        ln.Substring(ln.Length / 2));
                    output.Add(backpack);
                }
            }
            return output;
        }
    }
}
