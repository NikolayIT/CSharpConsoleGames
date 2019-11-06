using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace Tetris
{
    class Program
    {
        // Settings
        static int TetrisRows = 20;
        static int TetrisCols = 10;
        static int InfoCols = 20;
        static int ConsoleRows = 1 + TetrisRows + 1;
        static int ConsoleCols = 1 + TetrisCols + 1 + InfoCols + 1;
        static List<bool[,]> TetrisFigures = new List<bool[,]>()
            {
                new bool[,] // ----
                {
                    { true, true, true, true }
                },
                new bool[,] // O
                {
                    { true, true },
                    { true, true }
                },
                /*new bool[,] // big O
                {
                    { true, true, true },
                    { true, true, true },
                    { true, true, true },
                },*/
                new bool[,] // T
                {
                    { false, true, false },
                    { true, true, true },
                },
                new bool[,] // S
                {
                    { false, true, true, },
                    { true, true, false, },
                },
                new bool[,] // Z
                {
                    { true, true, false },
                    { false, true, true },
                },
                new bool[,] // J
                {
                    { true, false, false },
                    { true, true, true }
                },
                new bool[,] // L
                {
                    { false, false, true },
                    { true, true, true }
                },
            };
        static int[] ScorePerLines = { 0, 40, 100, 300, 1200 };

        // State
        static TetrisGameState State = new TetrisGameState(TetrisRows, TetrisCols);
        static Random Random = new Random();

        static void Main(string[] args)
        {
            var musicPlayer = new MusicPlayer();
            //musicPlayer.Play();

            var scoreManager = new ScoreManager("scores.txt");
            State.HighScore = scoreManager.GetHighScore();

            Console.ForegroundColor = ConsoleColor.White;
            Console.Title = "Tetris v1.0";
            Console.CursorVisible = false;
            Console.WindowHeight = ConsoleRows + 1;
            Console.WindowWidth = ConsoleCols;
            Console.BufferHeight = ConsoleRows + 1;
            Console.BufferWidth = ConsoleCols;
            State.CurrentFigure = TetrisFigures[Random.Next(0, TetrisFigures.Count)];
            while (true)
            {
                State.Frame++;
                State.UpdateLevel();
                // Read user input
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Escape)
                    {
                        // Environment.Exit(0);
                        return; // Becase of Main()
                    }
                    if (key.Key == ConsoleKey.LeftArrow || key.Key == ConsoleKey.A)
                    {
                        if (State.CurrentFigureCol >= 1 && !CheckForLeftCollision(State.CurrentFigure))
                        {
                            State.CurrentFigureCol--;
                        }
                    }
                    if (key.Key == ConsoleKey.RightArrow || key.Key == ConsoleKey.D)
                    {
                        if (State.CurrentFigureCol < TetrisCols - State.CurrentFigure.GetLength(1)
                            && !CheckForRightCollision(State.CurrentFigure))
                        {
                            State.CurrentFigureCol++;
                        }
                    }
                    if (key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.S)
                    {
                        State.Frame = 1;
                        State.Score += State.Level;
                        State.CurrentFigureRow++;
                    }
                    if (key.Key == ConsoleKey.Spacebar || key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.W)
                    {
                        RotateCurrentFigure();
                    }
                }

                // Update the game state
                if (State.Frame % (State.FramesToMoveFigure - State.Level) == 0)
                {
                    State.CurrentFigureRow++;
                    State.Frame = 0;
                }

                if (Collision(State.CurrentFigure))
                {
                    AddCurrentFigureToTetrisField();
                    int lines = CheckForFullLines();
                    State.Score += ScorePerLines[lines] * State.Level;
                    State.CurrentFigure = TetrisFigures[Random.Next(0, TetrisFigures.Count)];
                    State.CurrentFigureRow = 0;
                    State.CurrentFigureCol = 0;
                    if (Collision(State.CurrentFigure)) // game is over
                    {
                        scoreManager.Add(State.Score);
                        var scoreAsString = State.Score.ToString();
                        scoreAsString += new string(' ', 7 - scoreAsString.Length);
                        Write("╔═════════╗", 5, 5);
                        Write("║ Game    ║", 6, 5);
                        Write("║   over! ║", 7, 5);
                        Write($"║ {scoreAsString} ║", 8, 5);
                        Write("╚═════════╝", 9, 5);
                        Thread.Sleep(100000);
                        return;
                    }
                }

                // Redraw UI
                DrawBorder();
                DrawInfo();
                DrawTetrisField();
                DrawCurrentFigure();

                Thread.Sleep(40);
            }
        }

        private static void RotateCurrentFigure()
        {
            var newFigure = new bool[State.CurrentFigure.GetLength(1), State.CurrentFigure.GetLength(0)];
            for (int row = 0; row < State.CurrentFigure.GetLength(0); row++)
            {
                for (int col = 0; col < State.CurrentFigure.GetLength(1); col++)
                {
                    newFigure[col, State.CurrentFigure.GetLength(0) - row - 1] = State.CurrentFigure[row, col];
                }
            }

            if (!Collision(newFigure))
            {
                State.CurrentFigure = newFigure;
            }
        }

        private static int CheckForFullLines() // 0, 1, 2, 3, 4
        {
            int lines = 0;

            for (int row = 0; row < State.TetrisField.GetLength(0); row++)
            {
                bool rowIsFull = true;
                for (int col = 0; col < State.TetrisField.GetLength(1); col++)
                {
                    if (State.TetrisField[row, col] == false)
                    {
                        rowIsFull = false;
                        break;
                    }
                }

                if (rowIsFull)
                {
                    for (int rowToMove = row; rowToMove >= 1; rowToMove--)
                    {
                        for (int col = 0; col < State.TetrisField.GetLength(1); col++)
                        {
                            State.TetrisField[rowToMove, col] = State.TetrisField[rowToMove - 1, col];
                        }
                    }

                    lines++;
                }
            }

            return lines;
        }

        static void AddCurrentFigureToTetrisField()
        {
            for (int row = 0; row < State.CurrentFigure.GetLength(0); row++)
            {
                for (int col = 0; col < State.CurrentFigure.GetLength(1); col++)
                {
                    if (State.CurrentFigure[row, col])
                    {
                        State.TetrisField[State.CurrentFigureRow + row, State.CurrentFigureCol + col] = true;
                    }
                }
            }
        }

        static bool Collision(bool[,] figure)
        {
            if (State.CurrentFigureCol > TetrisCols - figure.GetLength(1))
            {
                return true;
            }

            if (State.CurrentFigureRow + figure.GetLength(0) == TetrisRows)
            {
                return true;
            }

            for (int row = 0; row < figure.GetLength(0); row++)
            {
                for (int col = 0; col < figure.GetLength(1); col++)
                {
                    if (figure[row, col] &&
                        State.TetrisField[State.CurrentFigureRow + row + 1, State.CurrentFigureCol + col])
                    {
                        return true;
                    }

                }
            }

            return false;
        }

        static void DrawBorder()
        {
            Console.SetCursorPosition(0, 0);
            string line = "╔";
            line += new string('═', TetrisCols);
            /* for (int i = 0; i < TetrisCols; i++)
            {
                line += "═";
            } */

            line += "╦";
            line += new string('═', InfoCols);
            line += "╗";
            Console.Write(line);

            for (int i = 0; i < TetrisRows; i++)
            {
                string middleLine = "║";
                middleLine += new string(' ', TetrisCols);
                middleLine += "║";
                middleLine += new string(' ', InfoCols);
                middleLine += "║";
                Console.Write(middleLine);
            }

            string endLine = "╚";
            endLine += new string('═', TetrisCols);
            endLine += "╩";
            endLine += new string('═', InfoCols);
            endLine += "╝";
            Console.Write(endLine);
        }

        static void DrawInfo()
        {
            // TODO: Move to score?
            if (State.Score > State.HighScore)
            {
                State.HighScore = State.Score;
            }

            Write("Level:", 1, 3 + TetrisCols);
            Write(State.Level.ToString(), 2, 3 + TetrisCols);
            Write("Score:", 4, 3 + TetrisCols);
            Write(State.Score.ToString(), 5, 3 + TetrisCols);
            Write("Best:", 7, 3 + TetrisCols);
            Write(State.HighScore.ToString(), 8, 3 + TetrisCols);
            Write("Frame:", 10, 3 + TetrisCols);
            Write(State.Frame.ToString() + " / " + (State.FramesToMoveFigure - State.Level).ToString(), 11, 3 + TetrisCols);
            Write("Position:", 13, 3 + TetrisCols);
            Write($"{State.CurrentFigureRow}, {State.CurrentFigureCol}", 14, 3 + TetrisCols);
            Write("Keys:", 16, 3 + TetrisCols);
            Write($"  ^ ", 18, 3 + TetrisCols);
            Write($"<   > ", 19, 3 + TetrisCols);
            Write($"  v  ", 20, 3 + TetrisCols);
        }

        static void DrawTetrisField()
        {
            for (int row = 0; row < State.TetrisField.GetLength(0); row++)
            {
                string line = "";
                for (int col = 0; col < State.TetrisField.GetLength(1); col++)
                {
                    if (State.TetrisField[row, col])
                    {
                        line += "*";
                    }
                    else
                    {
                        line += " ";
                    }
                }

                Write(line, row + 1, 1);
            }
        }

        static void DrawCurrentFigure()
        {
            for (int row = 0; row < State.CurrentFigure.GetLength(0); row++)
            {
                for (int col = 0; col < State.CurrentFigure.GetLength(1); col++)
                {
                    if (State.CurrentFigure[row, col])
                    {
                        Write("*", row + 1 + State.CurrentFigureRow, 1 + State.CurrentFigureCol + col);
                    }
                }
            }
        }

        static void Write(string text, int row, int col)
        {
            Console.SetCursorPosition(col, row);
            Console.Write(text);
        }

        static bool CheckForLeftCollision(bool [,] figure)
        {

            for (int row = 0; row < figure.GetLength(0); row++)
            {
                for (int col = 0; col < figure.GetLength(1); col++)
                {
                    if (figure[row, col] &&
                        State.TetrisField[State.CurrentFigureRow + row, State.CurrentFigureCol + col-1])
                    {
                        return true;
                    }

                }
            }

            return false;
        }

        static bool CheckForRightCollision(bool[,] figure)
        {

            for (int row = 0; row < figure.GetLength(0); row++)
            {
                for (int col = 0; col < figure.GetLength(1); col++)
                {
                    if (figure[row, col] &&
                        State.TetrisField[State.CurrentFigureRow + row, State.CurrentFigureCol + col + 1])
                    {
                        return true;
                    }

                }
            }

            return false;
        }
    }
}
