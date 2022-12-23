namespace AOC_2022.Day22
{
    public class Day22PuzzleManager : PuzzleManager
    {
        //new protected const string INPUT_FILE_NAME = "test.txt";

        public Tile[,] TilesPartOne { get; set; }
        public string[] Instructions { get; set; }

        public Day22PuzzleManager()
        {
            var inputHelper = new Day22InputHelper(INPUT_FILE_NAME);
            (TilesPartOne, Instructions) = inputHelper.Parse();
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
            var startingCoords = FindStartingCoords();
            var currentTile = TilesPartOne[startingCoords.X, startingCoords.Y];
            var direction = Direction.Right;
            foreach(var instruction in Instructions)
            {
                if (int.TryParse(instruction, out var distance))
                {
                    for (var i = 0; i < distance; i++)
                    {
                        var nextTile = currentTile.ConnectedTiles[direction];
                        if (nextTile.Type == '#')
                        {
                            break;
                        }
                        currentTile = nextTile;
                    }
                }
                else if (instruction == "L" || instruction == "R")
                {
                    direction = Turn(direction, instruction);
                }
                else
                {
                    throw new ArgumentException("Unexpected instruction.");
                }
            }

            var solution = 1000 * currentTile.Coords.Y + 4 * currentTile.Coords.X + (int)direction;
            Console.WriteLine($"The solution to part one is '{solution}'.");
            return Task.CompletedTask;
        }

        public override Task SolvePartTwo()
        {
            var solution = 0;
            Console.WriteLine($"The solution to part two is '{solution}'.");
            return Task.CompletedTask;
        }

        private (int X, int Y) FindStartingCoords()
        {
            for (var x = 0; x < TilesPartOne.GetLength(0); x++)
            {
                if (TilesPartOne[x, 0].Type != ' ')
                {
                    return (x, 0);
                }
            }
            throw new ArgumentException("Could not find starting coordinates.");
        }

        private Direction Turn(Direction direction, string instruction)
        {
            if (instruction == "L")
            {
                return (Direction)Mod((int)direction - 1, 4);
            }
            else
            {
                return (Direction)Mod((int)direction + 1, 4);
            }
        }

        private int Mod(int x, int m)
        {
            return (x % m + m) % m;
        }
    }
}
