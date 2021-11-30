namespace TicTacToe.Players
{
    public interface IPlayer
    {
        Index Play(Board board, Symbol symbol);
    }
}
