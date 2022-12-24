namespace AOC_2022.Day24
{
    internal class Day24InputHelper : InputHelper<List<string>>
    {
        public Day24InputHelper(string fileName) : base(fileName)
        {
        }

        public override List<string> Parse()
        {
            var output = new List<string>();
            using (var sr = new StreamReader(InputPath))
            {
                string ln;
                while ((ln = sr.ReadLine()!) != null)
                {
                    output.Add(ln);
                }
            }
            return output;
        }
    }
}
