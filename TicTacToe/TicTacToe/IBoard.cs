using System.Collections.Generic;

namespace TicTacToe
{
    public interface IBoard
    {
        bool IsFull();

        void PlaceSymbol(Index index, Symbol symbol);

        IEnumerable<Index> GetEmptyPositions();

        Symbol GetRowSymbol(int row);

        Symbol GetColumnSymbol(int column);

        Symbol GetDiagonalTLBRSymbol();

        Symbol GetDiagonalTRBLSymbol();
    }
}
