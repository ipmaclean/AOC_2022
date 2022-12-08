namespace AOC_2022.Day8
{
    internal class Day8InputHelper : InputHelper<Tree[,]>
    {
        public Day8InputHelper(string fileName) : base(fileName)
        {
        }

        public override Tree[,] Parse()
        {
            var output = new Tree[0, 0];
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
                output = new Tree[xCount, yCount];
                var yCoord = 0;
                string ln;
                while ((ln = sr.ReadLine()!) != null)
                {
                    var xCoord = 0;
                    foreach (var tree in ln)
                    {
                        output[xCoord++, yCoord] = new Tree(tree - '0');
                    }
                    yCoord++;
                }
            }
            return output;
        }
    }
}
