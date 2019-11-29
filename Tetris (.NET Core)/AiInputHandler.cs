using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris
{
    public class AiInputHandler : IInputHandler
    {
        private readonly TetrisGame game;

        public AiInputHandler(TetrisGame game)
        {
            this.game = game;
        }

        public TetrisGameInput GetInput()
        {
            return TetrisGameInput.Down;
        }
    }
}
