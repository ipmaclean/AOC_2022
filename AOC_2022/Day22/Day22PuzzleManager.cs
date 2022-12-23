namespace AOC_2022.Day22
{
    public class Day22PuzzleManager : PuzzleManager
    {
        private Day22InputHelper _inputHelper;

        public Day22PuzzleManager()
        {
            _inputHelper = new Day22InputHelper(INPUT_FILE_NAME);
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
            (var tiles, var instructions) = _inputHelper.Parse();
            var startingCoords = FindStartingCoords(tiles);
            var currentTile = tiles[startingCoords.X, startingCoords.Y];
            var direction = Direction.Right;
            foreach(var instruction in instructions)
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

        // I have just gone for a horrible hard coded solution for part two
        public override Task SolvePartTwo()
        {
            (var tiles, var instructions) = _inputHelper.ParsePartTwo();
            var startingCoords = FindStartingCoords(tiles);
            var currentTile = tiles[startingCoords.X, startingCoords.Y];
            var direction = Direction.Right;
            foreach (var instruction in instructions)
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
                        if (currentTile.Face != nextTile.Face)
                        {
                            direction = PartTwoDirectionCalculator(currentTile.Face, nextTile.Face);
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
            Console.WriteLine($"The solution to part two is '{solution}'.");
            return Task.CompletedTask;
        }

        private static (int X, int Y) FindStartingCoords(Tile[,] tiles)
        {
            for (var x = 0; x < tiles.GetLength(0); x++)
            {
                if (tiles[x, 0].Type != ' ')
                {
                    return (x, 0);
                }
            }
            throw new ArgumentException("Could not find starting coordinates.");
        }

        private static Direction Turn(Direction direction, string instruction)
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

        private static Direction PartTwoDirectionCalculator(int fromFace, int toFace)
        {
            if (fromFace == 1)
            {
                if (toFace == 2)
                {
                    return Direction.Right;
                }
                if (toFace == 3)
                {
                    return Direction.Down;
                }
                if (toFace == 5)
                {
                    return Direction.Right;
                }
                if (toFace == 6)
                {
                    return Direction.Right;
                }
            }
            if (fromFace == 2)
            {
                if (toFace == 1)
                {
                    return Direction.Left;
                }
                if (toFace == 3)
                {
                    return Direction.Left;
                }
                if (toFace == 5)
                {
                    return Direction.Up;
                }
                if (toFace == 4)
                {
                    return Direction.Left;
                }
            }
            if (fromFace == 3)
            {
                if (toFace == 1)
                {
                    return Direction.Up;
                }
                if (toFace == 6)
                {
                    return Direction.Down;
                }
                if (toFace == 2)
                {
                    return Direction.Up;
                }
                if (toFace == 4)
                {
                    return Direction.Down;
                }
            }
            if (fromFace == 4)
            {
                if (toFace == 2)
                {
                    return Direction.Left;
                }
                if (toFace == 3)
                {
                    return Direction.Up;
                }
                if (toFace == 5)
                {
                    return Direction.Left;
                }
                if (toFace == 6)
                {
                    return Direction.Left;
                }
            }
            if (fromFace == 5)
            {
                if (toFace == 1)
                {
                    return Direction.Down;
                }
                if (toFace == 2)
                {
                    return Direction.Down;
                }
                if (toFace == 6)
                {
                    return Direction.Up;
                }
                if (toFace == 4)
                {
                    return Direction.Up;
                }
            }
            if (fromFace == 6)
            {
                if (toFace == 1)
                {
                    return Direction.Right;
                }
                if (toFace == 5)
                {
                    return Direction.Down;
                }
                if (toFace == 3)
                {
                    return Direction.Right;
                }
                if (toFace == 4)
                {
                    return Direction.Right;
                }
            }
            throw new ArgumentException("Could not find direction after traversing faces.");
        }

        private static int Mod(int x, int m)
        {
            return (x % m + m) % m;
        }
    }
}
