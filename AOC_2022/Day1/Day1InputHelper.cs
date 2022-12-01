namespace AOC_2022.Day1
{
    internal class Day1InputHelper : InputHelper<List<List<int>>>
    {
        public Day1InputHelper(string fileName) : base(fileName)
        {
        }

        public override List<List<int>> Parse()
        {
            var output = new List<List<int>>();
            using (var sr = new StreamReader(InputPath))
            {
                string ln;
                var elf = new List<int>();
                while ((ln = sr.ReadLine()!) != null)
                {
                    if (ln.Trim() == string.Empty)
                    {
                        output.Add(new List<int>(elf));
                        elf = new List<int>();
                    }
                    else
                    {
                        elf.Add(int.Parse(ln));
                    }
                }
                output.Add(new List<int>(elf));
            }
            return output;
        }
    }
}
