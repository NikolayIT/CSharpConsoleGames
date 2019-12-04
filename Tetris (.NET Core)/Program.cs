namespace Tetris
{
    // TODO: When we have collision during moving (left/right) we have unusual behaviour
    public static class Program
    {
        public static void Main()
        {
            new MusicPlayer().Play();
            int TetrisRows = 20;
            int TetrisCols = 10;
            var gameManager = new TetrisGameManager(
                new TetrisGame(TetrisRows, TetrisCols),
                new ConsoleInputHandler(),
                new TetrisConsoleWriter(TetrisRows, TetrisCols, '▓'),
                new ScoreManager("scores.txt"));
            gameManager.MainLoop();
        }
    }
}
