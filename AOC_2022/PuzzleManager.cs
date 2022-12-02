namespace AOC_2022
{
    public abstract class PuzzleManager : IPuzzleManager
    {
        protected const string INPUT_FILE_NAME = "input.txt";

        public virtual void Reset()
        {
        }
        public abstract Task SolveBothParts();
        public abstract Task SolvePartOne();
        public abstract Task SolvePartTwo();
    }
}
