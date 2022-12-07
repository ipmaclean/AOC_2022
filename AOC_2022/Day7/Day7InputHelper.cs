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
                var currentFolder = new Folder("/", null);
                output.Add(currentFolder);

                // Note that the list won't contain any folders that aren't navigated into - if such empty, unchecked folders exist
                string ln;
                while ((ln = sr.ReadLine()!) != null)
                {
                    if (ln.Substring(0, 4) == "$ cd")
                    {
                        if (ln.Substring(5) == "..")
                        {
                            currentFolder.Size = currentFolder.Folders.Sum(x => x.Size) + currentFolder.Files.Sum(x => x.Size);
                            currentFolder = currentFolder.Parent!;
                        }
                        else if (ln.Substring(5) == "/")
                        {
                            continue;
                        }
                        else
                        {
                            var childFolder = new Folder(ln.Substring(5), currentFolder);
                            output.Add(childFolder);
                            currentFolder.Folders.Add(childFolder);
                            currentFolder = childFolder;
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
                    currentFolder.Files.Add(new File(ln.Substring(size.Length + 1), long.Parse(size), currentFolder));
                }

                while (currentFolder != null)
                {
                    currentFolder.Size = currentFolder.Folders.Sum(x => x.Size) + currentFolder.Files.Sum(x => x.Size);
                    currentFolder = currentFolder.Parent!;
                }
            }
            return output;
        }
    }
}
