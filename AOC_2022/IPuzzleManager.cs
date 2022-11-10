namespace AOC_2022
{
    public interface IPuzzleManager
    {
        public Task SolvePartOne();
        public Task SolvePartTwo();
        public Task SolveBothParts();
        public void Reset();
    }
}