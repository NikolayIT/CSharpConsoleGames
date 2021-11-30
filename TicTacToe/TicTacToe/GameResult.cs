using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public class GameResult
    {
        public GameResult(Symbol winner, Board board)
        {
            Winner = winner;
            Board = board;
        }

        public Symbol Winner { get; }
        public Board Board { get; }
    }
}
