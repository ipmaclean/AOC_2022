namespace AOC_2022.Day17
{
    internal class Day17InputHelper : InputHelper<string>
    {
        public Day17InputHelper(string fileName) : base(fileName)
        {
        }

        public override string Parse()
        {
            using (var sr = new StreamReader(InputPath))
            {
                string ln;
                while ((ln = sr.ReadLine()!) != null)
                {
                    return ln;
                }
            }
            return string.Empty;
        }
    }
}
