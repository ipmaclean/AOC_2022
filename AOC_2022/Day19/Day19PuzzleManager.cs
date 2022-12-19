using System.Diagnostics;

namespace AOC_2022.Day19
{
    public class Day19PuzzleManager : PuzzleManager
    {
        //new protected const string INPUT_FILE_NAME = "test.txt";

        public Day19PuzzleManager()
        {
            var inputHelper = new Day19InputHelper(INPUT_FILE_NAME);
            Blueprints = inputHelper.Parse();
        }

        public List<Blueprint> Blueprints { get; set; }

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
            var solution = 0;
            foreach (var blueprint in Blueprints)
            {
                solution += blueprint.Name * FindMaxGeodes(blueprint, 24);
            }
            Console.WriteLine($"The solution to part one is '{solution}'.");
            return Task.CompletedTask;
        }

        private int FindMaxGeodes(Blueprint blueprint, int maxTime, int bestCurrentResult = 0)
        {
            var solution = 0;
            var states = new PriorityQueue<TimeState, int>();
            states.Enqueue(
                new TimeState(
                    1,
                    (1, 1),
                    (0, 0),
                    (0, 0),
                    (0, 0)),
                maxTime - 1
                );

            while (states.TryDequeue(out var currentState, out var timeRemaining))
            {
                if (currentState.Minute == maxTime)
                {
                    solution = Math.Max(solution, currentState.Geode.Material);
                }

                // Optimisation - if building a geode machine every minute from now
                // would not beat the current best solution then discard this state.
                var maxPossibleGeodes = currentState.Geode.Material + timeRemaining * currentState.Geode.Machine + timeRemaining * (timeRemaining - 1) / 2;
                if (maxPossibleGeodes <= solution || maxPossibleGeodes <= bestCurrentResult)
                {
                    continue;
                }

                // Queue the state where nothing is built
                var noBuildState = new TimeState(
                    currentState.Minute + 1,
                    (currentState.Ore.Material + currentState.Ore.Machine, currentState.Ore.Machine),
                    (currentState.Clay.Material + currentState.Clay.Machine, currentState.Clay.Machine),
                    (currentState.Obsidian.Material + currentState.Obsidian.Machine, currentState.Obsidian.Machine),
                    (currentState.Geode.Material + currentState.Geode.Machine, currentState.Geode.Machine));
                states.Enqueue(noBuildState, timeRemaining - 1);

                // If possible queue an ore machine state
                if (currentState.Ore.Material >= blueprint.OreMachineCost)
                {
                    states.Enqueue(noBuildState with
                    {
                        Ore = (noBuildState.Ore.Material - blueprint.OreMachineCost, noBuildState.Ore.Machine + 1)
                    },
                    timeRemaining - 1);
                }

                // If possible queue a clay machine state
                if (currentState.Ore.Material >= blueprint.ClayMachineCost)
                {
                    states.Enqueue(noBuildState with
                    {
                        Ore = (noBuildState.Ore.Material - blueprint.ClayMachineCost, noBuildState.Ore.Machine),
                        Clay = (noBuildState.Clay.Material, noBuildState.Clay.Machine + 1)
                    },
                    timeRemaining - 1);
                }

                // If possible queue a obsidian machine state
                if (currentState.Ore.Material >= blueprint.ObsidianMachineCost.Ore && currentState.Clay.Material >= blueprint.ObsidianMachineCost.Clay)
                {
                    states.Enqueue(noBuildState with
                    {
                        Ore = (noBuildState.Ore.Material - blueprint.ObsidianMachineCost.Ore, noBuildState.Ore.Machine),
                        Clay = (noBuildState.Clay.Material - blueprint.ObsidianMachineCost.Clay, noBuildState.Clay.Machine),
                        Obsidian = (noBuildState.Obsidian.Material, noBuildState.Obsidian.Machine + 1)
                    },
                    timeRemaining - 1);
                }

                // If possible queue a geode machine state
                if (currentState.Ore.Material >= blueprint.GeodeMachineCost.Ore && currentState.Obsidian.Material >= blueprint.GeodeMachineCost.Obsidian)
                {
                    states.Enqueue(noBuildState with
                    {
                        Ore = (noBuildState.Ore.Material - blueprint.GeodeMachineCost.Ore, noBuildState.Ore.Machine),
                        Obsidian = (noBuildState.Obsidian.Material - blueprint.GeodeMachineCost.Obsidian, noBuildState.Obsidian.Machine),
                        Geode = (noBuildState.Geode.Material, noBuildState.Geode.Machine + 1)
                    },
                    timeRemaining - 1);
                }
            }

            return solution;
        }

        public override Task SolvePartTwo()
        {
            var solution = 0;
            Console.WriteLine($"The solution to part two is '{solution}'.");
            return Task.CompletedTask;
        }
    }
}
