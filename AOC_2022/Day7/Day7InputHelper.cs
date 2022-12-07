using System.Text.RegularExpressions;

namespace AOC_2022.Day7
{
    internal class Day7InputHelper : InputHelper<List<Folder>>
    {
        public Day7InputHelper(string fileName) : base(fileName)
        {
        }

        public override List<Folder> Parse()
        {
            var output = new List<Folder>();
            using (var sr = new StreamReader(InputPath))
            {
                var numberRegex = new Regex(@"\d+");
                Folder currenctFolder = new Folder("", null);

                // Note that the list won't contain any folders that aren't navigated into - if such empty, unchecked folders exist
                string ln;
                while ((ln = sr.ReadLine()!) != null)
                {
                    if (ln.Substring(0, 4) == "$ cd")
                    {
                        if (ln.Substring(5) == "..")
                        {
                            currenctFolder.Size = currenctFolder.Folders.Sum(x => x.Size) + currenctFolder.Files.Sum(x => x.Size);
                            currenctFolder = currenctFolder.Parent!;
                        }
                        else
                        {
                            var childFolder = new Folder(ln.Substring(5), currenctFolder);
                            output.Add(childFolder);
                            currenctFolder.Folders.Add(childFolder);
                            currenctFolder = childFolder;
                        }
                        continue;
                    }
                    if (ln == "$ ls")
                    {
                        continue;
                    }
                    if (ln.Substring(0, 3) == "dir")
                    {
                        continue;
                    }
                    var size = numberRegex.Match(ln).Value;
                    currenctFolder.Files.Add(new File(ln.Substring(size.Length + 1), long.Parse(size), currenctFolder));
                }

                while (currenctFolder != null)
                {
                    currenctFolder.Size = currenctFolder.Folders.Sum(x => x.Size) + currenctFolder.Files.Sum(x => x.Size);
                    currenctFolder = currenctFolder.Parent!;
                }
            }
            return output;
        }
    }
}
