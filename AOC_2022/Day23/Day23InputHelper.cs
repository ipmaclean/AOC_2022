namespace AOC_2022.Day23
{
    internal class Day23InputHelper : InputHelper<Dictionary<(int X, int Y), (int X, int Y)>>
    {
        public Day23InputHelper(string fileName) : base(fileName)
        {
        }

        public override Dictionary<(int X, int Y), (int X, int Y)> Parse()
        {
            var output = new Dictionary<(int X, int Y), (int X, int Y)>();

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
                            output.Add((xCoord, yCoord), (0, 0));
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
