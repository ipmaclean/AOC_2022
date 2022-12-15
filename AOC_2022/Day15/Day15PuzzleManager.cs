namespace AOC_2022.Day15
{
    public class Day15PuzzleManager : PuzzleManager
    {
        public Day15PuzzleManager()
        {
            var inputHelper = new Day15InputHelper(INPUT_FILE_NAME);
            Sensors = inputHelper.Parse();
        }

        public List<Sensor> Sensors { get; set; }

        public override Task SolveBothParts()
        {
            SolvePartOne();
            Console.WriteLine();
            SolvePartTwo();
            return Task.CompletedTask;
        }

        public override Task SolvePartOne()
        {
            // Find beaconless count in row
            Console.WriteLine($"The solution to part one is '{Solve(2_000_000, isPartOne: true)}'."); 
            return Task.CompletedTask;
        }

        private long Solve(long rowNumber, bool isPartOne)
        {
            // Get the min and max x coord covered by each sensor
            var minMaxes = new HashSet<(long min, long max)>();
            foreach(var sensor in Sensors)
            {
                if (Math.Abs(rowNumber - sensor.Location.Y) >= sensor.DistanceToNearestBeacon)
                {
                    continue;
                }
                var minX = sensor.Location.X - (sensor.DistanceToNearestBeacon - Math.Abs(rowNumber - sensor.Location.Y));
                var maxX = sensor.Location.X + (sensor.DistanceToNearestBeacon - Math.Abs(rowNumber - sensor.Location.Y));

                minMaxes.Add((minX, maxX));
            }

            // Merge any overlapping min and maxes
            var minMaxesDistinct = new HashSet<(long min, long max)>();
            while (minMaxes.Any())
            {
                var minMax = minMaxes.First();
                minMaxes.Remove(minMax);

                GetOverLaps(minMax, minMaxes, minMaxesDistinct);
            }
            if (minMaxesDistinct.Count > 1 && !isPartOne)
            {
                return minMaxesDistinct.Min(x => x.max) + 1;
            }
            else if (!isPartOne)
            {
                return -1;
            }

            // Remove any beacons within the min and max ranges.
            var beaconlessCount = 0L;
            foreach (var minMax in minMaxesDistinct)
            {
                var beaconsInRange = Sensors
                    .Where(x => x.NearestBeacon.X >= minMax.min && x.NearestBeacon.Y <= minMax.max && x.NearestBeacon.Y == rowNumber)
                    .Select(x => x.NearestBeacon).Distinct().Count();
                beaconlessCount += (minMax.max - minMax.min + 1) - beaconsInRange;
            }

            return beaconlessCount;
        }

        private void GetOverLaps((long min, long max) minMax, HashSet<(long min, long max)> minMaxes, HashSet<(long x, long y)> minMaxesDistinct)
        {
            var overlaps = minMaxes.Where(x =>
                    !(minMax.min - 1 > x.max || minMax.max + 1 < x.min)
                    ).ToHashSet();
            if (!overlaps.Any())
            {
                minMaxesDistinct.Add(minMax);
                return;
            }
            foreach (var overlap in overlaps)
            {
                minMaxes.Remove(overlap);
            }
            overlaps.Add(minMax);
            (long min, long max) overlapRange = (overlaps.Min(x => x.min), overlaps.Max(x => x.max));
            GetOverLaps(overlapRange, minMaxes, minMaxesDistinct);
        }

        public override Task SolvePartTwo()
        {
            // Find only non filled space in the range.
            var solution = 0L;
            for (var i = 0; i < 4_000_000; i++)
            {
                var beaconXCoord = Solve(i, isPartOne: false);
                if (beaconXCoord > 0)
                {
                    solution = beaconXCoord * 4_000_000 + i;
                    break;
                }
            }
            Console.WriteLine($"The solution to part two is '{solution}'.");
            return Task.CompletedTask;
        }
    }
}
