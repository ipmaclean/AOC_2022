using System.Text;

namespace AOC_2022.Day16
{
    public class Day16PuzzleManager : PuzzleManager
    {
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
            var valves = _inputHelper.Parse();

            foreach (var valve in valves.Where(x => x.Name == "AA" || x.Rate > 0))
            {
                FindShortestDistances(valve, valves);
                ResetShortestPaths(valves);
            }

            var solution = 0;
            var firstValve = valves.First(x => x.Name == "AA");
            var workingValves = valves.Where(x => x.Rate > 0).ToList();

            var nextSteps = new Queue<Day16BreadthFirstSearchState>();
            nextSteps.Enqueue(new Day16BreadthFirstSearchState(firstValve, new List<string>(), 30, 0));
            while (nextSteps.TryDequeue(out var currentState))
            {
                // Optimisation - if all remaining closed valve rates * time isn't better than best current, discard this path
                if (currentState.TotalPressureReleased + workingValves.Where(x => !currentState.Path.Contains(x.Name)).Sum(x => x.Rate * currentState.TimeRemaining) < solution)
                {
                    continue;
                }

                foreach (var nextValve in workingValves.Where(x => !currentState.Path.Contains(x.Name)))
                {
                    // You have no time to visit the next valve - we have a solution
                    if (currentState.TimeRemaining - (currentState.Valve.Distances[nextValve] + 1) <= 0)
                    {
                        solution = Math.Max(
                            solution,
                            currentState.TotalPressureReleased
                            );
                        continue;
                    }

                    var newTotalPressureReleased =
                        currentState.TotalPressureReleased + (currentState.TimeRemaining - (currentState.Valve.Distances[nextValve] + 1)) * nextValve.Rate;

                    // Visited every working valve - we have a solution
                    if (currentState.Path.Count + 1 == workingValves.Count)
                    {
                        solution = Math.Max(
                            solution,
                            newTotalPressureReleased
                            );
                        continue;
                    }

                    var newPath = new List<string>(currentState.Path);
                    newPath.Add(nextValve.Name);

                    nextSteps.Enqueue(new Day16BreadthFirstSearchState(
                        nextValve,
                        newPath,
                        currentState.TimeRemaining - (currentState.Valve.Distances[nextValve] + 1),
                        newTotalPressureReleased
                        ));
                }
            }

            Console.WriteLine($"The solution to part one is '{solution}'.");
            return Task.CompletedTask;
        }

