namespace AOC_2022.Day7
{
    public class File
    {
        public File(string name, long size, Folder parent)
        {
            Name = name;
            Size = size;
            Parent = parent;
        }

        public string Name { get; set; }
        public long Size { get; set; }
        public Folder Parent { get; set; }
    }
}
