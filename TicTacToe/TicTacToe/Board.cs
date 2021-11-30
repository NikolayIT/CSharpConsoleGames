using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe
{
    public class Board : IBoard
    {
        private Symbol[,] board;

        public Board()
            : this(3, 3)
        {
        }

        public Board(int rows, int columns)
        {
            if (rows != columns)
            {
                throw new ArgumentException("Rows should be equal to columns");
            }

            Rows = rows;
            Columns = columns;
            board = new Symbol[rows, columns];
        }

        public int Rows { get; }

        public int Columns { get; }

        public Symbol[,] BoardState => this.board;

        public Symbol GetRowSymbol(int row)
        {
            var symbol = this.board[row, 0];
            if (symbol == Symbol.None)
            {
                return Symbol.None;
            }

            for (int col = 1; col < this.Columns; col++)
            {
                if (this.board[row, col] != symbol)
                {
                    return Symbol.None;
                }
            }

            return symbol;
        }

        public Symbol GetColumnSymbol(int column)
        {
            var symbol = this.board[0, column];
            if (symbol == Symbol.None)
            {
                return Symbol.None;
            }

            for (int row = 1; row < this.Rows; row++)
            {
                if (this.board[row, column] != symbol)
                {
                    return Symbol.None;
                }
            }

            return symbol;
        }

        public Symbol GetDiagonalTLBRSymbol()
        {
            var symbol = this.board[0, 0];
            if (symbol == Symbol.None)
            {
                return symbol;
            }

            for (int i = 1; i < this.Rows; i++)
            {
                if (this.board[i, i] != symbol)
                {
                    return Symbol.None;
                }
            }

            return symbol;
        }

        public Symbol GetDiagonalTRBLSymbol()
        {
            var symbol = this.board[0, this.Rows - 1];
            if (symbol == Symbol.None)
            {
                return Symbol.None;
            }

            for (int row = 1; row < this.Rows; row++)
            {
                if (this.board[row, this.Rows - row - 1] != symbol)
                {
                    return Symbol.None;
                }
            }

            return symbol;
        }

        public IEnumerable<Index> GetEmptyPositions()
        {
            var positions = new List<Index>();
            for (int i = 0; i < this.Rows; i++)
            {
                for (int j = 0; j < this.Columns; j++)
                {
                    if (this.board[i, j] == Symbol.None)
                    {
                        positions.Add(new Index(i, j));
                    }
                }
            }

            return positions;
        }

        public bool IsFull()
        {
            for (int i = 0; i < this.Rows; i++)
            {
                for (int j = 0; j < this.Columns; j++)
                {
                    if (this.board[i, j] == Symbol.None)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public void PlaceSymbol(Index index, Symbol symbol)
        {
            if (index.Row < 0 || index.Column < 0
                || index.Row >= this.Rows || index.Column >= this.Columns)
            {
                throw new IndexOutOfRangeException("Index is out of range.");
            }

            this.board[index.Row, index.Column] = symbol;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < this.Rows; i++)
            {
                for (int j = 0; j < this.Columns; j++)
                {
                    if (this.board[i, j] == Symbol.None)
                    {
                        sb.Append('.');
                    }
                    else
                    {
                        sb.Append(this.board[i, j]);
                    }
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
