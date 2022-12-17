using System.Diagnostics;
using System.Text;

namespace AOC_2022.Day17
{
    public class Day17PuzzleManager : PuzzleManager
    {
        //new protected const string INPUT_FILE_NAME = "test.txt";

        private int _directionsIndex = 0;
        private int _blockSelectorIndex = 0;
        private Dictionary<string, (long, long)> _blockDroppedStatesToBlocksDroppedMap = new Dictionary<string, (long, long)>();

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
            _directionsIndex = 0;
            _blockSelectorIndex = 0;
            Console.WriteLine($"The solution to part one is '{DropBlocks(2022)}'.");
            return Task.CompletedTask;
        }

        public override Task SolvePartTwo()
        {
            _blockDroppedStatesToBlocksDroppedMap = new Dictionary<string, (long, long)>();
            // The pattern of dropped blocks will repeat.
            // When top 'layer' of blocks is the same and index of directions is the same and block type is the same

            var totalBlocksToDrop = 1_000_000_000_000L;

            ((var dropsToStartOfCycle, var heightAtStartOfCycle), (var dropsToEndOfCycle, var heightAtEndOfCycle))= FindDropsBeforeRepeatingBlockState();

            var heightGainDuringCycle = heightAtEndOfCycle - heightAtStartOfCycle;
            var cycleDuration = dropsToEndOfCycle - dropsToStartOfCycle;

            totalBlocksToDrop -= dropsToStartOfCycle;

            var quotient = totalBlocksToDrop / cycleDuration;
            var remainder = totalBlocksToDrop % cycleDuration;

            var totalHeightAtEndOfCycleAndRemainder = (long)DropBlocks((int)dropsToEndOfCycle + (int)remainder);

            var solution = totalHeightAtEndOfCycleAndRemainder + (quotient - 1) * heightGainDuringCycle;
            Console.WriteLine($"The solution to part two is '{solution}'.");
            return Task.CompletedTask;
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

        private int DropBlocks(int numberOfBlocksToDrop)
        {
            _directionsIndex = 0;
            _blockSelectorIndex = 0;
            var existingBlocks = new HashSet<(int x, int y)>();
            var blocksDropped = 0;
            var highestBlockHeight = 0;

            while (blocksDropped++ < numberOfBlocksToDrop)
            {
                var block = new Block(_blockSelectorIndex, highestBlockHeight);
                _blockSelectorIndex = (_blockSelectorIndex + 1) % 5;

                var blockMoving = true;
                while (blockMoving)
                {
                    block.TryMoveHorizontal(Directions[_directionsIndex], existingBlocks);
                    _directionsIndex = (_directionsIndex + 1) % Directions.Length;
                    blockMoving = block.TryMoveDown(existingBlocks);
                }
                existingBlocks.UnionWith(block.Coords);
                highestBlockHeight = Math.Max(highestBlockHeight, block.Coords.Max(coord => coord.Y));
            }

            return highestBlockHeight;
        }

        private ((long firstBlocksDropped, long firstBlockHeight), (long currentBlocksDropped, long currentBlockHeight)) FindDropsBeforeRepeatingBlockState()
        {
            _directionsIndex = 0;
            _blockSelectorIndex = 0;
            var existingBlocks = new HashSet<(int x, int y)>();
            var blocksDropped = 0L;
            var highestBlockHeight = 0;

            while (true)
            {
                var block = new Block(_blockSelectorIndex, highestBlockHeight);
                _blockSelectorIndex = (_blockSelectorIndex + 1) % 5;

                var blockMoving = true;
                while (blockMoving)
                {
                    block.TryMoveHorizontal(Directions[_directionsIndex], existingBlocks);
                    _directionsIndex = (_directionsIndex + 1) % Directions.Length;
                    blockMoving = block.TryMoveDown(existingBlocks);
                }
                blocksDropped++;
                existingBlocks.UnionWith(block.Coords);
                highestBlockHeight = Math.Max(highestBlockHeight, block.Coords.Max(coord => coord.Y));

                var blockDroppedStateHash = GetBlockDroppedStateHash(CalculateTopLayer(existingBlocks), _blockSelectorIndex, _directionsIndex);
                if (_blockDroppedStatesToBlocksDroppedMap.ContainsKey(blockDroppedStateHash))
                {
                    return (_blockDroppedStatesToBlocksDroppedMap[blockDroppedStateHash], (blocksDropped, (long)highestBlockHeight));
                }
                _blockDroppedStatesToBlocksDroppedMap.Add(blockDroppedStateHash, (blocksDropped, (long)highestBlockHeight));
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
