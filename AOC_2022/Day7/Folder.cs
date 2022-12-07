namespace AOC_2022.Day7
{
    public class Folder
    {
        public Folder(string name, Folder? parent)
        {
            Name = name;
            Parent = parent;
        }

        public string Name { get; set; }
        public long? Size { get; set; }
        public Folder? Parent { get; set; }

        public ICollection<Folder> Folders { get; set; } = new List<Folder>();
        public ICollection<File> Files { get; set; } = new List<File>();
    }
}
