using System;

namespace Tetris
{
    public class TetrisConsoleWriter
    {
        private int tetrisRows;
        private int tetrisColumns;
        private int infoColumns;
        private int consoleRows;
        private int consoleColumns;
        private char tetrisCharacter;

        public TetrisConsoleWriter(
            int tetrisRows,
            int tetrisColumns,
            char tetrisCharacter = '*',
            int infoColumns = 11)
        {
            this.tetrisRows = tetrisRows;
            this.tetrisColumns = tetrisColumns;
            this.tetrisCharacter = tetrisCharacter;
            this.infoColumns = infoColumns;
            this.consoleRows = 1 + this.tetrisRows + 1;
            this.consoleColumns = 1 + this.tetrisColumns + 1 + this.infoColumns + 1;

            this.Frame = 0;
            this.FramesToMoveFigure = 15;

            Console.WindowHeight = this.consoleRows + 1;
            Console.WindowWidth = this.consoleColumns;
            
            Console.BufferHeight = this.consoleRows + 1;
            Console.BufferWidth = this.consoleColumns;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Title = "Tetris v1.0";
            Console.CursorVisible = false;
            new WindowBlocker().Block();
        }

        public int Frame { get; set; }

        public int FramesToMoveFigure { get; private set; }

        public void DrawAll(ITetrisGame state, ScoreManager scoreManager)
        {
            this.DrawBorder();
            this.DrawGameState(3 + this.tetrisColumns, state, scoreManager);
            this.DrawTetrisField(state.TetrisField);
            this.DrawCurrentFigure(state.CurrentFigure, state.CurrentFigureRow, state.CurrentFigureCol);

        }

        public void DrawGameState(int startColumn, ITetrisGame state, ScoreManager scoreManager)
        {
            this.Write("Level:", 1, startColumn);
            this.Write(state.Level.ToString(), 2, startColumn);
            this.Write("Score:", 4, startColumn);
            this.Write(scoreManager.Score.ToString(), 5, startColumn);
            this.Write("Best:", 7, startColumn);
            this.Write(scoreManager.HighScore.ToString(), 8, startColumn);
            this.Write("Frame:", 10, startColumn);
            this.Write(this.Frame.ToString() + " / " + (this.FramesToMoveFigure - state.Level).ToString(), 11, startColumn);
            this.Write("Position:", 13, startColumn);
            this.Write($"{state.CurrentFigureRow}, {state.CurrentFigureCol}", 14, startColumn);
            this.Write("Keys:", 16, startColumn);
            this.Write($"  ^ ", 18, startColumn);
            this.Write($"<   > ", 19, startColumn);
            this.Write($"  v  ", 20, startColumn);
        }

        public void DrawBorder()
        {
            Console.SetCursorPosition(0, 0);
            string line = "╔";
            line += new string('═', this.tetrisColumns);
            /* for (int i = 0; i < TetrisCols; i++)
            {
                line += "═";
            } */

            line += "╦";
            line += new string('═', this.infoColumns);
            line += "╗";
            Console.Write(line);

            for (int i = 0; i < this.tetrisRows; i++)
            {
                string middleLine = "║";
                middleLine += new string(' ', this.tetrisColumns);
                middleLine += "║";
                middleLine += new string(' ', this.infoColumns);
                middleLine += "║";
                Console.Write(middleLine);
            }

            string endLine = "╚";
            endLine += new string('═', this.tetrisColumns);
            endLine += "╩";
            endLine += new string('═', this.infoColumns);
            endLine += "╝";
            Console.Write(endLine);
        }

        public void WriteGameOver(int score)
        {
            int row = this.tetrisRows / 2 - 3;
            int column = (this.tetrisColumns + 3 + this.infoColumns) / 2 - 6;

            var scoreAsString = score.ToString();
            scoreAsString = new string(' ', 7 - scoreAsString.Length) + scoreAsString;
            Write("╔═════════╗", row, column);
            Write("║ Game    ║", row + 1, column);
            Write("║   over! ║", row + 2, column);
            Write($"║ {scoreAsString} ║", row + 3, column);
            Write("╚═════════╝", row + 4, column);
        }

        public void DrawTetrisField(bool[,] tetrisField)
        {
            for (int row = 0; row < tetrisField.GetLength(0); row++)
            {
                string line = "";
                for (int col = 0; col < tetrisField.GetLength(1); col++)
                {
                    if (tetrisField[row, col])
                    {
                        line += tetrisCharacter;
                    }
                    else
                    {
                        line += " ";
                    }
                }

                this.Write(line, row + 1, 1);
            }
        }

        public void DrawCurrentFigure(Tetromino currentFigure, int currentFigureRow, int currentFigureColumn)
        {
            for (int row = 0; row < currentFigure.Width; row++)
            {
                for (int col = 0; col < currentFigure.Height; col++)
                {
                    if (currentFigure.Body[row, col])
                    {
                        Write(tetrisCharacter.ToString(), row + 1 + currentFigureRow, 1 + currentFigureColumn + col);
                    }
                }
            }
        }

        private void Write(string text, int row, int col)
        {
            Console.SetCursorPosition(col, row);
            Console.Write(text);
        }
    }
}
