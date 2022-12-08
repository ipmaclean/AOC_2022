namespace AOC_2022.Day8
{
    public class Tree
    {
        public int Height { get; private set; }
        public bool IsVisible { get; private set; } = false;

        public Tree(int height)
        {
            Height = height;
        }

        public void SetVisible()
            => IsVisible = true;
    }
}
