namespace AOC_2022.Day20
{
    public class WrappedInt
    {
        public WrappedInt(long value, long originalIndex)
        {
            Value = value;
            OriginalIndex = originalIndex;
        }

        public long Value { get; set; }
        public long OriginalIndex { get; set; }
    }
}
