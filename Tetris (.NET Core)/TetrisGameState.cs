using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris
{
    public class TetrisGameState
    {
        public TetrisGameState(int tetrisRows, int tetrisColumns)
        {
             this.HighScore = 0;
             this.Score = 0;
             this.Frame = 0;
             this.Level = 1;
             this.FramesToMoveFigure = 3;
             this.CurrentFigure = null;
             this.CurrentFigureRow = 0;
             this.CurrentFigureCol = 0;
             this.TetrisField = new bool[tetrisRows, tetrisColumns];
        }

        public int HighScore { get; set; }
        public int Score { get; set; }
        public int Frame { get; set; }
        public int Level { get; private set; }
        public int FramesToMoveFigure { get; private set; }
        public bool[,] CurrentFigure { get; set; }
        public int CurrentFigureRow { get; set; }
        public int CurrentFigureCol { get; set; }
        public bool[,] TetrisField { get; private set; }

        public void UpdateLevel()
        {
            // TODO: On next level lower FramesToMoveFigure
            if (this.Score <= 0)
            {
                this.Level = 1;
                return;
            }

            this.Level = (int)Math.Log10(this.Score) - 1;
            if (this.Level < 1)
            {
                this.Level = 1;
            }

            if (this.Level > 10)
            {
                this.Level = 10;
            }
        }
    }
}
