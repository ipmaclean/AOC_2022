using System.Diagnostics;
using System.Text;

namespace AOC_2022.Day17
{
    public class Day17PuzzleManager : PuzzleManager
    {
        private Dictionary<long, long> _droppedBlocksToHeightMap = new Dictionary<long, long>();

        public Day17PuzzleManager()
        {
            var inputHelper = new Day17InputHelper(INPUT_FILE_NAME);
            Directions = inputHelper.Parse();
        }

        public string Directions { get; set; }

        public override Task SolveBothParts()
        {
            SolvePartOne();
            Console.WriteLine();
            SolvePartTwo();
            return Task.CompletedTask;
        }

        public override Task SolvePartOne()
        {
            Console.WriteLine($"The solution to part one is '{Solve(2022)}'.");
            return Task.CompletedTask;
        }

        public override Task SolvePartTwo()
        {
            Console.WriteLine($"The solution to part two is '{Solve(1_000_000_000_000)}'.");
            return Task.CompletedTask;
        }

        private long Solve(long blocksToDrop)
        {
            // The pattern of dropped blocks will repeat when top 'layer' of blocks is
            // the same and index of directions is the same and block type is the same.
            ((var dropsToStartOfCycle, var heightAtStartOfCycle), (var dropsToEndOfCycle, var heightAtEndOfCycle)) = FindDropsBeforeRepeatingBlockState();
            if (blocksToDrop <= dropsToEndOfCycle)
            {
                return _droppedBlocksToHeightMap[blocksToDrop];
            }

            var heightGainDuringCycle = heightAtEndOfCycle - heightAtStartOfCycle;
            var cycleDuration = dropsToEndOfCycle - dropsToStartOfCycle;

            blocksToDrop -= dropsToStartOfCycle;

            var quotient = blocksToDrop / cycleDuration;
            var remainder = blocksToDrop % cycleDuration;

            var totalHeightAtEndOfCycleAndRemainder = DropBlocks(dropsToEndOfCycle + remainder);

            return totalHeightAtEndOfCycleAndRemainder + (quotient - 1) * heightGainDuringCycle;
        }

        private static string GetBlockDroppedStateHash(List<(int X, int Y)> topLayer, int blockSelectorIndex, int directionsIndex)
        {
            var sb = new StringBuilder();
            foreach(var coord in topLayer.OrderBy(coord => coord.X))
            {
                sb.Append(coord.Y);
                sb.Append(',');
            }
            sb.Append(blockSelectorIndex);
            sb.Append(',');
            sb.Append(directionsIndex);
            return sb.ToString();
        }

        private long DropBlocks(long numberOfBlocksToDrop)
        {
            var directionsIndex = 0;
            var blockSelectorIndex = 0;
            var existingBlocks = new HashSet<(int x, int y)>();
            var blocksDropped = 0L;
            var highestBlockHeight = 0;

            while (blocksDropped++ < numberOfBlocksToDrop)
            {
                var block = new Block(blockSelectorIndex, highestBlockHeight);
                blockSelectorIndex = (blockSelectorIndex + 1) % 5;

                var blockMoving = true;
                while (blockMoving)
                {
                    block.TryMoveHorizontal(Directions[directionsIndex], existingBlocks);
                    directionsIndex = (directionsIndex + 1) % Directions.Length;
                    blockMoving = block.TryMoveDown(existingBlocks);
                }
                existingBlocks.UnionWith(block.Coords);
                highestBlockHeight = Math.Max(highestBlockHeight, block.Coords.Max(coord => coord.Y));
            }

            return highestBlockHeight;
        }

        private ((long firstBlocksDropped, long firstBlockHeight), (long currentBlocksDropped, long currentBlockHeight)) FindDropsBeforeRepeatingBlockState()
        {
            var directionsIndex = 0;
            var blockSelectorIndex = 0;
            var existingBlocks = new HashSet<(int x, int y)>();
            var blocksDropped = 0L;
            var highestBlockHeight = 0;
            var blockDroppedStatesToBlocksDroppedAndHeightMap = new Dictionary<string, (long, long)>();
            _droppedBlocksToHeightMap = new Dictionary<long, long>();

            while (true)
            {
                var block = new Block(blockSelectorIndex, highestBlockHeight);
                blockSelectorIndex = (blockSelectorIndex + 1) % 5;

                var blockMoving = true;
                while (blockMoving)
                {
                    block.TryMoveHorizontal(Directions[directionsIndex], existingBlocks);
                    directionsIndex = (directionsIndex + 1) % Directions.Length;
                    blockMoving = block.TryMoveDown(existingBlocks);
                }
                blocksDropped++;
                existingBlocks.UnionWith(block.Coords);
                highestBlockHeight = Math.Max(highestBlockHeight, block.Coords.Max(coord => coord.Y));

                var blockDroppedStateHash = GetBlockDroppedStateHash(CalculateTopLayer(existingBlocks), blockSelectorIndex, directionsIndex);
                if (blockDroppedStatesToBlocksDroppedAndHeightMap.ContainsKey(blockDroppedStateHash))
                {
                    return (blockDroppedStatesToBlocksDroppedAndHeightMap[blockDroppedStateHash], (blocksDropped, highestBlockHeight));
                }
                blockDroppedStatesToBlocksDroppedAndHeightMap.Add(blockDroppedStateHash, (blocksDropped, highestBlockHeight));
                _droppedBlocksToHeightMap.Add(blocksDropped, highestBlockHeight);
            }
        }

        private List<(int X, int Y)> CalculateTopLayer(HashSet<(int x, int y)> existingBlocks)
        {
            var topLayer = new List<(int x, int y)>();
            for (var i = 0; i < 7; i++)
            {
                var highestY = existingBlocks.Any(coord => coord.x == i) ?
                    existingBlocks.Where(coord => coord.x == i).Max(coord => coord.y) :
                    0;
                topLayer.Add((i, highestY));
            }
            var minimumY = topLayer.Min(coord => coord.y);
            return topLayer.Select(coord => (coord.x, coord.y - minimumY)).ToList();
        }
    }
}
