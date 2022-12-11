namespace AOC_2022.Day11
{
    internal class Monkey
    {
        public Monkey(Queue<long> items, Func<long, long> operation, long divisibilityTest, int trueMonkey, int falseMonkey)
        {
            Items = items;
            Operation = operation;
            DivisibilityTest = divisibilityTest;
            TrueMonkey = trueMonkey;
            FalseMonkey = falseMonkey;
        }

        public Queue<long> Items { get; private set; }
        public Func<long, long> Operation { get; private set; }
        public long DivisibilityTest { get; private set; }
        public int TrueMonkey { get; private set; }
        public int FalseMonkey { get; private set; }
        public long InspectionCount { get; private set; } = 0;

        public (long itemWorryLevel, int monkeyToThrowTo) InspectAndThrow(bool isPartOne)
        {
            var item = Items.Dequeue();
            InspectionCount++;
            item = Operation(item);
            if (isPartOne)
            {
                item /= 3;
            }
            return item % DivisibilityTest == 0 ?
                (item, TrueMonkey) :
                (item, FalseMonkey);
        }

        public void AddItem(long item)
        {
            Items.Enqueue(item);
        }
    }
}
