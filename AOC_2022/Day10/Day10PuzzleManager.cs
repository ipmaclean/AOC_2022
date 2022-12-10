namespace AOC_2022.Day10
{
    public class Day10PuzzleManager : PuzzleManager
    {
        private Day10InputHelper _inputHelper { get; set; }
        private int _signalStrengths = 0;

        public Day10PuzzleManager()
        {
            var inputHelper = new Day10InputHelper(INPUT_FILE_NAME);
            _inputHelper = inputHelper;
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
            var crtComputer = new CrtComputer(_inputHelper.Parse());
            crtComputer.ClockChangedEvent += HandleClockChanged;
            _signalStrengths = 0;
            crtComputer.Run();
            crtComputer.ClockChangedEvent -= HandleClockChanged;
            Console.WriteLine($"The solution to part one is '{_signalStrengths}'.");
            return Task.CompletedTask;
        }

        public override Task SolvePartTwo()
        {
            var crtComputer = new CrtComputer(_inputHelper.Parse());
            var crtMonitor = new CrtMonitor(crtComputer);
            crtComputer.Run();
            Console.WriteLine($"The solution to part two is:");
            crtMonitor.Print();
            return Task.CompletedTask;
        }

        private void HandleClockChanged(object? sender, ClockChangedEventArgs e)
        {
            if ((e.ClockCycle + 20) % 40 == 0)
            {
                _signalStrengths += e.ClockCycle * e.XRegister;

            }
        }
    }
}
