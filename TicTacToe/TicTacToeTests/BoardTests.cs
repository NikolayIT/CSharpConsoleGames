using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe;

namespace TicTacToeTests
{
    [TestFixture]
    public class BoardTests
    {
        [Test]
        public void IsFullShouldReturnTrueForFullBoard()
        {
            var board = new Board(3,3);
            Assert.IsFalse(board.IsFull());

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Assert.IsFalse(board.IsFull());
                    board.PlaceSymbol(new TicTacToe.Index(i, j), Symbol.X);
                }
            }

            Assert.IsTrue(board.IsFull());
        }

        [Test]
        public void GetRowSymbolShouldWorkCorrectly()
        {
            var board = new Board(4, 4);

            for (int i = 0; i < board.Columns; i++)
            {
                Assert.AreEqual(Symbol.None, board.GetRowSymbol(2));
                board.PlaceSymbol(
                    new TicTacToe.Index(2, i),
                    Symbol.O);
            }

            Assert.AreEqual(Symbol.O, board.GetRowSymbol(2));
        }

        [TestCase(Symbol.X)]
        [TestCase(Symbol.O)]
        public void GetColumnSymbolShouldWorkCorrectly(Symbol symbol)
        {
            var board = new Board(5, 5);

            for (int i = 0; i < board.Rows; i++)
            {
                Assert.AreEqual(Symbol.None, board.GetColumnSymbol(2));
                board.PlaceSymbol(
                    new TicTacToe.Index(i, 1),
                    symbol);
            }

            Assert.AreEqual(symbol, board.GetColumnSymbol(1));
        }

        // TODO: Test GetDiagonalTLBRSymbol()
        // TODO: Test GetDiagonalTRBLSymbol()
        // TODO: Test ToString()

        [Test]
        public void GetEmptyPositionsShouldReturnAllPositionsForEmptyBoard()
        {
            var board = new Board(3, 3);
            var positions = board.GetEmptyPositions();
            Assert.AreEqual(3 * 3, positions.Count());
        }

        [Test]
        public void GetEmptyPositionsShouldReturnCorrectNumberOfPositions()
        {
            var board = new Board(4, 4);
            board.PlaceSymbol(new TicTacToe.Index(1, 1), Symbol.X);
            board.PlaceSymbol(new TicTacToe.Index(2, 2), Symbol.O);
            var positions = board.GetEmptyPositions();
            Assert.AreEqual(4*4-2, positions.Count());
        }
    }
}
