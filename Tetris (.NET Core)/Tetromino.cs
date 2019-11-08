namespace Tetris
{
    public class Tetromino
    {
        public Tetromino(bool[,] body)
        {
            this.Body = body;
        }

        public bool[,] Body { get; private set; }

        public int Width => this.Body.GetLength(0);

        public int Height => this.Body.GetLength(1);

        public Tetromino GetRotate()
        {
            var newFigure = new bool[this.Height, this.Width];
            for (int row = 0; row < this.Width; row++)
            {
                for (int col = 0; col < this.Height; col++)
                {
                    newFigure[col, this.Width - row - 1] = this.Body[row, col];
                }
            }

            return new Tetromino(newFigure);
        }
    }
}
