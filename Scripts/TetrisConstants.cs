namespace Game.Features.Tetris
{
    public static class TetrisConstants
    {
        public const int BOARD_WIDTH = 10;
        public const int BOARD_HEIGHT = 20;
        public const int BOARD_SPAWN_BUFFER = 2;
        public const int BOARD_TOTAL_HEIGHT = BOARD_HEIGHT + BOARD_SPAWN_BUFFER;
        public const float CELL_SIZE = 1f;

        public const float INITIAL_DROP_INTERVAL = 1f;
        public const float MIN_DROP_INTERVAL = 0.15f;
        public const float DROP_INTERVAL_DECREASE = 0.08f;

        public const int SCORE_LINE_1 = 100;
        public const int SCORE_LINE_2 = 300;
        public const int SCORE_LINE_3 = 500;
        public const int SCORE_LINE_4 = 800;

        public const int LINE_CLEAR_SCORE_MAX_COUNT = 4;

        public const float LINE_CLEAR_ANIM_DURATION = 0.35f;
        public const float LINE_CLEAR_BLINK_INTERVAL = 0.06f;

        public const int CURRENT_BLOCK_CELL_COUNT = 4;
        public const int ROTATION_COUNT = 4;
        public const int BLOCK_TYPE_COUNT = 7;

        public const float MOVE_REPEAT_INITIAL_DELAY = 0.18f;
        public const float MOVE_REPEAT_INTERVAL = 0.06f;
        public const float SOFT_DROP_REPEAT_INTERVAL = 0.05f;
    }
}
