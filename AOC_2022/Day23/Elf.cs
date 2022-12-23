namespace AOC_2022.Day23
{
    public class Elf
    {
        public Elf((int X, int Y) coords)
        {
            Coords = coords;
            Proposal = null;
        }

        public (int X, int Y) Coords { get; set; }
        public (int X, int Y)? Proposal { get; set; }
    }
}
