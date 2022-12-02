namespace AOC_2022.Day2
{
    public class RpsGame
    {
        public Shape OpponentShape { get; set; }
        public Shape MyShape { get; set; }
        public EndState EndState { get; set; }

        public RpsGame(Shape opponentShape, Shape myShape, EndState endState)
        {
            OpponentShape = opponentShape;
            MyShape = myShape;
            EndState = endState;
        }

        public int CalculateScorePartOne()
        {
            if (MyShape == OpponentShape)
            {
                return (int)MyShape + 1 + (int)EndState.Draw;
            }
            if (((int)OpponentShape + 1) % 3 == (int)MyShape)
            {
                return (int)MyShape + 1 + (int)EndState.Win;
            }
            return (int)MyShape + 1 + (int)EndState.Loss;
        }

        public int CalculateScorePartTwo()
        {
            int shapeScore;
            if(EndState == EndState.Loss)
            {
                shapeScore = ((int)OpponentShape + 2) % 3 + 1;
            }
            else if (EndState == EndState.Draw)
            {
                shapeScore = (int)OpponentShape + 1;
            }
            else
            {
                shapeScore = ((int)OpponentShape + 1) % 3 + 1;
            }
            return shapeScore + (int)EndState;
        }
    }
}
