using System.Diagnostics;

namespace AOC_2022.Day19
{
    public class Day19PuzzleManager : PuzzleManager
    {
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

        public override Task SolvePartTwo()
        {
            var solution = 1;
            for (var i = 0; i < 3; i++)
            {
                solution *= FindMaxGeodes(Blueprints[i], 32);
            }
            Console.WriteLine($"The solution to part two is '{solution}'.");
            return Task.CompletedTask;
        }

        private int FindMaxGeodes(Blueprint blueprint, int maxTime)
        {
            var mostExpensiveOreCost = Math.Max(blueprint.OreMachineCost, blueprint.ClayMachineCost);
            mostExpensiveOreCost = Math.Max(mostExpensiveOreCost, blueprint.ObsidianMachineCost.Ore);
            mostExpensiveOreCost = Math.Max(mostExpensiveOreCost, blueprint.GeodeMachineCost.Ore);

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
                // Optimisation - if building a geode machine every minute from now
                // would not beat the current best solution then discard this state.
                var maxPossibleGeodes = currentState.Geode.Material + timeRemaining * currentState.Geode.Machine + timeRemaining * (timeRemaining - 1) / 2;
                if (maxPossibleGeodes <= solution)
                {
                    continue;
                }

                // If possible queue an ore machine state if you don't already have
                // enough to create the most expensive machine every minute.
                var minutesUntilCanAffordOreMachine = Math.Max(0, (int)Math.Ceiling((double)(blueprint.OreMachineCost - currentState.Ore.Material) / (double)currentState.Ore.Machine));
                if (timeRemaining - minutesUntilCanAffordOreMachine - 1 < 0 && currentState.Ore.Machine < mostExpensiveOreCost)
                {
                    solution = Math.Max(solution, currentState.Geode.Material + currentState.Geode.Machine * timeRemaining);
                }
                else if (currentState.Ore.Machine < mostExpensiveOreCost)
                {
                    states.Enqueue(new TimeState(
                        currentState.Minute + minutesUntilCanAffordOreMachine + 1,
                        (currentState.Ore.Material + currentState.Ore.Machine * (minutesUntilCanAffordOreMachine + 1) - blueprint.OreMachineCost, currentState.Ore.Machine + 1),
                        (currentState.Clay.Material + currentState.Clay.Machine * (minutesUntilCanAffordOreMachine + 1), currentState.Clay.Machine),
                        (currentState.Obsidian.Material + currentState.Obsidian.Machine * (minutesUntilCanAffordOreMachine + 1), currentState.Obsidian.Machine),
                        (currentState.Geode.Material + currentState.Geode.Machine * (minutesUntilCanAffordOreMachine + 1), currentState.Geode.Machine)),
                    timeRemaining - minutesUntilCanAffordOreMachine - 1);
                }

                // If possible queue a clay machine state if you don't already have
                // enough to create an obsidian machine every minute.
                var minutesUntilCanAffordClayMachine = Math.Max(0, (int)Math.Ceiling((double)(blueprint.ClayMachineCost - currentState.Ore.Material) / (double)currentState.Ore.Machine));
                if (timeRemaining - minutesUntilCanAffordClayMachine - 1 < 0 && currentState.Clay.Machine < blueprint.ObsidianMachineCost.Clay)
                {
                    solution = Math.Max(solution, currentState.Geode.Material + currentState.Geode.Machine * timeRemaining);
                }
                else if (currentState.Clay.Machine < blueprint.ObsidianMachineCost.Clay)
                {
                    states.Enqueue(new TimeState(
                        currentState.Minute + minutesUntilCanAffordClayMachine + 1,
                        (currentState.Ore.Material + currentState.Ore.Machine * (minutesUntilCanAffordClayMachine + 1) - blueprint.ClayMachineCost, currentState.Ore.Machine),
                        (currentState.Clay.Material + currentState.Clay.Machine * (minutesUntilCanAffordClayMachine + 1), currentState.Clay.Machine + 1),
                        (currentState.Obsidian.Material + currentState.Obsidian.Machine * (minutesUntilCanAffordClayMachine + 1), currentState.Obsidian.Machine),
                        (currentState.Geode.Material + currentState.Geode.Machine * (minutesUntilCanAffordClayMachine + 1), currentState.Geode.Machine)),
                    timeRemaining - minutesUntilCanAffordClayMachine - 1);
                }

                // If possible queue a obsidian machine state if you don't already have
                // enough to create a geode machine machine every minute.
                var minutesUntilCanAffordOreCostOfObsidianMachine = Math.Max(0, (int)Math.Ceiling((double)(blueprint.ObsidianMachineCost.Ore - currentState.Ore.Material) / (double)currentState.Ore.Machine));
                var minutesUntilCanAffordClayCostOfObsidianMachine = Math.Max(0, (int)Math.Ceiling((double)(blueprint.ObsidianMachineCost.Clay - currentState.Clay.Material) / (double)currentState.Clay.Machine));
                var minutesUntilCanAffordObsidianMachine = Math.Max(minutesUntilCanAffordOreCostOfObsidianMachine, minutesUntilCanAffordClayCostOfObsidianMachine);
                if (currentState.Clay.Machine > 0 && timeRemaining - minutesUntilCanAffordObsidianMachine - 1 < 0 && currentState.Obsidian.Machine < blueprint.GeodeMachineCost.Obsidian)
                {
                    solution = Math.Max(solution, currentState.Geode.Material + currentState.Geode.Machine * timeRemaining);
                }
                else if (currentState.Clay.Machine > 0 && currentState.Obsidian.Machine < blueprint.GeodeMachineCost.Obsidian)
                {
                    states.Enqueue(new TimeState(
                        currentState.Minute + minutesUntilCanAffordObsidianMachine + 1,
                        (currentState.Ore.Material + currentState.Ore.Machine * (minutesUntilCanAffordObsidianMachine + 1) - blueprint.ObsidianMachineCost.Ore, currentState.Ore.Machine),
                        (currentState.Clay.Material + currentState.Clay.Machine * (minutesUntilCanAffordObsidianMachine + 1) - blueprint.ObsidianMachineCost.Clay, currentState.Clay.Machine),
                        (currentState.Obsidian.Material + currentState.Obsidian.Machine * (minutesUntilCanAffordObsidianMachine + 1), currentState.Obsidian.Machine + 1),
                        (currentState.Geode.Material + currentState.Geode.Machine * (minutesUntilCanAffordObsidianMachine + 1), currentState.Geode.Machine)),
                    timeRemaining - minutesUntilCanAffordObsidianMachine - 1);
                }

                // If possible queue a geode machine state
                var minutesUntilCanAffordOreCostOfGeodeMachine = Math.Max(0, (int)Math.Ceiling((double)(blueprint.GeodeMachineCost.Ore - currentState.Ore.Material) / (double)currentState.Ore.Machine));
                var minutesUntilCanAffordObsidianCostOfGeodeMachine = Math.Max(0, (int)Math.Ceiling((double)(blueprint.GeodeMachineCost.Obsidian - currentState.Obsidian.Material) / (double)currentState.Obsidian.Machine));
                var minutesUntilCanAffordGeodeMachine = Math.Max(minutesUntilCanAffordOreCostOfGeodeMachine, minutesUntilCanAffordObsidianCostOfGeodeMachine);
                if (currentState.Obsidian.Machine > 0 && timeRemaining - minutesUntilCanAffordGeodeMachine - 1 < 0)
                {
                    solution = Math.Max(solution, currentState.Geode.Material + currentState.Geode.Machine * timeRemaining);
                }
                else if (currentState.Obsidian.Machine > 0)
                {
                    states.Enqueue(new TimeState(
                        currentState.Minute + minutesUntilCanAffordGeodeMachine + 1,
                        (currentState.Ore.Material + currentState.Ore.Machine * (minutesUntilCanAffordGeodeMachine + 1) - blueprint.GeodeMachineCost.Ore, currentState.Ore.Machine),
                        (currentState.Clay.Material + currentState.Clay.Machine * (minutesUntilCanAffordGeodeMachine + 1), currentState.Clay.Machine),
                        (currentState.Obsidian.Material + currentState.Obsidian.Machine * (minutesUntilCanAffordGeodeMachine + 1) - blueprint.GeodeMachineCost.Obsidian, currentState.Obsidian.Machine),
                        (currentState.Geode.Material + currentState.Geode.Machine * (minutesUntilCanAffordGeodeMachine + 1), currentState.Geode.Machine + 1)),
                    timeRemaining - minutesUntilCanAffordGeodeMachine - 1);
                }
            }

            return solution;
        }
    }
}
