namespace AOC_2022.Day23
{
    internal class Day23InputHelper : InputHelper<List<int>>
    {
        public Day23InputHelper(string fileName) : base(fileName)
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
