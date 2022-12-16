namespace AOC_2022.Day16
{
    public class Day16PuzzleManager : PuzzleManager
    {
        //new protected const string INPUT_FILE_NAME = "test.txt";

        private Day16InputHelper _inputHelper;
        public Day16PuzzleManager()
        {
            _inputHelper = new Day16InputHelper(INPUT_FILE_NAME);
        }
        public override Task SolveBothParts()
        {
            SolvePartOne();
            Console.WriteLine();
            SolvePartTwo();
            return Task.CompletedTask;
        }

        public override Task SolvePartOne()
        {
            //  Ideas
            //      Map out shortest distances between (>0) valves and from start
            //      Pressure released = value * time remaining to avoid tracking things
            //      Go from highest pressure released over time from current position to lowest?
            //          That does not give the optimal solution unfortunately

            var valves = _inputHelper.Parse();

            foreach (var valve in valves.Where(x => x.Name == "AA" || x.Rate > 0))
            {
                FindShortestDistances(valve, valves);
                ResetShortestPaths(valves);
            }

            var solution = 0;
            var firstValve = valves.First(x => x.Name == "AA");
            var workingValves = valves.Where(x => x.Rate > 0).ToList();

            var nextSteps = new Queue<(Valve valve, List<string> path, int timeRemaining, int totalPressureReleased)>();
            nextSteps.Enqueue((firstValve, new List<string>(), 30, 0));
            while (nextSteps.TryDequeue(out var currentStep))
            {
                (var currentValve, var currentPath, var timeRemaining, var totalPressureReleased) = currentStep;

                // Optimisation - if all remaining closed valve rates * time isn't better than best current, discard this path
                if (totalPressureReleased + workingValves.Where(x => !currentPath.Contains(x.Name)).Sum(x => x.Rate * timeRemaining) < solution)
                {
                    continue;
                }

                foreach (var nextValve in workingValves.Where(x => !currentPath.Contains(x.Name)))
                {
                    // Time's up or Visited every working valve
                    if (timeRemaining - (currentValve.Distances[nextValve] + 1) <= 0)
                    {
                        solution = Math.Max(
                            solution,
                            totalPressureReleased
                            );
                        continue;
                    }

                    var newTotalPressureReleased = 
                        totalPressureReleased + (timeRemaining - (currentValve.Distances[nextValve] + 1)) * nextValve.Rate;

                    if (currentPath.Count + 1 == workingValves.Count)
                    {
                        solution = Math.Max(
                            solution,
                            newTotalPressureReleased
                            );
                        continue;
                    }

                    var newPath = new List<string>(currentPath);
                    newPath.Add(nextValve.Name);

                    nextSteps.Enqueue((
                        nextValve,
                        newPath,
                        timeRemaining - (currentValve.Distances[nextValve] + 1),
                        newTotalPressureReleased
                        ));
                }
            }

            Console.WriteLine($"The solution to part one is '{solution}'.");
            return Task.CompletedTask;
        }

        public void FindShortestDistances(Valve startValve, List<Valve> valves)
        {
            var valvesToVisit = new Queue<Valve>();
            startValve.ShortestPath = 0;
            valvesToVisit.Enqueue(startValve);

            while (valvesToVisit.TryDequeue(out var currentValve))
            {
                foreach (var valveName in currentValve.ConnectedValves)
                {
                    var valve = valves.First(x => x.Name == valveName);

                    if (valve.ShortestPath < 0)
                    {
                        valve.ShortestPath = currentValve.ShortestPath + 1;
                        startValve.Distances.Add(valve, valve.ShortestPath);
                        valvesToVisit.Enqueue(valve);
                    }
                }
            }
        }

        public void ResetShortestPaths(List<Valve> valves)
        {
            foreach (var valve in valves)
            {
                valve.ShortestPath = -1;
            }
        }

        public override Task SolvePartTwo()
        {
            var solution = 0;
            Console.WriteLine($"The solution to part two is '{solution}'.");
            return Task.CompletedTask;
        }
    }
}
