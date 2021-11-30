using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe;
using TicTacToe.Players;
using Index = TicTacToe.Index;

namespace TicTacToeTests
{
    [TestFixture]
    public class TicTacToeGameTests
    {
        [Test]
        public void PlayShouldReturnValue()
        {
            var player = new Mock<IPlayer>();
            player.Setup(x => x.Play(It.IsAny<Board>(), It.IsAny<Symbol>()))
                .Returns((Board x, Symbol s) =>
                {
                    return x.GetEmptyPositions().First();
                });

            var game = new TicTacToeGame(player.Object, player.Object);
        }

        [Test]
        public void PlayShouldReturnCorrectWinner()
        {
            int player1CurrentCol = 0;
            int player2CurrentCol = 0;
            var player1 = new Mock<IPlayer>();
            player1.Setup(x => x.Play(It.IsAny<Board>(), It.IsAny<Symbol>()))
                .Returns((Board x, Symbol s) => new Index(0, player1CurrentCol++));
            var player2 = new Mock<IPlayer>();
            player2.Setup(x => x.Play(It.IsAny<Board>(), It.IsAny<Symbol>()))
                .Returns((Board x, Symbol s) => new Index(1, player2CurrentCol++));

            var game = new TicTacToeGame(player1.Object, player2.Object);
            var winner = game.Play();
            Assert.AreEqual(Symbol.X, winner);
        }
        
        // TODO: Add more tests
    }
}
