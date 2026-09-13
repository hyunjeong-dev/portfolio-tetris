using UnityEngine;

namespace Game.Features.Tetris
{
    public static class TetrisBlockLogic
    {
        static readonly Vector2Int[] _kickOffsets =
        {
            new Vector2Int(0, 0),
            new Vector2Int(-1, 0),
            new Vector2Int(1, 0),
            new Vector2Int(-2, 0),
            new Vector2Int(2, 0),
            new Vector2Int(0, 1),
            new Vector2Int(0, -1),
        };

        public static Vector2Int[] KickOffsets => _kickOffsets;

        public static int NextRotation(int current, int direction)
        {
            var rotationCount = TetrisConstants.ROTATION_COUNT;
            var nextRotation = current + direction;
            return ((nextRotation % rotationCount) + rotationCount) % rotationCount;
        }

        public static Vector2Int GetSpawnPosition(int boardWidth, int boardHeight)
        {
            return new Vector2Int(boardWidth / 2, boardHeight - 1);
        }
    }
}
