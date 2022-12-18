namespace AOC_2022.Day17
{
    public class Block
    {
        public Block(int type, int highestBlockHeight)
        {
            Coords = new HashSet<(int X, int Y)>();
            switch (type)
            {
                // ####
                case 0:
                    Coords.Add((2, highestBlockHeight + 4));
                    Coords.Add((3, highestBlockHeight + 4));
                    Coords.Add((4, highestBlockHeight + 4));
                    Coords.Add((5, highestBlockHeight + 4));
                    break;

                // .#.
                // ###
                // .#.
                case 1:
                    Coords.Add((3, highestBlockHeight + 6));
                    Coords.Add((2, highestBlockHeight + 5));
                    Coords.Add((3, highestBlockHeight + 5));
                    Coords.Add((4, highestBlockHeight + 5));
                    Coords.Add((3, highestBlockHeight + 4));
                    break;

                // ..#
                // ..#
                // ###
                case 2:
                    Coords.Add((4, highestBlockHeight + 6));
                    Coords.Add((4, highestBlockHeight + 5));
                    Coords.Add((2, highestBlockHeight + 4));
                    Coords.Add((3, highestBlockHeight + 4));
                    Coords.Add((4, highestBlockHeight + 4));
                    break;

                // #
                // #
                // #
                // #
                case 3:
                    Coords.Add((2, highestBlockHeight + 7));
                    Coords.Add((2, highestBlockHeight + 6));
                    Coords.Add((2, highestBlockHeight + 5));
                    Coords.Add((2, highestBlockHeight + 4));
                    break;

                // ##
                // ##
                case 4:
                    Coords.Add((2, highestBlockHeight + 5));
                    Coords.Add((3, highestBlockHeight + 5));
                    Coords.Add((2, highestBlockHeight + 4));
                    Coords.Add((3, highestBlockHeight + 4));
                    break;

                default:
                    throw new ArgumentException("Unexpected block type.");
            }
        }

        public HashSet<(int X, int Y)> Coords { get; set; }

        /// <summary>
        /// Returns true if the block successfully moved down, false otherwise.
        /// </summary>
        public bool TryMoveDown(HashSet<(int x, int y)> existingBlocks)
        {
            HashSet<(int X, int Y)> downCoords = Coords.Select(coord => (coord.X, coord.Y - 1)).ToHashSet();

            // Can use a more performant loop if necessary 
            if (downCoords.Any(coord => coord.Y < 1) ||
                downCoords.Intersect(existingBlocks).Any())
            {
                return false;
            }

            Coords = downCoords;
            return true;
        }

        /// <summary>
        /// Returns true if the block successfully moved left/right, false otherwise.
        /// </summary>
        public bool TryMoveHorizontal(char direction, HashSet<(int x, int y)> existingBlocks)
        {
            var vector = direction == '<' ? -1 : 1;
            HashSet<(int X, int Y)> horizontalCoords = Coords.Select(coord => (coord.X + vector, coord.Y)).ToHashSet();

            // Can use a more performant loop if necessary 
            if (horizontalCoords.Any(coord => coord.X < 0 || coord.X > 6) ||
                horizontalCoords.Intersect(existingBlocks).Any())
            {
                return false;
            }

            Coords = horizontalCoords;
            return true;
        }
    }
}