namespace AOC_2022.Day16
{
    public class Valve
    {
        public Valve(
            string name,
            int rate,
            List<string> connectedValves
            )
        {
            Name = name;
            Rate = rate;
            ConnectedValves = connectedValves;
        }

        public string Name { get; private set; }
        public int Rate { get; private set; }
        public List<string> ConnectedValves { get; set; } = new List<string>();
        public Dictionary<Valve, int> Distances { get; set; } = new Dictionary<Valve, int>();
        public int ShortestPath { get; set; } = -1;
        public bool IsOpened { get; set; } = false;
    }
}
