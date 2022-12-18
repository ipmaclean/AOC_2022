namespace AOC_2022.Day18
{
    public class Day18PuzzleManager : PuzzleManager
    {
        private readonly List<(int X, int Y, int Z)> _adjacentCoordVectors = new List<(int X, int Y, int Z)>()
            {
                (1, 0, 0),
                (-1, 0, 0),
                (0, 1, 0),
                (0, -1, 0),
                (0, 0, 1),
                (0, 0, -1)
            };

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

        public override Task SolvePartTwo()
        {
            var airBubbles = new HashSet<(int X, int Y, int Z)>();
            var solution = 0;
            foreach (var coord in Coords)
            {
                solution += CountEmptyFaces(coord, airBubbles, isPartTwo: true);
            }
            Console.WriteLine($"The solution to part two is '{solution}'.");
            return Task.CompletedTask;
        }
        private int CountEmptyFaces((int X, int Y, int Z) coord, HashSet<(int X, int Y, int Z)>? airBubbles = null, bool isPartTwo = false)
        {
            if (airBubbles is null)
            {
                airBubbles = new HashSet<(int X, int Y, int Z)>();
            }
            var emptyFaceCount = 0;
            foreach (var adjacentCoordVector in _adjacentCoordVectors)
            {
                var adjacentCoord = (coord.X + adjacentCoordVector.X, coord.Y + adjacentCoordVector.Y, coord.Z + adjacentCoordVector.Z);
                if (!Coords.Contains(adjacentCoord))
                {
                    if (isPartTwo)
                    {
                        var coordsBeingChecked = new HashSet<(int X, int Y, int Z)>();
                        if (IsInAirBubble(adjacentCoord, coordsBeingChecked, airBubbles))
                        {
                            airBubbles.UnionWith(coordsBeingChecked);
                            continue;
                        }
                    }
                    emptyFaceCount++;
                }
            }
            return emptyFaceCount;
        }

        private bool IsInAirBubble(
            (int X, int Y, int Z) coord,
            HashSet<(int X, int Y, int Z)> coordsBeingChecked,
            HashSet<(int X, int Y, int Z)> airBubbles)
        {
            if (airBubbles.Contains(coord))
            {
                return true;
            }
            if (CanSeeOutside(coord))
            {
                return false;
            }
            coordsBeingChecked.Add(coord);
            foreach (var adjacentCoordVector in _adjacentCoordVectors)
            {
                var adjacentCoord = (coord.X + adjacentCoordVector.X, coord.Y + adjacentCoordVector.Y, coord.Z + adjacentCoordVector.Z);

                if (coordsBeingChecked.Contains(adjacentCoord) || Coords.Contains(adjacentCoord))
                {
                    continue;
                }

                if (!IsInAirBubble(adjacentCoord, coordsBeingChecked, airBubbles))
                {
                    return false;
                }
            }
            return true;
        }

        private bool CanSeeOutside((int X, int Y, int Z) adjacentCoord)
        {
            if (Coords.Any(coord => coord.X < adjacentCoord.X && coord.Y == adjacentCoord.Y && coord.Z == adjacentCoord.Z) &&
                Coords.Any(coord => coord.X > adjacentCoord.X && coord.Y == adjacentCoord.Y && coord.Z == adjacentCoord.Z) &&
                Coords.Any(coord => coord.X == adjacentCoord.X && coord.Y < adjacentCoord.Y && coord.Z == adjacentCoord.Z) &&
                Coords.Any(coord => coord.X == adjacentCoord.X && coord.Y > adjacentCoord.Y && coord.Z == adjacentCoord.Z) &&
                Coords.Any(coord => coord.X == adjacentCoord.X && coord.Y == adjacentCoord.Y && coord.Z < adjacentCoord.Z) &&
                Coords.Any(coord => coord.X == adjacentCoord.X && coord.Y == adjacentCoord.Y && coord.Z > adjacentCoord.Z))
            {
                return false;
            }
            return true;
        }
    }
}
