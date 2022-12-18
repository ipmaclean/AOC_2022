namespace AOC_2022.Day16
{
    internal class Day16BreadthFirstSearchState
    {
        public Valve Valve { get; set; }
        public List<string> Path { get; set; }
        public int TimeRemaining { get; set; }
        public int TotalPressureReleased { get; set; }

        public Day16BreadthFirstSearchState(Valve currentValve, List<string> path, int timeRemaining, int totalPressureReleased)
        {
            Valve = currentValve;
            Path = path;
            TimeRemaining = timeRemaining;
            TotalPressureReleased = totalPressureReleased;
        }

        public Day16BreadthFirstSearchState(Day16BreadthFirstSearchState state)
        {
            Valve = state.Valve;
            Path = new List<string>(state.Path);
            TimeRemaining = state.TimeRemaining;
            TotalPressureReleased = state.TotalPressureReleased;
        }
    }
}