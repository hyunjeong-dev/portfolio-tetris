using UnityEngine;

namespace Game.Features.Tetris
{
    public class TetrisBlock
    {
        public TetrisBlockType Type { get; set; }
        public Vector2Int Position { get; set; }
        public int Rotation { get; set; }

        public Vector2Int[] GetCells() => TetrisBlockData.GetCells(Type, Rotation);
        public Color GetColor() => TetrisBlockData.GetColor(Type);

        public void Set(TetrisBlockType type, Vector2Int position, int rotation)
        {
            Type = type;
            Position = position;
            Rotation = rotation;
        }

        public void CopyFrom(TetrisBlock other)
        {
            Type = other.Type;
            Position = other.Position;
            Rotation = other.Rotation;
        }
    }
}
