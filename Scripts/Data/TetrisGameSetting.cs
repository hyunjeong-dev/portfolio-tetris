using UnityEngine;

namespace Game.Features.Tetris
{
    [CreateAssetMenu(fileName = "TetrisGameSetting", menuName = "Tetris/Game Setting", order = 0)]
    public class TetrisGameSetting : ScriptableObject
    {
        [Header("Board")]
        [Min(4)] public int BoardWidth = TetrisConstants.BOARD_WIDTH;
        [Min(8)] public int BoardHeight = TetrisConstants.BOARD_HEIGHT;
        [Min(0)] public int BoardSpawnBuffer = TetrisConstants.BOARD_SPAWN_BUFFER;
        [Min(0.01f)] public float CellSize = TetrisConstants.CELL_SIZE;

        [Header("Falling Speed")]
        [Min(0.05f)] public float InitialDropInterval = TetrisConstants.INITIAL_DROP_INTERVAL;
        [Min(0.01f)] public float MinDropInterval = TetrisConstants.MIN_DROP_INTERVAL;
        [Min(0f)] public float DropIntervalDecrease = TetrisConstants.DROP_INTERVAL_DECREASE;

        [Header("Line Clear Animation")]
        [Min(0f)] public float LineClearAnimDuration = TetrisConstants.LINE_CLEAR_ANIM_DURATION;
        [Range(0f, 0.5f)] public float GhostAlpha = 0.35f;

        [Header("Sprites")]
        public Sprite CellSprite;
        public Sprite GhostSprite;

        public int BoardTotalHeight => BoardHeight + BoardSpawnBuffer;
    }
}
