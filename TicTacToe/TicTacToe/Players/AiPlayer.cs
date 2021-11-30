using System;

namespace TicTacToe.Players
{
    // TODO: Set depth limit
    public class AiPlayer : IPlayer
    {
        public AiPlayer()
        {
            this.WinnerLogic = new GameWinnerLogic();
        }

        public GameWinnerLogic WinnerLogic { get; }

        public Index Play(Board board, Symbol symbol)
        {
            // Instead best move -> best moveS
            Index bestMove = null;
            int bestMoveValue = -1000;
            var moves = board.GetEmptyPositions();
            foreach (var move in moves)
            {
                board.PlaceSymbol(move, symbol);
                var value = MiniMax(board, symbol, symbol == Symbol.X ? Symbol.O : Symbol.X);
                board.PlaceSymbol(move, Symbol.None);
                if (value > bestMoveValue)
                {
                    bestMove = move;
                    bestMoveValue = value;
                }

                // if (value == bestMoveValue) add to list
            }

            return bestMove;
        }

        private int MiniMax(Board board, Symbol player, Symbol currentPlayer)
        {
            if (this.WinnerLogic.IsGameOver(board))
            {
                var winner = this.WinnerLogic.GetWinner(board);
                if (winner == player) return 1;
                else if (winner == Symbol.None) return 0;
                else return -1;
            }

            var bestValue = player == currentPlayer ? -100 : 100; // TODO:
            var options = board.GetEmptyPositions();
            foreach (var option in options)
            {
                board.PlaceSymbol(option, currentPlayer);
                var value = MiniMax(board, player, currentPlayer == Symbol.O ? Symbol.X : Symbol.O);
                board.PlaceSymbol(option, Symbol.None);
                bestValue =
                    currentPlayer == player ?
                        Math.Max(bestValue, value) :
                        Math.Min(bestValue, value);
            }

            return bestValue;
        }
    }
}
