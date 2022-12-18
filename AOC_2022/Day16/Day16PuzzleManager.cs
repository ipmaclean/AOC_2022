using System.Diagnostics;

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
            var sw = new Stopwatch();
            sw.Start();
            SolvePartOne();
            sw.Stop();
            Console.WriteLine($"Part One: {sw.ElapsedMilliseconds}ms.");
            Console.WriteLine();
            sw.Restart();
            SolvePartTwo();
            sw.Stop();
            Console.WriteLine($"Part Two: {sw.ElapsedMilliseconds}ms.");
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
                    // Time's up
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

                    // Visited every working valve
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
            var valves = _inputHelper.Parse();

            foreach (var valve in valves.Where(x => x.Name == "AA" || x.Rate > 0))
            {
                FindShortestDistances(valve, valves);
                ResetShortestPaths(valves);
            }

            var solution = 0;
            var firstValve = valves.First(x => x.Name == "AA");
            var workingValves = valves.Where(x => x.Rate > 0).ToList();

            // Need to use time elapsed for this instead of time left
            var nextSteps = new PriorityQueue<Day16BreadthFirstSearchState[], int>();
            nextSteps.Enqueue(
                new Day16BreadthFirstSearchState[] {
                    new Day16BreadthFirstSearchState(firstValve, new List<string>(), 26, 0),
                    new Day16BreadthFirstSearchState(firstValve, new List<string>(), 26, 0) },
                0);
            while (nextSteps.TryDequeue(out var currentState, out int timeElapsed))
            {
                var timeRemaining = 26 - timeElapsed;

                // Optimisation - if all remaining closed valve rates * time isn't better than best current, discard this path
                if (currentState[0].TotalPressureReleased + currentState[1].TotalPressureReleased + workingValves.Where(x => !currentState[0].Path.Contains(x.Name) && !currentState[1].Path.Contains(x.Name)).Sum(x => x.Rate * timeRemaining) < solution)
                {
                    continue;
                }

                var unvisitedWorkingValves = workingValves.Where(x => !currentState[0].Path.Contains(x.Name) && !currentState[1].Path.Contains(x.Name)).ToList();

                if (currentState[0].TimeRemaining == currentState[1].TimeRemaining && unvisitedWorkingValves.Count > 1)
                {
                    for (var i = 0; i < unvisitedWorkingValves.Count; i++)
                    {
                        for (var j = 0; j < unvisitedWorkingValves.Count; j++)
                        {
                            if (i == j)
                            {
                                continue;
                            }
                            // Time's up - might be an issue here if one runs out but other's ok - my head is not working well enough to figure it out

                            var currentStateOneOutOfTime = currentState[0].TimeRemaining - (currentState[0].Valve.Distances[unvisitedWorkingValves[i]] + 1) <= 0;
                            var currentStateTwoOutOfTime = currentState[1].TimeRemaining - (currentState[1].Valve.Distances[unvisitedWorkingValves[j]] + 1) <= 0;

                            var currentStateOneNewTotalPressureReleased = currentStateOneOutOfTime ?
                                currentState[0].TotalPressureReleased :
                                currentState[0].TotalPressureReleased + (currentState[0].TimeRemaining - (currentState[0].Valve.Distances[unvisitedWorkingValves[i]] + 1)) * unvisitedWorkingValves[i].Rate;
                            var currentStateTwoNewTotalPressureReleased = currentStateTwoOutOfTime ?
                                currentState[1].TotalPressureReleased :
                                currentState[1].TotalPressureReleased + (currentState[1].TimeRemaining - (currentState[1].Valve.Distances[unvisitedWorkingValves[j]] + 1)) * unvisitedWorkingValves[j].Rate;


                            if (currentStateOneOutOfTime && currentStateTwoOutOfTime)
                            {
                                solution = Math.Max(
                                    solution,
                                    currentState[0].TotalPressureReleased + currentState[1].TotalPressureReleased
                                    );
                                continue;
                            }

                            var newTotalPressuresReleased = new int[] {
                                currentStateOneNewTotalPressureReleased,
                                currentStateTwoNewTotalPressureReleased
                            };


                            var valvesToVisit = currentStateOneOutOfTime || currentStateTwoOutOfTime ? 1 : 2;
                            // Visited every working valve
                            if (currentState[0].Path.Count + currentState[1].Path.Count + valvesToVisit == workingValves.Count)
                            {
                                solution = Math.Max(
                                    solution,
                                    newTotalPressuresReleased[0] + newTotalPressuresReleased[1]
                                    );
                                continue;
                            }

                            var newPaths = new List<string>[] {
                                new List<string>(currentState[0].Path),
                                new List<string>(currentState[1].Path),
                            };

                            if (!currentStateOneOutOfTime)
                            {
                                newPaths[0].Add(unvisitedWorkingValves[i].Name);
                            }
                            if (!currentStateTwoOutOfTime)
                            {
                                newPaths[1].Add(unvisitedWorkingValves[j].Name);
                            }

                            var timeRemainingStateOne = currentStateOneOutOfTime ?
                                currentState[0].TimeRemaining :
                                currentState[0].TimeRemaining - (currentState[0].Valve.Distances[unvisitedWorkingValves[i]] + 1);
                            var timeRemainingStateTwo = currentStateTwoOutOfTime ?
                                currentState[1].TimeRemaining :
                                currentState[1].TimeRemaining - (currentState[1].Valve.Distances[unvisitedWorkingValves[j]] + 1);

                            var lowestTimeElapsed = 26 - Math.Max(
                                timeRemainingStateOne,
                                timeRemainingStateTwo
                                );

                            var nextValveStateOne = currentStateOneOutOfTime ?
                                currentState[0].Valve :
                                unvisitedWorkingValves[i];
                            var nextValveStateTwo = currentStateTwoOutOfTime ?
                                currentState[1].Valve :
                                unvisitedWorkingValves[j];

                            nextSteps.Enqueue(
                                new Day16BreadthFirstSearchState[] {
                                new Day16BreadthFirstSearchState(nextValveStateOne, newPaths[0], timeRemainingStateOne, newTotalPressuresReleased[0]),
                                new Day16BreadthFirstSearchState(nextValveStateTwo, newPaths[1], timeRemainingStateTwo, newTotalPressuresReleased[1]) },
                                lowestTimeElapsed);
                        }
                    }
                }
                else if (currentState[0].TimeRemaining == currentState[1].TimeRemaining && unvisitedWorkingValves.Count == 1)
                {
                    var unvisitedValve = unvisitedWorkingValves[0];
                    var nearestState = currentState.First(x => x.Valve.Distances[unvisitedValve] == currentState.Min(x => x.Valve.Distances[unvisitedValve]));
                    var otherState = currentState.First(x => x != nearestState);

                    // Time's up
                    if (nearestState.TimeRemaining - (nearestState.Valve.Distances[unvisitedValve] + 1) <= 0)
                    {
                        solution = Math.Max(
                            solution,
                            nearestState.TotalPressureReleased + otherState.TotalPressureReleased
                            );
                        continue;
                    }

                    var newTotalPressureReleased =
                        nearestState.TotalPressureReleased + (nearestState.TimeRemaining - (nearestState.Valve.Distances[unvisitedValve] + 1)) * unvisitedValve.Rate;

                    // Visited every working valve
                    if (nearestState.Path.Count + otherState.Path.Count + 1 == workingValves.Count)
                    {
                        solution = Math.Max(
                            solution,
                            newTotalPressureReleased + otherState.TotalPressureReleased
                            );
                        continue;
                    }

                    var newPath = new List<string>(nearestState.Path);
                    newPath.Add(unvisitedValve.Name);

                    var lowestTimeElapsed = 26 - Math.Max(nearestState.TimeRemaining - (nearestState.Valve.Distances[unvisitedValve] + 1), otherState.TimeRemaining);
                    nextSteps.Enqueue(
                        new Day16BreadthFirstSearchState[] {
                                new Day16BreadthFirstSearchState(unvisitedValve, newPath, nearestState.TimeRemaining - (nearestState.Valve.Distances[unvisitedValve] + 1), newTotalPressureReleased),
                                otherState },
                        lowestTimeElapsed);
                }
                else
                {
                    var readyState = currentState.First(x => x.TimeRemaining == currentState.Max(x => x.TimeRemaining));
                    var unreadyState = currentState.First(x => x != readyState);

                    foreach (var nextValve in unvisitedWorkingValves)
                    {
                        // Time's up
                        if (readyState.TimeRemaining - (readyState.Valve.Distances[nextValve] + 1) <= 0)
                        {
                            solution = Math.Max(
                                solution,
                                readyState.TotalPressureReleased + unreadyState.TotalPressureReleased
                                );
                            continue;
                        }

                        var newTotalPressureReleased =
                            readyState.TotalPressureReleased + (readyState.TimeRemaining - (readyState.Valve.Distances[nextValve] + 1)) * nextValve.Rate;

                        // Visited every working valve
                        if (readyState.Path.Count + unreadyState.Path.Count + 1 == workingValves.Count)
                        {
                            solution = Math.Max(
                                solution,
                                newTotalPressureReleased + unreadyState.TotalPressureReleased
                                );
                            continue;
                        }

                        var newPath = new List<string>(readyState.Path);
                        newPath.Add(nextValve.Name);

                        var lowestTimeElapsed = 26 - Math.Max(readyState.TimeRemaining - (readyState.Valve.Distances[nextValve] + 1), unreadyState.TimeRemaining);
                        nextSteps.Enqueue(
                            new Day16BreadthFirstSearchState[] {
                                new Day16BreadthFirstSearchState(nextValve, newPath, readyState.TimeRemaining - (readyState.Valve.Distances[nextValve] + 1), newTotalPressureReleased),
                                unreadyState },
                            lowestTimeElapsed);
                    }
                }
            }
            Console.WriteLine($"The solution to part two is '{solution}'.");
            return Task.CompletedTask;
        }
    }
}
