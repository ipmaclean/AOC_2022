namespace AOC_2022.Day6
{
    internal class Day6InputHelper : InputHelper<string>
    {
        public Day6InputHelper(string fileName) : base(fileName)
        {
        }

        public override string Parse()
        {
            var output = string.Empty;
            using (var sr = new StreamReader(InputPath))
            {
                string ln;
                while ((ln = sr.ReadLine()!) != null)
                {
                    output = ln;
                }
            }
            return output;
        }
    }
}
