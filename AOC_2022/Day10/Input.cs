namespace AOC_2022.Day10
{
    public class Input
    {
        public Input(Instruction instruction, int? value = null)
        {
            Instruction = instruction;
            Value = value;
        }

        public Instruction Instruction { get; set; }
        public int? Value { get; set; }
    }
}
