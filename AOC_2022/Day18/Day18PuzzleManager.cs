namespace AOC_2022.Day18
{
    public class Day18PuzzleManager : PuzzleManager
    {
        //new protected const string INPUT_FILE_NAME = "test.txt";

        public Day18PuzzleManager()
        {
            var inputHelper = new Day18InputHelper(INPUT_FILE_NAME);
            Coords = inputHelper.Parse();
        }

        public HashSet<(int X, int Y, int Z)> Coords { get; set; }

        public override Task SolveBothParts()
        {
            SolvePartOne();
            Console.WriteLine();
            SolvePartTwo();
            return Task.CompletedTask;
        }

        public override Task SolvePartOne()
        {
            var solution = 0;
            foreach (var coord in Coords)
            {
                solution += CountEmptyFaces(coord);
            }
            Console.WriteLine($"The solution to part one is '{solution}'.");
            return Task.CompletedTask;
        }

        private int CountEmptyFaces((int X, int Y, int Z) coord)
        {
            var emptyFaceCount = 0;
            var adjacentCoordVectors = new List<(int X, int Y, int Z)>()
            {
                (1, 0, 0),
                (-1, 0, 0),
                (0, 1, 0),
                (0, -1, 0),
                (0, 0, 1),
                (0, 0, -1)
            };

            foreach (var adjacentCoordVector in adjacentCoordVectors)
            {
                var adjacentCoord = (coord.X + adjacentCoordVector.X, coord.Y + adjacentCoordVector.Y, coord.Z + adjacentCoordVector.Z);
                if (!Coords.Contains(adjacentCoord))
                {
                    emptyFaceCount++;
                }
            }
            return emptyFaceCount;
        }

        public override Task SolvePartTwo()
        {
            var solution = 0;
            Console.WriteLine($"The solution to part two is '{solution}'.");
            return Task.CompletedTask;
        }
    }
}
