namespace Game.Features.Tetris
{
    public static class TetrisScoreLogic
    {
        public static int CalculateLineScore(int lineCount)
        {
            switch (lineCount)
            {
                case 1: return TetrisConstants.SCORE_LINE_1;
                case 2: return TetrisConstants.SCORE_LINE_2;
                case 3: return TetrisConstants.SCORE_LINE_3;
                case 4: return TetrisConstants.SCORE_LINE_4;
                default: return 0;
            }
        }

        public static int CalculateLevel(int lines)
        {
            if (lines <= 5)
            {
                return 1;
            }
            return 2 + (lines - 6) / 5;
        }

        public static float CalculateDropInterval(int level, TetrisGameSetting gameSetting)
        {
            var levelSteps = level <= 1 ? 0 : level - 1;
            var interval = gameSetting.InitialDropInterval - levelSteps * gameSetting.DropIntervalDecrease;
            return interval < gameSetting.MinDropInterval ? gameSetting.MinDropInterval : interval;
        }
    }
}
