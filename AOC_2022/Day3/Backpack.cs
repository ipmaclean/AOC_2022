namespace AOC_2022.Day3
{
    public class Backpack
    {
        public string CompartmentOne { get; set; }
        public string CompartmentTwo { get; set; }

        public string AllItems => CompartmentOne + CompartmentTwo;

        public Backpack(string compartmentOne, string compartmentTwo)
        {
            CompartmentOne = compartmentOne;
            CompartmentTwo = compartmentTwo;
        }

        public char FindSharedItem()
            => CompartmentOne.ToCharArray().Intersect(CompartmentTwo.ToCharArray())
                .Single();
    }
}
