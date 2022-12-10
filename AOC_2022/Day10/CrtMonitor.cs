using System.Text;

namespace AOC_2022.Day10
{
    public  class CrtMonitor
    {
        private StringBuilder _stringBuilder = new StringBuilder();
        public CrtMonitor(CrtComputer crtComputer)
        {
            crtComputer.ClockChangedEvent += HandleClockChanged;
        }

        private void HandleClockChanged(object? sender, ClockChangedEventArgs e)
        {
            var pixel = Math.Abs(e.XRegister - ((e.ClockCycle - 1) % 40)) <= 1 ? '#' : ' ';
            _stringBuilder.Append(pixel);
            if (e.ClockCycle % 40 == 0)
            {
                _stringBuilder.Append(Environment.NewLine);
            }
        }

        public void Print()
        {
            Console.WriteLine(_stringBuilder.ToString());
        }
    }
}