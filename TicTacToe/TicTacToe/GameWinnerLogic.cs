using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public class GameWinnerLogic
    {
        public bool IsGameOver(Board board)
        {
            for (int row = 0; row < board.Rows; row++)
            {
                if (board.GetRowSymbol(row) != Symbol.None)
                {
                    return true;
                }
            }

            for (int col = 0; col < board.Columns; col++)
            {
                if (board.GetColumnSymbol(col) != Symbol.None)
                {
                    return true;
                }
            }

            if (board.GetDiagonalTLBRSymbol() != Symbol.None)
            {
                return true;
            }

            if (board.GetDiagonalTRBLSymbol() != Symbol.None)
            {
                return true;
            }

            return board.IsFull();
        }

        public Symbol GetWinner(Board board)
        {
            for (int row = 0; row < board.Rows; row++)
            {
                var winner = board.GetRowSymbol(row);
                if (winner != Symbol.None)
                {
                    return winner;
                }
            }

            for (int col = 0; col < board.Columns; col++)
            {
                var winner = board.GetColumnSymbol(col);
                if (winner != Symbol.None)
                {
                    return winner;
                }
            }

            var diagonal1 = board.GetDiagonalTLBRSymbol();
            if (diagonal1 != Symbol.None)
            {
                return diagonal1;
            }

            var diagonal2 = board.GetDiagonalTRBLSymbol();
            if (diagonal2 != Symbol.None)
            {
                return diagonal2;
            }

            return Symbol.None;
        }
    }
}
