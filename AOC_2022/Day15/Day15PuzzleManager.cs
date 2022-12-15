namespace AOC_2022.Day15
{
    public class Day15PuzzleManager : PuzzleManager
    {
        new protected const string INPUT_FILE_NAME = "test.txt";
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
            Console.WriteLine($"The solution to part one is '{FindBeaconlessCountInRow(10)}'.");
            //Console.WriteLine($"The solution to part one is '{FindBeaconlessCountInRow(2000000)}'."); 
            return Task.CompletedTask;
        }

        private long FindBeaconlessCountInRow(long rowNumber)
        {
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
            var beaconlessCount = 0L;

            var minMaxesDistinct = new HashSet<(long min, long max)>();

            while (minMaxes.Any())
            {
                var minMax = minMaxes.First();
                minMaxes.Remove(minMax);

                GetOverLaps(minMax, minMaxes, minMaxesDistinct);
                
            }
            foreach(var minMax in minMaxesDistinct)
            {
                var beaconsInRange = Sensors.Where(x => x.NearestBeacon.X >= minMax.min && x.NearestBeacon.Y <= minMax.max && x.NearestBeacon.Y == rowNumber).Select(x => x.NearestBeacon).Distinct().Count();
                beaconlessCount += (minMax.max - minMax.min + 1) - beaconsInRange;
            }

            return beaconlessCount;
        }

        private void GetOverLaps((long x, long y) minMax, HashSet<(long x, long y)> minMaxes, HashSet<(long x, long y)> minMaxesDistinct)
        {
            var overlaps = minMaxes.Where(x =>
                    !(minMax.x > x.y ||
                    minMax.y < x.x)
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
            (long x, long y) overlapRange = (overlaps.Min(x => x.x), overlaps.Max(x => x.y));
            GetOverLaps(overlapRange, minMaxes, minMaxesDistinct);
        }

        public override Task SolvePartTwo()
        {

            var solution = 0;
            Console.WriteLine($"The solution to part two is '{solution}'.");
            return Task.CompletedTask;
        }
    }
}
