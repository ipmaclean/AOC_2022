namespace AOC_2022.Day4
{
    public class ElfTeam
    {
        public (int LowerBound, int UpperBound) ElfOne { get; set; }
        public (int LowerBound, int UpperBound) ElfTwo { get; set; }

        public ElfTeam((int LowerBound, int UpperBound) elfOne, (int LowerBound, int UpperBound) elfTwo)
        {
            ElfOne = elfOne;
            ElfTwo = elfTwo;
        }

        public bool IsFullyContained()
        {
            if (ElfOne.LowerBound >= ElfTwo.LowerBound && ElfOne.UpperBound <= ElfTwo.UpperBound ||
                ElfTwo.LowerBound >= ElfOne.LowerBound && ElfTwo.UpperBound <= ElfOne.UpperBound)
            {
                return true;
            }
            return false;
        }

        public bool HasOverlap()
        {
            if (ElfOne.LowerBound < ElfTwo.LowerBound && ElfOne.UpperBound < ElfTwo.LowerBound ||
                ElfTwo.LowerBound < ElfOne.LowerBound && ElfTwo.UpperBound < ElfOne.LowerBound)
            {
                return false;
            }
            return true;
        }
    }
}
