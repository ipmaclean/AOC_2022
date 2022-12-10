namespace AOC_2022.Day10
{
    public class CrtComputer
    {
        private List<Input> _programInput;
        private int _clockCycle = 0;
        private int _xRegister = 1;

        public CrtComputer(List<Input> programInput)
        {
            _programInput = programInput;
        }

        public event EventHandler<ClockChangedEventArgs>? ClockChangedEvent;

        public void Run()
        {
            foreach (var input in _programInput)
            {
                ProcessInput(input);
            }
        }

        public void Reset()
        {
            _clockCycle = 0;
            _xRegister = 1;
        }

        private void AdvanceClock()
        {
            _clockCycle++;
            RaiseClockChangedEvent();
        }

        private void ProcessInput(Input input)
        {
            switch (input.Instruction)
            {
                case Instruction.NoOp:
                    ProcessNoop();
                    break;
                case Instruction.AddX:
                    ProcessAddX(input.Value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(input.Instruction), $"Unexpected instruction: {input.Instruction}");
            };
        }

        private void ProcessNoop()
        {
            AdvanceClock();
        }

        private void ProcessAddX(int? value)
        {
            if (!value.HasValue)
            {
                throw new ArgumentException($"ProcessAddX value was unexpectedly null.");
            }
            AdvanceClock();
            AdvanceClock();
            _xRegister += value.Value;
        }

        private void RaiseClockChangedEvent()
        {
            EventHandler<ClockChangedEventArgs>? raiseClockChangedEvent = ClockChangedEvent;

            if (raiseClockChangedEvent != null)
            {
                raiseClockChangedEvent(this, new ClockChangedEventArgs(_clockCycle, _xRegister));
            }
        }
    }
}
