using System;

namespace Tetris
{
    public class ConsoleInputHandler : IInputHandler
    {
        public TetrisGameInput GetInput()
        {
            // Read user input
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                {
                    return TetrisGameInput.Exit;
                }
                if (key.Key == ConsoleKey.LeftArrow || key.Key == ConsoleKey.A)
                {
                    return TetrisGameInput.Left;
                }
                if (key.Key == ConsoleKey.RightArrow || key.Key == ConsoleKey.D)
                {
                    return TetrisGameInput.Right;
                }
                if (key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.S)
                {
                    return TetrisGameInput.Down;
                }
                if (key.Key == ConsoleKey.Spacebar || key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.W)
                {
                    return TetrisGameInput.Rotate;
                }
            }

            return TetrisGameInput.None;
        }
    }
}
