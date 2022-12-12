namespace AOC_2022.Day12
{
    public class Day12PuzzleManager : PuzzleManager
    {
        //new protected const string INPUT_FILE_NAME = "test.txt";
        private Day12InputHelper _inputHelper { get; set; }

        public Day12PuzzleManager()
        {
            _inputHelper = new Day12InputHelper(INPUT_FILE_NAME);
        }
        public override Task SolveBothParts()
        {
            SolvePartOne();
            Console.WriteLine();
            SolvePartTwo();
            return Task.CompletedTask;
        }

        public override Task SolvePartOne()
        {
            var tiles = _inputHelper.Parse();
            var startTile = FindStart(tiles);
            
            startTile.ShortestPath = 0;
            var solution = BreadthFirstSearch(startTile, tiles);

            Console.WriteLine($"The solution to part one is '{solution}'.");
            return Task.CompletedTask;
        }

        public override Task SolvePartTwo()
        {
            var tiles = _inputHelper.Parse();
            var allStartTiles = FindAllStarts(tiles);

            var solution = int.MaxValue;

            foreach (var startTile in allStartTiles)
            {
                startTile.ShortestPath = 0;
                solution = Math.Min(solution, BreadthFirstSearch(startTile, tiles));
                ResetShortestPaths(tiles);
            }

            Console.WriteLine($"The solution to part two is '{solution}'.");
            return Task.CompletedTask;
        }

        public int BreadthFirstSearch(Tile startTile, Tile[,] tiles)
        {
            var tilesToVisit = new Queue<Tile>();
            tilesToVisit.Enqueue(startTile);

            var directions = new (int X, int Y)[]
            {
                (-1, 0),
                (1, 0),
                (0, -1),
                (0, 1)
            };

            while (tilesToVisit.TryDequeue(out var currentTile))
            {
                foreach (var direction in directions)
                {
                    (var x, var y) = (currentTile.Coords.X + direction.X, currentTile.Coords.Y + direction.Y);
                    if (x >= 0 && x <= tiles.GetLength(0) - 1 && y >= 0 && y <= tiles.GetLength(1) - 1)
                    {
                        var nextTile = tiles[x, y];
                        if (nextTile.IsReachableFrom(currentTile.Elevation) && nextTile.ShortestPath < 0)
                        {
                            nextTile.ShortestPath = currentTile.ShortestPath + 1;
                            if (nextTile.IsEnd)
                            {
                                return nextTile.ShortestPath;
                            }
                            tilesToVisit.Enqueue(nextTile);
                        }
                    }
                }
            }
            return int.MaxValue;
        }

        public Tile FindStart(Tile[,] tiles)
        {
            for (var y = 0; y < tiles.GetLength(1); y++)
            {
                for (var x = 0; x < tiles.GetLength(0); x++)
                {
                    if (tiles[x, y].IsStart)
                    {
                        return tiles[x, y];
                    }
                }
            }
            throw new Exception("Could not find starting tile.");
        }

        public List<Tile> FindAllStarts(Tile[,] tiles)
        {
            var allStarts = new List<Tile>();
            for (var y = 0; y < tiles.GetLength(1); y++)
            {
                for (var x = 0; x < tiles.GetLength(0); x++)
                {
                    if (tiles[x, y].Elevation == 'a')
                    {
                        allStarts.Add(tiles[x, y]);
                    }
                }
            }
            return allStarts;
        }

        public void ResetShortestPaths(Tile[,] tiles)
        {
            for (var y = 0; y < tiles.GetLength(1); y++)
            {
                for (var x = 0; x < tiles.GetLength(0); x++)
                {
                    tiles[x, y].ShortestPath = -1;
                }
            }
        }
    }
}
