using UnityEngine;

namespace Game.Features.Tetris
{
    public class TetrisBoardModule : TetrisModule
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int TotalHeight { get; private set; }

        public int[,] BoardRef => _board;
        public bool[] ScratchClearedLines => _scratchClearedLines;
        public int[] ScratchClearedLineIndices => _scratchClearedLineIndices;

        int[,] _board;
        bool[] _scratchClearedLines;
        int[] _scratchClearedLineIndices;

        protected override void OnRegister()
        {
            var config = Context.Data.GameSetting;
            Width = config.BoardWidth;
            Height = config.BoardHeight;
            TotalHeight = config.BoardTotalHeight;
            _board = new int[Width, TotalHeight];
            _scratchClearedLines = new bool[Height];
            _scratchClearedLineIndices = new int[TetrisConstants.LINE_CLEAR_SCORE_MAX_COUNT];
        }

        public void Clear()
        {
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < TotalHeight; y++)
                {
                    _board[x, y] = 0;
                }
            }
        }

        public override void Reset()
        {
            Clear();
        }

        public bool IsOccupied(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= TotalHeight)
            {
                return true;
            }
            return _board[x, y] != 0;
        }

        public int GetCell(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= TotalHeight)
            {
                return 0;
            }
            return _board[x, y];
        }

        public bool CanPlace(Vector2Int[] cells, Vector2Int position)
        {
            return TetrisBoardLogic.CanPlace(_board, Width, TotalHeight, cells, position);
        }

        public Vector2Int CalculateDropPosition(Vector2Int[] cells, Vector2Int startPosition)
        {
            return TetrisBoardLogic.CalculateDropPosition(_board, Width, TotalHeight, cells, startPosition);
        }

        public void Place(Vector2Int[] cells, Vector2Int position, int value)
        {
            foreach (var cell in cells)
            {
                var x = position.x + cell.x;
                var y = position.y + cell.y;
                if (x < 0 || x >= Width || y < 0 || y >= TotalHeight)
                {
                    continue;
                }
                _board[x, y] = value;
            }
        }

        public int MarkFullLines()
        {
            for (var y = 0; y < Height; y++)
            {
                _scratchClearedLines[y] = false;
            }

            var count = TetrisBoardLogic.FindFullLines(_board, Width, Height, _scratchClearedLineIndices);
            var marked = count < _scratchClearedLineIndices.Length ? count : _scratchClearedLineIndices.Length;
            for (var i = 0; i < marked; i++)
            {
                _scratchClearedLines[_scratchClearedLineIndices[i]] = true;
            }

            return count;
        }

        public void CollapseMarkedLines()
        {
            TetrisBoardLogic.CollapseLines(_board, Width, TotalHeight, _scratchClearedLines, Height);
        }
    }
}
