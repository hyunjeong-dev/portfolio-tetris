using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Features.Tetris
{
    public class TetrisObjectModule : TetrisModule
    {
        TetrisBoardEntity _boardEntity;
        TetrisBlockEntity _currentBlockEntity;
        TetrisBlockEntity _ghostBlockEntity;

        TetrisCellPool _cellPool;
        SpriteRenderer[,] _placedCells;
        bool _subscribed;

        protected override void OnRegister()
        {
            _boardEntity = Context.BoardEntity;
            _currentBlockEntity = Context.CurrentBlockEntity;
            _ghostBlockEntity = Context.GhostBlockEntity;

            var boardWidth = Context.Board.Width;
            var boardHeight = Context.Board.TotalHeight;
            var cellSize = Context.Data.GameSetting.CellSize;

            _boardEntity.FitBackground(Context.Board.Width, Context.Board.Height, cellSize);
            _placedCells = new SpriteRenderer[boardWidth, boardHeight];

            _cellPool = new(_boardEntity, _currentBlockEntity);

            if (_currentBlockEntity != null && _boardEntity.CurrentBlockRoot != null)
            {
                _currentBlockEntity.transform.SetParent(_boardEntity.CurrentBlockRoot, false);
                _currentBlockEntity.SetVisible(false);
            }
            if (_ghostBlockEntity != null && _boardEntity.GhostBlockRoot != null)
            {
                _ghostBlockEntity.transform.SetParent(_boardEntity.GhostBlockRoot, false);
                _ghostBlockEntity.SetVisible(false);
            }
        }

        protected override void OnBegin()
        {
            RegisterBlockEvents();
        }

        protected override void OnEnd()
        {
            UnregisterBlockEvents();
            if (_cellPool != null)
            {
                _cellPool.Dispose();
                _cellPool = null;
            }
        }

        void RegisterBlockEvents()
        {
            if (_subscribed)
            {
                return;
            }
            if (Context == null || Context.Block == null)
            {
                return;
            }
            Context.Block.BlockChanged += OnBlockChanged;
            Context.Block.BlockLocked += OnBlockLocked;
            _subscribed = true;
        }

        void UnregisterBlockEvents()
        {
            if (!_subscribed)
            {
                return;
            }
            if (Context != null && Context.Block != null)
            {
                Context.Block.BlockChanged -= OnBlockChanged;
                Context.Block.BlockLocked -= OnBlockLocked;
            }
            _subscribed = false;
        }

        void OnBlockChanged() => RefreshCurrentBlock();

        void OnBlockLocked()
        {
            var data = Context.Data;
            var block = data.CurrentBlock;
            var cells = block.GetCells();
            var color = block.GetColor();
            var cellSize = data.GameSetting.CellSize;
            var boardWidth = Context.Board.Width;
            var boardHeight = Context.Board.TotalHeight;

            foreach (var cell in cells)
            {
                var x = block.Position.x + cell.x;
                var y = block.Position.y + cell.y;
                if (x < 0 || x >= boardWidth || y < 0 || y >= boardHeight)
                {
                    continue;
                }
                if (_placedCells[x, y] != null)
                {
                    continue;
                }

                var spriteRenderer = _cellPool.Get();
                if (spriteRenderer == null)
                {
                    continue;
                }
                spriteRenderer.transform.localPosition = _boardEntity.CellToLocal(x, y, cellSize);
                spriteRenderer.transform.localScale = Vector3.one;
                spriteRenderer.color = color;
                _placedCells[x, y] = spriteRenderer;
            }
        }

        public void RefreshCurrentBlock()
        {
            if (_currentBlockEntity == null || _ghostBlockEntity == null)
            {
                return;
            }
            if (!Context.Block.HasCurrentBlock)
            {
                _currentBlockEntity.SetVisible(false);
                _ghostBlockEntity.SetVisible(false);
                return;
            }

            var data = Context.Data;
            var block = data.CurrentBlock;
            var cells = block.GetCells();
            var cellSize = data.GameSetting.CellSize;

            _currentBlockEntity.SetVisible(true);
            _currentBlockEntity.SetColor(block.GetColor());
            _currentBlockEntity.transform.localPosition = new Vector3(block.Position.x * cellSize, block.Position.y * cellSize, 0f);
            _currentBlockEntity.UpdateLocalPositions(cells, cellSize);

            var ghostVisible = data.CurrentGhostPosition.y < block.Position.y;
            _ghostBlockEntity.SetVisible(ghostVisible);
            if (ghostVisible)
            {
                var ghostColor = block.GetColor();
                ghostColor.a = data.GameSetting.GhostAlpha;
                _ghostBlockEntity.SetColor(ghostColor);
                _ghostBlockEntity.transform.localPosition = new Vector3(data.CurrentGhostPosition.x * cellSize, data.CurrentGhostPosition.y * cellSize, 0f);
                _ghostBlockEntity.UpdateLocalPositions(cells, cellSize);
            }
        }

        public void ClearAllPlacedCells()
        {
            if (_placedCells == null || _cellPool == null)
            {
                return;
            }
            var width = _placedCells.GetLength(0);
            var height = _placedCells.GetLength(1);
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    if (_placedCells[x, y] != null)
                    {
                        _cellPool.Release(_placedCells[x, y]);
                        _placedCells[x, y] = null;
                    }
                }
            }
        }

        public override void Reset()
        {
            ClearAllPlacedCells();
            RefreshCurrentBlock();
        }

        public async UniTask PlayLineClearAsync(int width, bool[] linesToClear, int playHeight, CancellationToken cancellationToken)
        {
            var duration = Context.Data.GameSetting.LineClearAnimDuration;
            await TetrisLineClearLogic.PlayAsync(_placedCells, width, linesToClear, playHeight, duration, cancellationToken);
        }

        public void CollapseVisualCells(bool[] clearedRows, int playHeight)
        {
            if (_placedCells == null)
            {
                return;
            }
            var width = _placedCells.GetLength(0);
            var totalHeight = _placedCells.GetLength(1);
            var cellSize = Context.Data.GameSetting.CellSize;

            for (var y = 0; y < playHeight; y++)
            {
                if (!clearedRows[y])
                {
                    continue;
                }
                for (var x = 0; x < width; x++)
                {
                    if (_placedCells[x, y] != null)
                    {
                        _cellPool.Release(_placedCells[x, y]);
                        _placedCells[x, y] = null;
                    }
                }
            }

            var writeY = 0;
            for (var readY = 0; readY < totalHeight; readY++)
            {
                var clearedRow = readY < playHeight && clearedRows[readY];
                if (clearedRow)
                {
                    continue;
                }
                if (writeY != readY)
                {
                    for (var x = 0; x < width; x++)
                    {
                        var spriteRenderer = _placedCells[x, readY];
                        _placedCells[x, writeY] = spriteRenderer;
                        _placedCells[x, readY] = null;
                        if (spriteRenderer != null)
                        {
                            spriteRenderer.transform.localPosition = _boardEntity.CellToLocal(x, writeY, cellSize);
                        }
                    }
                }
                writeY++;
            }
        }

    }
}
