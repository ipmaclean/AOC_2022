using System.Text.RegularExpressions;

namespace AOC_2022.Day22
{
    internal class Day22InputHelper : InputHelper<(Tile[,] Tiles, string[] Instructions)>
    {
        public Day22InputHelper(string fileName) : base(fileName)
        {
        }

        public override (Tile[,] Tiles, string[] Instructions) Parse()
        {
            var tiles = new Tile[0, 0];
            var instructionsRaw = string.Empty;
            var yCount = 0;
            var xCount = 0;
            using (var sr = new StreamReader(InputPath))
            {
                string ln;
                while ((ln = sr.ReadLine()!) != string.Empty)
                {
                    xCount = Math.Max(xCount, ln.Length);
                    yCount++;
                }
            }

            using (var sr = new StreamReader(InputPath))
            {
                tiles = new Tile[xCount, yCount];
                var yCoord = 0;
                string ln;
                while ((ln = sr.ReadLine()!) != string.Empty)
                {
                    for (var xCoord = 0; xCoord < tiles.GetLength(0); xCoord++)
                    {
                        if (xCoord < ln.Length)
                        {
                            tiles[xCoord, yCoord] = new Tile(ln[xCoord], (xCoord + 1, yCoord + 1));
                        }
                        else
                        {
                            tiles[xCoord, yCoord] = new Tile(' ', (xCoord + 1, yCoord + 1));
                        }
                    }
                    yCoord++;
                }

                instructionsRaw = sr.ReadLine()!;
            }

            // Link up the tiles
            
            //     Horizontally
            for (var yCoord = 0; yCoord < tiles.GetLength(1); yCoord++)
            {
                Tile? startTile = null;
                var previousTile = tiles[0, yCoord];
                var currentTile = tiles[0, yCoord];
                for (var xCoord = 0; xCoord < tiles.GetLength(0); xCoord++)
                {
                    if (startTile is null && currentTile.Type != ' ')
                    {
                        startTile = currentTile;
                    }
                    Tile nextTile;
                    if (xCoord + 1 >= tiles.GetLength(0) ||
                        (startTile is not null && tiles[xCoord + 1, yCoord].Type == ' '))
                    {
                        nextTile = startTile!;
                        startTile!.ConnectedTiles[Direction.Left] = currentTile;
                    }
                    else
                    {
                        nextTile = tiles[xCoord + 1, yCoord];
                    }
                    if (currentTile.Type != ' ')
                    {
                        currentTile.ConnectedTiles.Add(Direction.Right, nextTile);
                        currentTile.ConnectedTiles.Add(Direction.Left, previousTile);
                    }

                    previousTile = currentTile;
                    currentTile = nextTile;

                    if (xCoord + 1 >= tiles.GetLength(0) ||
                        (startTile is not null && tiles[xCoord + 1, yCoord].Type == ' '))
                    {
                        break;
                    }
                }
            }

            //     Horizontally
            for (var xCoord = 0; xCoord < tiles.GetLength(0); xCoord++)
            {
                Tile? startTile = null;
                var previousTile = tiles[xCoord, 0];
                var currentTile = tiles[xCoord, 0];
                for (var yCoord = 0; yCoord < tiles.GetLength(1); yCoord++)
                {
                    if (startTile is null && currentTile.Type != ' ')
                    {
                        startTile = currentTile;
                    }
                    Tile nextTile;
                    if (yCoord + 1 >= tiles.GetLength(1) ||
                        (startTile is not null && tiles[xCoord, yCoord + 1].Type == ' '))
                    {
                        nextTile = startTile!;
                        startTile!.ConnectedTiles[Direction.Up] = currentTile;
                    }
                    else
                    {
                        nextTile = tiles[xCoord, yCoord + 1];
                    }
                    currentTile.ConnectedTiles.Add(Direction.Down, nextTile);
                    currentTile.ConnectedTiles.Add(Direction.Up, previousTile);

                    previousTile = currentTile;
                    currentTile = nextTile;

                    if (yCoord + 1 >= tiles.GetLength(1) ||
                        (startTile is not null && tiles[xCoord, yCoord + 1].Type == ' '))
                    {
                        break;
                    }
                }
            }

            var instructions = Regex.Split(instructionsRaw, @"([LR])");

            return (tiles, instructions);
        }
    }
}
