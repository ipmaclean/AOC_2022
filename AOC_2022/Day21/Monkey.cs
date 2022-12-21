namespace AOC_2022.Day21
{
    public class Monkey
    {
        public Monkey(
            string name,
            long? value,
            Func<long, long, long>? operation, 
            (string, string)? monkeys,
            Func<long, long, long>? invOperation1,
            Func<long, long, long>? invOperation2)
        {
            Name = name;
            Number = value;
            Operation = operation;
            Monkeys = monkeys;
            InvOperation1 = invOperation1;
            InvOperation2 = invOperation2;
        }

        public string Name { get; private set; }
        public long? Number { get; set; }
        public Func<long, long, long>? Operation { get; private set; }
        public (string, string)? Monkeys { get; private set; }
        public Func<long, long, long>? InvOperation1 { get; private set; }
        public Func<long, long, long>? InvOperation2 { get; private set; }
    }
}
