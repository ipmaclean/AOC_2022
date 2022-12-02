namespace AOC_2022.Day2
{
    internal class Day2InputHelper : InputHelper<List<RpsGame>>
    {
        public Day2InputHelper(string fileName) : base(fileName)
        {
        }

        public override List<RpsGame> Parse()
        {
            var shapeDictionary = new Dictionary<char, Shape>()
            {
                { 'A', Shape.Rock },
                { 'B', Shape.Paper },
                { 'C', Shape.Scissors },
                { 'X', Shape.Rock },
                { 'Y', Shape.Paper },
                { 'Z', Shape.Scissors }
            };

            var endStateDictionary = new Dictionary<char, EndState>()
            {
                { 'X', EndState.Loss },
                { 'Y', EndState.Draw },
                { 'Z', EndState.Win }
            };

            var output = new List<RpsGame>();
            using (var sr = new StreamReader(InputPath))
            {
                string ln;
                while ((ln = sr.ReadLine()!) != null)
                {
                    output.Add(
                        new RpsGame(
                            shapeDictionary[ln[0]],
                            shapeDictionary[ln[2]],
                            endStateDictionary[ln[2]]
                        ));
                }
            }
            return output;
        }
    }
}
