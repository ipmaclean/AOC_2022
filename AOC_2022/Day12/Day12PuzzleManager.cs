﻿namespace AOC_2022.Day12
{
    public class Day12PuzzleManager : PuzzleManager
    {
        public Day12PuzzleManager()
        {
            var inputHelper = new Day12InputHelper(INPUT_FILE_NAME);
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
            var solution = 0;
            Console.WriteLine($"The solution to part one is '{solution}'.");
            return Task.CompletedTask;
        }

        public override Task SolvePartTwo()
        {
            var solution = 0;
            Console.WriteLine($"The solution to part two is '{solution}'.");
            return Task.CompletedTask;
        }
    }
}