namespace AOC_2022.Day4
{
    public class ElfTeam
    {
        public List<(int LowerBound, int UpperBound)> Elves { get; set; }

        public ElfTeam(List<(int LowerBound, int UpperBound)> elves)
        {
            Elves = elves;
        }

        public bool IsFullyContained()
        {
            try
            {
                if (Elves[0] == Elves[1])
                {
                    return true;
                }
                var elfWithLowestLowerBound = Elves.First(x => x.LowerBound == Elves.Min(x => x.LowerBound));
                var otherElf = Elves.First(x => x != elfWithLowestLowerBound);

                if (elfWithLowestLowerBound.UpperBound >= otherElf.UpperBound ||
                    elfWithLowestLowerBound.LowerBound == otherElf.LowerBound && elfWithLowestLowerBound.UpperBound <= otherElf.UpperBound)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                throw;
            }
        }

        public bool HasOverlap()
        {
            try
            {
                if (Elves[0].LowerBound < Elves[1].LowerBound && Elves[0].UpperBound < Elves[1].LowerBound ||
                    Elves[1].LowerBound < Elves[0].LowerBound && Elves[1].UpperBound < Elves[0].LowerBound)
                {
                    return false;
                }
                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}
