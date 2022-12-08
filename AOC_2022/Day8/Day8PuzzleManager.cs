namespace AOC_2022.Day8
{
    public class Day8PuzzleManager : PuzzleManager
    {
        public Tree[,] Trees { get; set; }
        public Day8PuzzleManager()
        {
            var inputHelper = new Day8InputHelper(INPUT_FILE_NAME);
            Trees = inputHelper.Parse();
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

            for (var row = 0; row < Trees.GetLength(1); row++)
            {
                var currentHeight = -1;
                for (var xCoord = 0; xCoord < Trees.GetLength(0); xCoord++)
                {
                    if (Trees[xCoord, row].Height > currentHeight)
                    {
                        Trees[xCoord, row].SetVisible();
                        currentHeight = Trees[xCoord, row].Height;
                    }
                }
                currentHeight = -1;
                for (var xCoord = Trees.GetLength(0) - 1; xCoord >= 0; xCoord--)
                {
                    if (Trees[xCoord, row].Height > currentHeight)
                    {
                        Trees[xCoord, row].SetVisible();
                        currentHeight = Trees[xCoord, row].Height;
                    }
                }
            }
            for (var col = 0; col < Trees.GetLength(0); col++)
            {
                var currentHeight = -1;
                for (var yCoord = 0; yCoord < Trees.GetLength(1); yCoord++)
                {
                    if (Trees[col, yCoord].Height > currentHeight)
                    {
                        Trees[col, yCoord].SetVisible();
                        currentHeight = Trees[col, yCoord].Height;
                    }
                }
                currentHeight = -1;
                for (var yCoord = Trees.GetLength(1) - 1; yCoord >= 0; yCoord--)
                {
                    if (Trees[col, yCoord].Height > currentHeight)
                    {
                        Trees[col, yCoord].SetVisible();
                        currentHeight = Trees[col, yCoord].Height;
                    }
                }
            }

            Console.WriteLine($"The solution to part one is '{Trees.Cast<Tree>().Count(x => x.IsVisible)}'.");
            return Task.CompletedTask;
        }

        public Task SolvePartOneOld()
        {
            //var maxX = Trees.Max(x => x.XCoord);
            //var maxY = Trees.Max(x => x.YCoord);

            //for (var y = 0; y <= maxY; y++)
            //{
            //    FindAndSetVisibleTrees('x', y, '<', 0, maxX);
            //    FindAndSetVisibleTrees('x', y, '>', 0, maxX);
            //}

            //for (var x = 0; x <= maxX; x++)
            //{
            //    FindAndSetVisibleTrees('y', x, '<', 0, maxY);
            //    FindAndSetVisibleTrees('y', x, '>', 0, maxY);
            //}

            //Console.WriteLine($"The solution to part one is '{Trees.Count(x => x.IsVisible)}'.");
            return Task.CompletedTask;
        }

        // This recursive solution was actally very slow. Too much Max().
        // I could cut the time in half by allowing a null directionOfView for 
        // the outermost layer of recursion but I don't think it's worth the effort!

        // Ended up completely rewriting the solution but kept the old methods for posterity.
        // Trees used to just be a List and each Tree object had xCoord and yCoord properties.
        private void FindAndSetVisibleTrees(char axis, int fixedCoord, char directionOfView, int minCoord, int maxCoord)
        {
            //if (maxCoord < minCoord)
            //{
            //    return;
            //}
            //if (axis == 'x')
            //{
            //    var treesInSection = Trees.Where(x => x.YCoord == fixedCoord && x.XCoord >= minCoord && x.XCoord <= maxCoord);
            //    var tallestTreesInSection = treesInSection.Where(x => x.Height == treesInSection.Max(x => x.Height));
            //    if (directionOfView == '<')
            //    {
            //        var visibleTree = tallestTreesInSection.OrderBy(x => x.XCoord).First();
            //        visibleTree.SetVisible();
            //        FindAndSetVisibleTrees(axis, fixedCoord, directionOfView, minCoord, visibleTree.XCoord - 1);
            //    }
            //    else if (directionOfView == '>')
            //    {
            //        var visibleTree = tallestTreesInSection.OrderByDescending(x => x.XCoord).First();
            //        visibleTree.SetVisible();
            //        FindAndSetVisibleTrees(axis, fixedCoord, directionOfView, visibleTree.XCoord + 1, maxCoord);
            //    }
            //    else
            //    {
            //        throw new ArgumentException();
            //    }
            //}
            //else if (axis == 'y')
            //{
            //    var treesInSection = Trees.Where(x => x.XCoord == fixedCoord && x.YCoord >= minCoord && x.YCoord <= maxCoord);
            //    var tallestTreesInSection = treesInSection.Where(x => x.Height == treesInSection.Max(x => x.Height));
            //    if (directionOfView == '<')
            //    {
            //        var visibleTree = tallestTreesInSection.OrderBy(x => x.YCoord).First();
            //        visibleTree.SetVisible();
            //        FindAndSetVisibleTrees(axis, fixedCoord, directionOfView, minCoord, visibleTree.YCoord - 1);
            //    }
            //    else if (directionOfView == '>')
            //    {
            //        var visibleTree = tallestTreesInSection.OrderByDescending(x => x.YCoord).First();
            //        visibleTree.SetVisible();
            //        FindAndSetVisibleTrees(axis, fixedCoord, directionOfView, visibleTree.YCoord + 1, maxCoord);
            //    }
            //    else
            //    {
            //        throw new ArgumentException();
            //    }
            //}
            //else
            //{
            //    throw new ArgumentException();
            //}
        }

        public override Task SolvePartTwo()
        {
            var solution = 0;
            for (var row = 0; row < Trees.GetLength(1); row++)
            {
                for (var xCoord = 0; xCoord < Trees.GetLength(0); xCoord++)
                {
                    solution = Math.Max(solution, GetScenicScore(xCoord, row));
                }
            }

            Console.WriteLine($"The solution to part two is '{solution}'.");
            return Task.CompletedTask;
        }

        private int GetScenicScore(int xCoord, int yCoord)
        {
            var scenicScoreRight = 0;
            for (var x = xCoord + 1; x < Trees.GetLength(0); x++)
            {
                scenicScoreRight++;
                if (Trees[x, yCoord].Height >= Trees[xCoord, yCoord].Height)
                {
                    break;
                }
            }
            var scenicScoreLeft = 0;
            for (var x = xCoord - 1; x >= 0; x--)
            {
                scenicScoreLeft++;
                if (Trees[x, yCoord].Height >= Trees[xCoord, yCoord].Height)
                {
                    break;
                }
            }
            var scenicScoreDown = 0;
            for (var y = yCoord + 1; y < Trees.GetLength(1); y++)
            {
                scenicScoreDown++;
                if (Trees[xCoord, y].Height >= Trees[xCoord, yCoord].Height)
                {
                    break;
                }
            }
            var scenicScoreUp = 0;
            for (var y = yCoord - 1; y >= 0; y--)
            {
                scenicScoreUp++;
                if (Trees[xCoord, y].Height >= Trees[xCoord, yCoord].Height)
                {
                    break;
                }
            }
            return scenicScoreRight * scenicScoreLeft * scenicScoreDown * scenicScoreUp;
        }
    }
}
