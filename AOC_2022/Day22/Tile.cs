namespace AOC_2022.Day22
{
    public class Tile
    {
        public Tile(char type, (int X, int Y) coords)
        {
            Type = type;
            Coords = coords;
        }

        public char Type { get; set; }
        public (int X, int Y) Coords { get; set; }
        public Dictionary<Direction, Tile> ConnectedTiles { get; set; } = new Dictionary<Direction, Tile>();
    }
}
