using System;
using TicTacToe.Players;

namespace TicTacToe
{
    public class TicTacToeGame
    {
        public TicTacToeGame(IPlayer firstPlayer, IPlayer secondPlayer)
        {
            FirstPlayer = firstPlayer;
            SecondPlayer = secondPlayer;
            this.WinnerLogic = new GameWinnerLogic();
        }

        public IPlayer FirstPlayer { get; }
        public IPlayer SecondPlayer { get; }
        public GameWinnerLogic WinnerLogic { get; }

        public GameResult Play()
        {
            Board board = new Board();
            IPlayer currentPlayer = this.FirstPlayer;
            Symbol symbol = Symbol.X;
            while (!this.WinnerLogic.IsGameOver(board))
            {
                // Play round
                var move = currentPlayer.Play(board, symbol);
                board.PlaceSymbol(move, symbol);

                if (currentPlayer == this.FirstPlayer)
                {
                    currentPlayer = this.SecondPlayer;
                    symbol = Symbol.O;
                }
                else
                {
                    currentPlayer = this.FirstPlayer;
                    symbol = Symbol.X;
                }
            }

            var winner = this.WinnerLogic.GetWinner(board);
            return new GameResult(winner, board);
        }
    }
}