        public override Task SolvePartTwo()
        {
            var valves = _inputHelper.Parse();

            foreach (var valve in valves.Where(x => x.Name == "AA" || x.Rate > 0))
            {
                FindShortestDistances(valve, valves);
                ResetShortestPaths(valves);
            }

            var solution = 0;
            var firstValve = valves.First(x => x.Name == "AA");
            var workingValves = valves.Where(x => x.Rate > 0).ToList();
            var visitedStates = new HashSet<string>();

            // We will use the lowest time remaining at the priority in the queue to avoid
            // the queue being too long at any point in time.
            var nextSteps = new PriorityQueue<Day16BreadthFirstSearchState[], int>();
            nextSteps.Enqueue(
                new Day16BreadthFirstSearchState[] {
                    new Day16BreadthFirstSearchState(firstValve, new List<string>(), 26, 0),
                    new Day16BreadthFirstSearchState(firstValve, new List<string>(), 26, 0) },
                26);
            while (nextSteps.TryDequeue(out var currentState, out int timeElapsed))
            {
                var unvisitedWorkingValves = workingValves.Where(x => !currentState[0].Path.Contains(x.Name) && !currentState[1].Path.Contains(x.Name)).ToList();

                // Move whoever has the most time remaining - you or the elephant
                var movingState = currentState.First(x => x.TimeRemaining == currentState.Max(x => x.TimeRemaining));
                var otherState = currentState.First(x => x != movingState);

                // Optimisation - if all remaining closed valve rates * time isn't better than best current, discard this path
                if (currentState[0].TotalPressureReleased + currentState[1].TotalPressureReleased + workingValves.Where(x => !currentState[0].Path.Contains(x.Name) && !currentState[1].Path.Contains(x.Name)).Sum(x => x.Rate * movingState.TimeRemaining) < solution)
                {
                    continue;
                }

                var addedCurrentState = false;
                foreach (var nextValve in unvisitedWorkingValves)
                {
                    Day16BreadthFirstSearchState[] nextDualState;
                    // The moving valve closer has no time to visit the next valve
                    if (movingState.TimeRemaining - (movingState.Valve.Distances[nextValve] + 1) <= 0)
                    {
                        // Both you and the elephant have no time to visit anywhere - we have a solution.
                        if (otherState.TimeRemaining == 0)
                        {
                            solution = Math.Max(
                                solution,
                                movingState.TotalPressureReleased + otherState.TotalPressureReleased
                                );
                            continue;
                        }
                        // To avoid adding the same thing to the queue multiple times.
                        if (addedCurrentState)
                        {
                            continue;
                        }
                        addedCurrentState = true;

                        // Queue the same state, but with no time remaining.
                        nextDualState = new Day16BreadthFirstSearchState[]
                        {
                            new Day16BreadthFirstSearchState(movingState.Valve, new List<string>(movingState.Path), 0, movingState.TotalPressureReleased),
                            otherState
                        };
                        nextSteps.Enqueue(nextDualState, otherState.TimeRemaining);
                        continue;
                    }

                    var newTotalPressureReleased =
                        movingState.TotalPressureReleased + (movingState.TimeRemaining - (movingState.Valve.Distances[nextValve] + 1)) * nextValve.Rate;

                    // Visited every working valve - we have a solution.
                    if (movingState.Path.Count + otherState.Path.Count + 1 == workingValves.Count)
                    {
                        solution = Math.Max(
                            solution,
                            newTotalPressureReleased + otherState.TotalPressureReleased
                            );
                        continue;
                    }

                    var newPath = new List<string>(movingState.Path);
                    newPath.Add(nextValve.Name);

                    var lowestTimeRemaining = Math.Min(movingState.TimeRemaining - (movingState.Valve.Distances[nextValve] + 1), otherState.TimeRemaining);

                    nextDualState = new Day16BreadthFirstSearchState[]
                    {
                        new Day16BreadthFirstSearchState(nextValve, newPath, movingState.TimeRemaining - (movingState.Valve.Distances[nextValve] + 1), newTotalPressureReleased),
                        otherState
                    };
                    var nextDualStateHash = GetDualStateHash(nextDualState);
                    if (visitedStates.Contains(nextDualStateHash))
                    {
                        // Saves about 10s on my input but at the cost of 1.4GB of memory!
                        continue;
                    }
                    visitedStates.Add(nextDualStateHash);
                    nextSteps.Enqueue(nextDualState, lowestTimeRemaining);
                }
            }
            Console.WriteLine($"The solution to part two is '{solution}'.");
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

        private string GetDualStateHash(Day16BreadthFirstSearchState[] dualStates)
        {
            Day16BreadthFirstSearchState firstState;
            if (dualStates.All(x => x.Path.Count == 0))
            {
                firstState = dualStates[0];
            }
            else if (dualStates.Any(x => x.Path.Count == 0))
            {
                firstState = dualStates.First(x => x.Path.Count > 0);
            }
            else
            {
                firstState = dualStates.OrderBy(x => x.Path[0]).First();
            }

            var secondState = dualStates.First(x => x != firstState);
            var sb = new StringBuilder();
            var delimiter = "";
            foreach (var valveName in firstState.Path)
            {
                sb.Append(delimiter);
                sb.Append(valveName);
                delimiter = ",";
            }
            sb.Append(":");
            delimiter = "";
            foreach (var valveName in secondState.Path)
            {
                sb.Append(delimiter);
                sb.Append(valveName);
                delimiter = ",";
            }
            return sb.ToString();
        }
    }
}
