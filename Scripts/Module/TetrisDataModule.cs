using UnityEngine;

namespace Game.Features.Tetris
{
    public class TetrisDataModule : TetrisModule
    {
        public TetrisGameSetting GameSetting { get; private set; }
        public TetrisScoreData Score { get; private set; }
        public TetrisBlock CurrentBlock { get; private set; }
        public TetrisBlock NextBlock { get; private set; }
        public Vector2Int CurrentGhostPosition { get; set; }
        public float CurrentDropInterval => TetrisScoreLogic.CalculateDropInterval(Score.Level, GameSetting);

        protected override void OnRegister()
        {
            GameSetting = Context.GameSetting;
            Score = new();
            CurrentBlock = new();
            NextBlock = new();
        }

        public override void Reset()
        {
            Score.Reset();
            CurrentGhostPosition = default;
        }

        public void AddScore(int amount)
        {
            if (amount <= 0)
            {
                return;
            }
            Score.Score += amount;
        }

        public void AddLines(int count)
        {
            if (count <= 0)
            {
                return;
            }
            Score.Lines += count;
            Score.Level = TetrisScoreLogic.CalculateLevel(Score.Lines);
        }

    }
}
