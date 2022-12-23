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
                        var face = GetFace(xCoord, yCoord);
                        if (xCoord < ln.Length)
                        {
                            tiles[xCoord, yCoord] = new Tile(ln[xCoord], (xCoord + 1, yCoord + 1), face);
                        }
                        else
                        {
                            tiles[xCoord, yCoord] = new Tile(' ', (xCoord + 1, yCoord + 1), face);
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

        private static int GetFace(int xCoord, int yCoord)
        {
            if (xCoord >= 50 && xCoord <= 99 && yCoord >= 0 && yCoord <= 49)
            {
                return 1;
            }
            if (xCoord >= 100 && xCoord <= 149 && yCoord >= 0 && yCoord <= 49)
            {
                return 2;
            }
            if (xCoord >= 50 && xCoord <= 99 && yCoord >= 50 && yCoord <= 99)
            {
                return 3;
            }
            if (xCoord >= 50 && xCoord <= 99 && yCoord >= 100 && yCoord <= 149)
            {
                return 4;
            }
            if (xCoord >= 0 && xCoord <= 49 && yCoord >= 150 && yCoord <= 199)
            {
                return 5;
            }
            if (xCoord >= 0 && xCoord <= 49 && yCoord >= 100 && yCoord <= 149)
            {
                return 6;
            }
            return 0;
        }

        public (Tile[,] Tiles, string[] Instructions) ParsePartTwo()
        {
            (var tiles, var instructions) = Parse();

            //   1 2
            //   3
            // 6 4
            // 5

            // Link up 1 with adjacent faces (2, 3, 5, 6)
            // 1 and 2, 1 and 3 already linked correctly

            // 1 and 5 (1U x start low count up - 5L y start low count up) 
            for (var i = 0; i < 50; i++)
            {
                var edge1 = tiles[50 + i, 0];
                var edge5 = tiles[0, 150 + i];
                edge1.ConnectedTiles[Direction.Up] = edge5;
                edge5.ConnectedTiles[Direction.Left] = edge1;
            }
            // 1 and 6 (1L y start low count up - 6L y start high count low) 
            for (var i = 0; i < 50; i++)
            {
                var edge1 = tiles[50, i];
                var edge6 = tiles[0, 149 - i];
                edge1.ConnectedTiles[Direction.Left] = edge6;
                edge6.ConnectedTiles[Direction.Left] = edge1;
            }

            // Link up 4 with adjacent faces (2, 3, 5, 6)
            // 4 and 6, 4 and 3 already linked correctly
            // 4 and 2 (4R y start low count up - 2R y start high count down) 
            for (var i = 0; i < 50; i++)
            {
                var edge4 = tiles[99, 100 + i];
                var edge2 = tiles[149, 49 - i];
                edge4.ConnectedTiles[Direction.Right] = edge2;
                edge2.ConnectedTiles[Direction.Right] = edge4;
            }
            // 4 and 5 (4D x start low count up - 5R y start low count up) 
            for (var i = 0; i < 50; i++)
            {
                var edge4 = tiles[50 + i, 149];
                var edge5 = tiles[49, 150 + i];
                edge4.ConnectedTiles[Direction.Down] = edge5;
                edge5.ConnectedTiles[Direction.Right] = edge4;
            }

            // Link up 2 with 3 and 5
            // 2 and 3 (2D x start low count up - 3R y start low count up) 
            for (var i = 0; i < 50; i++)
            {
                var edge2 = tiles[100 + i, 49];
                var edge3 = tiles[99, 50 + i];
                edge2.ConnectedTiles[Direction.Down] = edge3;
                edge3.ConnectedTiles[Direction.Right] = edge2;
            }
            // 2 and 5 (2U x start low count up - 5D x start low count up) 
            for (var i = 0; i < 50; i++)
            {
                var edge2 = tiles[100 + i, 0];
                var edge5 = tiles[i, 199];
                edge2.ConnectedTiles[Direction.Up] = edge5;
                edge5.ConnectedTiles[Direction.Down] = edge2;
            }

            // Link up 6 with 5 and 3
            // 6 and 5 already linked correctly
            // 6 and 3 (6U x start low count up - 3L y start low count up) 
            for (var i = 0; i < 50; i++)
            {
                var edge6 = tiles[i, 100];
                var edge3 = tiles[50, 50 + i];
                edge6.ConnectedTiles[Direction.Up] = edge3;
                edge3.ConnectedTiles[Direction.Left] = edge6;
            }

            return (tiles, instructions);
        }
    }
}
