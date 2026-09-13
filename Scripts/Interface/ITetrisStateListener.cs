namespace Game.Features.Tetris
{
    public interface ITetrisStateListener
    {
        void OnStateChanged(TetrisStatus next);
    }
}
