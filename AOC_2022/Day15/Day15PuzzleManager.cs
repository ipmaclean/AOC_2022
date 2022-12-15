namespace AOC_2022.Day15
{
    public class Day15PuzzleManager : PuzzleManager
    {
        //new protected const string INPUT_FILE_NAME = "test.txt";
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
            //Console.WriteLine($"The solution to part one is '{FindBeaconlessCountInRow(10)}'.");
            Console.WriteLine($"The solution to part one is '{FindBeaconlessCountInRow(2000000)}'."); 
            return Task.CompletedTask;
        }

        private long FindBeaconlessCountInRow(long rowNumber)
        {
            var minMaxes = new HashSet<(long x, long y)>();
            foreach(var sensor in Sensors)
            {
                if (Math.Abs(rowNumber - sensor.Location.Y) > sensor.DistanceToNearestBeacon)
                {
                    continue;
                }
                var minX = sensor.Location.X - Math.Abs(sensor.DistanceToNearestBeacon - sensor.YDistanceToBeacon);
                var maxX = sensor.Location.X + Math.Abs(sensor.DistanceToNearestBeacon - sensor.YDistanceToBeacon);

                minMaxes.Add((minX, maxX));
            }
            var beaconlessCount = 0L;
            //var oldMinMaxCount = -1;
            //while (minMaxes.Count == oldMinMaxCount)
            //{
            //    var oldMinMaxCount = minMaxes.Count;
            //    var minMax = minMaxes.First();
            //    minMaxes.Remove(minMax);

            //    var overlaps = minMaxes.Where(x =>
            //        minMax.x >= x.x && minMax.x <= x.y ||
            //        minMax.y <= x.y && minMax.y <= x.y
            //        ).ToHashSet();

            //    overlaps.Add(minMax);
            //    (long x, long y) overlapRange = (overlaps.Min(x => x.x), overlaps.Max(x => x.y));
            //    var sensorsInRange = Sensors.Count(x => x.Location.X >= overlapRange.x && x.Location.Y <= overlapRange.y);
            //    beaconlessCount += (overlapRange.y - overlapRange.x) - sensorsInRange;

            //    foreach(var overlap in overlaps)
            //    {
            //        minMaxes.Remove(overlap);
            //    }
            //    minMaxes.Add(overlapRange);
            //}

            //foreach ()
            //{
            //    // get union of overlaps
            //}

            // Have a distinct minMax list
            // foreach minmax - get union of overlaps
            // for that overlap - get a union of overlaps - recusion until minMax isn't going down?

            // put that overlap into the distinct minMax list
            // move onto next value in minMax - continue until minMax is empty

            var minMaxesDistinct = new HashSet<(long x, long y)>();

            while (minMaxes.Any())
            {
                var minMax = minMaxes.First();
                minMaxes.Remove(minMax);

                GetOverLaps(minMax, minMaxes, minMaxesDistinct);
                
            }
            foreach(var minMax in minMaxesDistinct)
            {
                var beaconsInRange = Sensors.Where(x => x.NearestBeacon.X >= minMax.x && x.NearestBeacon.Y <= minMax.y && x.NearestBeacon.Y == rowNumber).Select(x => x.NearestBeacon).Distinct().Count();
                beaconlessCount += (minMax.y - minMax.x) - beaconsInRange;
            }

            //var localMin = long.MaxValue;
            //var localMax = long.MinValue;
            //foreach (var minMax in minMaxes)
            //{
            //    localMin = Math.Min(localMin, minMax.x);
            //    localMax = Math.Max(localMax, minMax.y);
            //}
            //return localMax - localMin;
            return beaconlessCount;
        }

        private void GetOverLaps((long x, long y) minMax, HashSet<(long x, long y)> minMaxes, HashSet<(long x, long y)> minMaxesDistinct)
        {
            var overlaps = minMaxes.Where(x =>
                    minMax.x >= x.x && minMax.x <= x.y ||
                    minMax.y <= x.y && minMax.y <= x.y
                    ).ToHashSet();
            if (!overlaps.Any())
            {
                minMaxesDistinct.Add(minMax);
                return;
            }
            overlaps.Add(minMax);
            foreach (var overlap in overlaps)
            {
                minMaxes.Remove(overlap);
            }
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
