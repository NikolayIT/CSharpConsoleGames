namespace Tetris
{
    public class LineTetrisGame : TetrisGame
    {
        public LineTetrisGame(int tetrisRows, int tetrisColumns) :
            base(tetrisRows, tetrisColumns)
        {
        }

        public override void NewRandomFigure()
        {
            this.CurrentFigure = TetrisFigures[0].GetRotate();
            this.CurrentFigureRow = 0;
            this.CurrentFigureCol = this.TetrisColumns / 2 - this.CurrentFigure.Width / 2;
        }
    }
}
