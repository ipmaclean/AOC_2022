namespace AOC_2022.Day15
{
    public class Sensor
    {
        public Sensor((long X, long Y) location, (long X, long Y) nearestBeacon)
        {
            Location = location;
            NearestBeacon = nearestBeacon;
        }

        public (long X, long Y) Location { get; private set; }
        public (long X, long Y) NearestBeacon { get; private set; }
        public long XDistanceToBeacon => Math.Abs(Location.X - NearestBeacon.X);
        public long YDistanceToBeacon => Math.Abs(Location.Y - NearestBeacon.Y);
        public long DistanceToNearestBeacon => XDistanceToBeacon + YDistanceToBeacon;
    }
}
