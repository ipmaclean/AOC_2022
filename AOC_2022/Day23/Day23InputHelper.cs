namespace AOC_2022.Day23
{
    internal class Day23InputHelper : InputHelper<HashSet<Elf>>
    {
        public Day23InputHelper(string fileName) : base(fileName)
        {
        }

        public override HashSet<Elf> Parse()
        {
            var output = new HashSet<Elf>();

            using (var sr = new StreamReader(InputPath))
            {
                var yCoord = 0;
                string ln;
                while ((ln = sr.ReadLine()!) != null)
                {
                    var xCoord = 0;
                    foreach (var tile in ln)
                    {
                        if (tile == '#')
                        {
                            output.Add(new Elf((xCoord, yCoord)));
                        }
                        xCoord++;
                    }
                    yCoord++;
                }
            }
            return output;
        }
    }
}
