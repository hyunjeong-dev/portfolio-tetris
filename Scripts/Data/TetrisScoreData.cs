namespace Game.Features.Tetris
{
    public class TetrisScoreData
    {
        public int Score { get; set; }
        public int Lines { get; set; }
        public int Level { get; set; }
        public float ElapsedTime { get; set; }

        public void Reset()
        {
            Score = 0;
            Lines = 0;
            Level = 1;
            ElapsedTime = 0f;
        }
    }
}
