namespace AOC_2022.Day19
{
    public struct TimeState
    {
        public int Minute { get; init; }
        public (int Material, int Machine) Ore { get; init; }
        public (int Material, int Machine) Clay { get; init; }
        public (int Material, int Machine) Obsidian { get; init; }
        public (int Material, int Machine) Geode { get; init; }

        public TimeState(
            int minute,
            (int Material, int Machine) ore,
            (int Material, int Machine) clay,
            (int Material, int Machine) obsidian,
            (int Material, int Machine) geode
            )
        {
            Minute = minute;
            Ore = ore;
            Clay = clay;
            Obsidian = obsidian;
            Geode = geode;
        }
    }
}
