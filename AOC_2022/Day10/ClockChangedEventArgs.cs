public class ClockChangedEventArgs : EventArgs
{
    public ClockChangedEventArgs(int clockCycle, int xRegister)
    {
        ClockCycle = clockCycle;
        XRegister = xRegister;
    }

    public int ClockCycle { get; set; }
    public int XRegister { get; set; }
}