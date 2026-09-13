using UnityEngine;

namespace Game.Features.Tetris
{
    public enum TetrisBlockType
    {
        I = 0,
        J = 1,
        L = 2,
        O = 3,
        S = 4,
        Z = 5,
        T = 6,
    }

    public static class TetrisBlockData
    {
        public const int TYPE_COUNT = TetrisConstants.BLOCK_TYPE_COUNT;

        static readonly Vector2Int[][][] _shapes;
        static readonly Color[] _colors;

        static TetrisBlockData()
        {
            _shapes = new Vector2Int[TYPE_COUNT][][];

            _shapes[(int)TetrisBlockType.I] = new[]
            {
                new[] { new Vector2Int(-1, 0), new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0) },
                new[] { new Vector2Int(1, 1), new Vector2Int(1, 0), new Vector2Int(1, -1), new Vector2Int(1, -2) },
                new[] { new Vector2Int(-1, -1), new Vector2Int(0, -1), new Vector2Int(1, -1), new Vector2Int(2, -1) },
                new[] { new Vector2Int(0, 1), new Vector2Int(0, 0), new Vector2Int(0, -1), new Vector2Int(0, -2) },
            };

            _shapes[(int)TetrisBlockType.J] = new[]
            {
                new[] { new Vector2Int(-1, 1), new Vector2Int(-1, 0), new Vector2Int(0, 0), new Vector2Int(1, 0) },
                new[] { new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(0, 0), new Vector2Int(0, -1) },
                new[] { new Vector2Int(-1, 0), new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(1, -1) },
                new[] { new Vector2Int(0, 1), new Vector2Int(0, 0), new Vector2Int(0, -1), new Vector2Int(-1, -1) },
            };

            _shapes[(int)TetrisBlockType.L] = new[]
            {
                new[] { new Vector2Int(-1, 0), new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(1, 1) },
                new[] { new Vector2Int(0, 1), new Vector2Int(0, 0), new Vector2Int(0, -1), new Vector2Int(1, -1) },
                new[] { new Vector2Int(-1, -1), new Vector2Int(-1, 0), new Vector2Int(0, 0), new Vector2Int(1, 0) },
                new[] { new Vector2Int(-1, 1), new Vector2Int(0, 1), new Vector2Int(0, 0), new Vector2Int(0, -1) },
            };

            var oCells = new[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(1, 1) };
            _shapes[(int)TetrisBlockType.O] = new[] { oCells, oCells, oCells, oCells };

            _shapes[(int)TetrisBlockType.S] = new[]
            {
                new[] { new Vector2Int(-1, 0), new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(1, 1) },
                new[] { new Vector2Int(0, 1), new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(1, -1) },
                new[] { new Vector2Int(-1, -1), new Vector2Int(0, -1), new Vector2Int(0, 0), new Vector2Int(1, 0) },
                new[] { new Vector2Int(-1, 1), new Vector2Int(-1, 0), new Vector2Int(0, 0), new Vector2Int(0, -1) },
            };

            _shapes[(int)TetrisBlockType.Z] = new[]
            {
                new[] { new Vector2Int(-1, 1), new Vector2Int(0, 1), new Vector2Int(0, 0), new Vector2Int(1, 0) },
                new[] { new Vector2Int(1, 1), new Vector2Int(1, 0), new Vector2Int(0, 0), new Vector2Int(0, -1) },
                new[] { new Vector2Int(-1, 0), new Vector2Int(0, 0), new Vector2Int(0, -1), new Vector2Int(1, -1) },
                new[] { new Vector2Int(0, 1), new Vector2Int(-1, 0), new Vector2Int(0, 0), new Vector2Int(-1, -1) },
            };

            _shapes[(int)TetrisBlockType.T] = new[]
            {
                new[] { new Vector2Int(-1, 0), new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(0, 1) },
                new[] { new Vector2Int(0, 1), new Vector2Int(0, 0), new Vector2Int(0, -1), new Vector2Int(1, 0) },
                new[] { new Vector2Int(-1, 0), new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(0, -1) },
                new[] { new Vector2Int(0, 1), new Vector2Int(0, 0), new Vector2Int(0, -1), new Vector2Int(-1, 0) },
            };

            _colors = new Color[TYPE_COUNT];
            _colors[(int)TetrisBlockType.I] = new Color(0.30f, 0.85f, 0.95f);
            _colors[(int)TetrisBlockType.J] = new Color(0.25f, 0.40f, 0.95f);
            _colors[(int)TetrisBlockType.L] = new Color(0.95f, 0.55f, 0.20f);
            _colors[(int)TetrisBlockType.O] = new Color(0.95f, 0.85f, 0.25f);
            _colors[(int)TetrisBlockType.S] = new Color(0.30f, 0.85f, 0.30f);
            _colors[(int)TetrisBlockType.Z] = new Color(0.95f, 0.30f, 0.30f);
            _colors[(int)TetrisBlockType.T] = new Color(0.70f, 0.35f, 0.85f);
        }

        public static Vector2Int[] GetCells(TetrisBlockType type, int rotation)
        {
            var rotationCount = TetrisConstants.ROTATION_COUNT;
            var normalizedRotation = ((rotation % rotationCount) + rotationCount) % rotationCount;
            return _shapes[(int)type][normalizedRotation];
        }

        public static Color GetColor(TetrisBlockType type) => _colors[(int)type];
    }
}
