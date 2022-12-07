namespace AOC_2022.Day9
{
    internal class Day9InputHelper : InputHelper<List<int>>
    {
        public Day9InputHelper(string fileName) : base(fileName)
        {
        }

        public override List<int> Parse()
        {
            var output = new List<int>();
            using (var sr = new StreamReader(InputPath))
            {
                string ln;
                while ((ln = sr.ReadLine()!) != null)
                {
                }
            }
            return output;
        }
    }
}
