using System.Text.RegularExpressions;

namespace AOC_2022.Day14
{
    internal class Day14InputHelper : InputHelper<HashSet<(int, int)>>
    {
        public Day14InputHelper(string fileName) : base(fileName)
        {
        }

        public override HashSet<(int, int)> Parse()
        {
            var output = new HashSet<(int, int)>();
            using (var sr = new StreamReader(InputPath))
            {
                string ln;
                var numberRegex = new Regex(@"\d+");
                while ((ln = sr.ReadLine()!) != null)
                {
                    var numberMatches = numberRegex.Matches(ln);

                    for (var i = 0; i < numberMatches.Count - 2; i += 2)
                    {
                        (int X, int Y) firstCoord = (int.Parse(numberMatches[i].Value), int.Parse(numberMatches[i + 1].Value));
                        (int X, int Y) secondCoord = (int.Parse(numberMatches[i + 2].Value), int.Parse(numberMatches[i + 3].Value));

                        if (firstCoord.X == secondCoord.X)
                        {
                            var xValue = firstCoord.X;
                            var yValues = Enumerable.Range(
                                Math.Min(firstCoord.Y, secondCoord.Y),
                                Math.Abs(firstCoord.Y - secondCoord.Y) + 1
                                );

                            foreach (var yValue in yValues)
                            {
                                if (!output.Contains((xValue, yValue)))
                                    output.Add((xValue, yValue));
                            }
                        }
                        else
                        {
                            var yValue = firstCoord.Y;
                            var xValues = Enumerable.Range(
                                Math.Min(firstCoord.X, secondCoord.X),
                                Math.Abs(firstCoord.X - secondCoord.X) + 1
                                );

                            foreach (var xValue in xValues)
                            {
                                if (!output.Contains((xValue, yValue)))
                                    output.Add((xValue, yValue));
                            }
                        }
                    }
                }
            }
            return output;
        }
    }
}
