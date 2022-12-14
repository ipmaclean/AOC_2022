namespace AOC_2022.Day14
{
    public class Day14PuzzleManager : PuzzleManager
    {
        private int _maximumYValue;
        private (int, int) _initalSandPosition = (500, 0);
        private HashSet<(int X, int Y)> _sandTiles = new HashSet<(int X, int Y)>();

        public Day14PuzzleManager()
        {
            var inputHelper = new Day14InputHelper(INPUT_FILE_NAME);
            RockTiles = inputHelper.Parse();

            _maximumYValue = 0;

            foreach (var rockTile in RockTiles)
            {
                if (rockTile.Y > _maximumYValue)
                    _maximumYValue = rockTile.Y;
            }
        }

        public HashSet<(int X, int Y)> RockTiles { get; set; }

        public override Task SolveBothParts()
        {
            SolvePartOne();
            Console.WriteLine();
            SolvePartTwo();
            return Task.CompletedTask;
        }

        public override Task SolvePartOne()
        {
            var sandFlowingIntoTheAbyss = false;
            _sandTiles = new HashSet<(int X, int Y)>();
            while (!sandFlowingIntoTheAbyss)
            {
                sandFlowingIntoTheAbyss = AdvanceSandPartOne();
            }
            PrintPretty("Part One");
            Console.WriteLine($"The solution to part one is '{_sandTiles.Count}'.");
            return Task.CompletedTask;
        }

        private bool AdvanceSandPartOne()
        {
            (int X, int Y) sandCoord = _initalSandPosition;
            var sandAtRest = false;
            while (!sandAtRest)
            {
                var newSandCoord = MoveSand(sandCoord);
                if (newSandCoord.Y >= _maximumYValue)
                {
                    return true;
                }
                if (newSandCoord == sandCoord)
                {
                    sandAtRest = true;
                }
                sandCoord = newSandCoord;
            }
            _sandTiles.Add(sandCoord);
            return false;
        }

        public override Task SolvePartTwo()
        {
            var sandBlockingSource = false;
            _sandTiles = new HashSet<(int X, int Y)>();
            while (!sandBlockingSource)
            {
                sandBlockingSource = AdvanceSandPartTwo();
            }
            PrintPretty("Part Two");
            Console.WriteLine($"The solution to part two is '{_sandTiles.Count}'.");
            return Task.CompletedTask;
        }

        private bool AdvanceSandPartTwo()
        {
            (int X, int Y) sandCoord = _initalSandPosition;
            var sandAtRest = false;
            while (!sandAtRest)
            {
                var newSandCoord = MoveSand(sandCoord);
                if (newSandCoord == sandCoord)
                {
                    sandAtRest = true;
                }
                sandCoord = newSandCoord;
            }
            _sandTiles.Add(sandCoord);
            if (sandCoord == _initalSandPosition)
            {
                return true;
            }
            return false;
        }

        private (int X, int Y) MoveSand((int X, int Y) sandCoord)
        {
            var xOffsets = new int[] { 0, -1, 1 };
            foreach (var xOffset in xOffsets)
            {
                if (!RockTiles.Contains((sandCoord.X + xOffset, sandCoord.Y + 1)) &&
                    !_sandTiles.Contains((sandCoord.X + xOffset, sandCoord.Y + 1)) &&
                    sandCoord.Y + 1 < _maximumYValue + 2)
                {
                    return (sandCoord.X + xOffset, sandCoord.Y + 1);
                }
            }
            return sandCoord;
        }

        public void PrintPretty(string fileNameSuffix)
        {
            var minimumXValue = 500;
            var maximumXValue = 500;

            foreach (var rockTile in RockTiles)
            {
                if (rockTile.X > maximumXValue)
                    maximumXValue = rockTile.X;
                if (rockTile.X < minimumXValue)
                    minimumXValue = rockTile.X;
            }

            foreach (var sandTile in _sandTiles)
            {
                if (sandTile.X > maximumXValue)
                    maximumXValue = sandTile.X;
                if (sandTile.X < minimumXValue)
                    minimumXValue = sandTile.X;
            }

            var array = new char[maximumXValue - minimumXValue + 4, _maximumYValue + 3];

            for (var x = 0; x < array.GetLength(0); x++)
            {
                for (var y = 0; y < array.GetLength(1); y++)
                {
                    array[x, y] = '.';
                }
            }

            foreach (var rockTile in RockTiles)
            {
                array[rockTile.X - minimumXValue + 1, rockTile.Y] = '#';
            }
            for (var i = 0; i < array.GetLength(0); i++)
            {
                array[i, _maximumYValue + 2] = '#';
            }

            foreach (var sandTile in _sandTiles)
            {
                array[sandTile.X - minimumXValue + 1, sandTile.Y] = 'o';
            }

            var fullPath = Path.Combine("..", "..", "..", "Day14", $"printPretty - {fileNameSuffix}.txt");
            using (StreamWriter writer = new StreamWriter(fullPath))
            {
                for (int i = 0; i < _maximumYValue + 3; i++)
                {
                    for (int j = 0; j < maximumXValue - minimumXValue + 3; j++)
                    {
                        writer.Write(array[j, i]);
                    }
                    writer.Write(Environment.NewLine);
                }
            }
        }
    }
}
