namespace AOC_2022.Day19
{
    public class Blueprint
    {
        public Blueprint(
            int name,
            int oreCost, 
            int clayCost,
            (int Ore, int Clay) obsidianCost,
            (int Ore, int Obsidian) geodeCost
            )
        {
            Name = name;
            OreMachineCost = oreCost;
            ClayMachineCost = clayCost;
            ObsidianMachineCost = obsidianCost;
            GeodeMachineCost = geodeCost;
        }

        public int Name { get; init; }
        public int OreMachineCost { get; init; }
        public int ClayMachineCost { get; init; }
        public (int Ore, int Clay) ObsidianMachineCost { get; init; }
        public (int Ore, int Obsidian) GeodeMachineCost { get; init; }
    }
}
