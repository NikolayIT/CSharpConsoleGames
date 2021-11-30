using System;
using TicTacToe.Players;

namespace TicTacToe
{
    class Program
    {
        // TODO: Add test to verify that random never win vs AI
        // TODO: Write stats to a file and show it
        // TODO: Use simple factory for the Game class based on the user input
        static void Main(string[] args)
        {
            Console.Title = "TicTacToe 1.0";
            while (true)
            {
                Console.Clear();
                Console.WriteLine("==== TicTacToe 1.0 ====");
                Console.WriteLine("1. Player vs Player");
                Console.WriteLine("2. Player vs Random");
                Console.WriteLine("3. Random vs Player");
                Console.WriteLine("4. Player vs AI");
                Console.WriteLine("5. AI vs Player");
                Console.WriteLine("6. Simulate Random vs Random");
                Console.WriteLine("7. Simulate AI vs Random");
                Console.WriteLine("8. Simulate Random vs AI");
                Console.WriteLine("9. Simulate AI vs AI");
                Console.WriteLine("0. Exit");

                while (true)
                {
                    Console.Write("Please enter number [0-9]: ");
                    var line = Console.ReadLine();
                    if (line == "0")
                    {
                        return;
                    }
                    if (line == "1")
                    {
                        PlayGame(new ConsolePlayer(), new ConsolePlayer());
                        break;
                    }
                    if (line == "2")
                    {
                        PlayGame(new ConsolePlayer(), new RandomPlayer());
                        break;
                    }
                    if (line == "3")
                    {
                        PlayGame(new RandomPlayer(), new ConsolePlayer());
                        break;
                    }
                    if (line == "4")
                    {
                        PlayGame(new ConsolePlayer(), new AiPlayer());
                        break;
                    }
                    if (line == "5")
                    {
                        PlayGame(new AiPlayer(), new ConsolePlayer());
                        break;
                    }
                    if (line == "6")
                    {
                        Simulate(new RandomPlayer(), new RandomPlayer(), 10000);
                        break;
                    }
                    if (line == "7")
                    {
                        Simulate(new AiPlayer(), new RandomPlayer(), 10);
                        break;
                    }
                    if (line == "8")
                    {
                        Simulate(new RandomPlayer(), new AiPlayer(), 10);
                        break;
                    }
                    if (line == "9")
                    {
                        Simulate(new AiPlayer(), new AiPlayer(), 10);
                        break;
                    }
                }

                Console.Write("Press [enter] to continue...");
                Console.ReadLine();
            }
        }

        static void Simulate(IPlayer player1, IPlayer player2, int count)
        {
            int x = 0, o = 0, draw = 0;
            int firstWinner = 0, secondWinner = 0;
            var first = player1;
            var second = player2;
            for (int i = 0; i < count; i++)
            {
                var game = new TicTacToeGame(first, second);
                var result = game.Play();
                if (result.Winner == Symbol.X && first == player1) firstWinner++;
                if (result.Winner == Symbol.O && first == player1) secondWinner++;
                if (result.Winner == Symbol.X && first == player2) secondWinner++;
                if (result.Winner == Symbol.O && first == player2) firstWinner++;
                if (result.Winner == Symbol.X) x++;
                if (result.Winner == Symbol.O) o++;
                if (result.Winner == Symbol.None) draw++;
                (first, second) = (second, first);
            }

            Console.WriteLine("Games played: " + count);
            Console.WriteLine("Games won by X: " + x);
            Console.WriteLine("Games won by O: " + o);
            Console.WriteLine("Draw games: " + draw);
            Console.WriteLine(player1.GetType().Name + " won games: " + firstWinner);
            Console.WriteLine(player2.GetType().Name + " won games: " + secondWinner);
        }

        static void PlayGame(IPlayer player1, IPlayer player2)
        {
            var game = new TicTacToeGame(player1, player2);
            var result = game.Play();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Game over!");
            if (result.Winner == Symbol.None)
            {
                Console.WriteLine("Winner: Draw");
            }
            else
            {
                Console.WriteLine("Winner: " + result.Winner);
            }

            Console.WriteLine(result.Board.ToString());
            Console.ResetColor();
        }
    }
}
