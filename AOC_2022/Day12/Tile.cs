namespace AOC_2022.Day12
{
    public class Tile
    {
        public Tile(char elevation, (int x, int y) coords)
        {
            if (elevation == 'S')
            {
                Elevation = 'a';
                IsStart = true;
            }
            else if (elevation == 'E')
            {
                Elevation = 'z';
                IsEnd = true;
            }
            else
            {
                Elevation = elevation;
            }
            Coords = coords;
        }

        public char Elevation { get; set; }
        public (int X, int Y) Coords { get; set; }
        public int ShortestPath { get; set; } = -1;
        public bool IsStart { get; set; } = false;
        public bool IsEnd { get; set; } = false;

        public bool IsReachableFrom(char elevation)
            => Elevation - 1 <= elevation;

        public bool HasBeenMapped()
            => ShortestPath >= 0;
    }
}

