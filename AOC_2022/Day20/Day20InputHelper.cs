namespace AOC_2022.Day20
{
    internal class Day20InputHelper : InputHelper<List<WrappedInt>>
    {
        public Day20InputHelper(string fileName) : base(fileName)
        {
        }

        public override List<WrappedInt> Parse()
        {
            var output = new List<WrappedInt>();
            using (var sr = new StreamReader(InputPath))
            {
                string ln;
                var index = 0;
                while ((ln = sr.ReadLine()!) != null)
                {
                    output.Add(new WrappedInt(long.Parse(ln), index++));
                }
            }
            return output;
        }
    }
}
