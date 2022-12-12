namespace AOC_2022.Day12
{
    internal class Day12InputHelper : InputHelper<Tile[,]>
    {
        public Day12InputHelper(string fileName) : base(fileName)
        {
        }

        public override Tile[,] Parse()
        {
            var output = new Tile[0, 0];
            var yCount = 0;
            var xCount = 0;
            using (var sr = new StreamReader(InputPath))
            {
                string ln;
                while ((ln = sr.ReadLine()!) != null)
                {
                    xCount = ln.Length;
                    yCount++;
                }
            }

            using (var sr = new StreamReader(InputPath))
            {
                output = new Tile[xCount, yCount];
                var yCoord = 0;
                string ln;
                while ((ln = sr.ReadLine()!) != null)
                {
                    var xCoord = 0;
                    foreach (var tile in ln)
                    {
                        output[xCoord, yCoord] = new Tile(tile, (xCoord++, yCoord));
                    }
                    yCoord++;
                }
            }
            return output;
        }
    }
}
