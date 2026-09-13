using System;
using UnityEngine;

namespace Game.Features.Tetris
{
    public class TetrisBlockModule : TetrisModule
    {
        public bool HasCurrentBlock => _hasCurrentBlock;
        public bool IsOnGround
        {
            get
            {
                if (!_hasCurrentBlock)
                {
                    return false;
                }

                var block = Context.Data.CurrentBlock;
                var board = Context.Board;
                return !board.CanPlace(block.GetCells(), new Vector2Int(block.Position.x, block.Position.y - 1));
            }
        }

        public bool IsTouchingSpawnBuffer
        {
            get
            {
                if (!_hasCurrentBlock)
                {
                    return false;
                }

                var block = Context.Data.CurrentBlock;
                var board = Context.Board;
                var cells = block.GetCells();
                foreach (var cell in cells)
                {
                    if (block.Position.y + cell.y >= board.Height)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public event Action BlockChanged;
        public event Action BlockLocked;

        System.Random _random;
        readonly int? _seed;
        bool _hasCurrentBlock;

        public TetrisBlockModule(int? seed)
        {
            _seed = seed;
        }

        protected override void OnRegister()
        {
            _random = _seed.HasValue ? new(_seed.Value) : new();
            PrepareNextBlock();
        }

        public override void Reset()
        {
            _hasCurrentBlock = false;
            PrepareNextBlock();
        }

        void PrepareNextBlock()
        {
            var next = Context.Data.NextBlock;
            next.Type = (TetrisBlockType)_random.Next(0, TetrisBlockData.TYPE_COUNT);
            next.Rotation = 0;
            next.Position = Vector2Int.zero;
        }

        public bool SpawnBlock()
        {
            var data = Context.Data;
            var board = Context.Board;
            var block = data.CurrentBlock;
            var next = data.NextBlock;

            block.Set(next.Type, TetrisBlockLogic.GetSpawnPosition(board.Width, board.Height), 0);
            PrepareNextBlock();

            var cells = block.GetCells();
            if (!board.CanPlace(cells, block.Position))
            {
                var upper = new Vector2Int(block.Position.x, block.Position.y + 1);
                if (board.CanPlace(cells, upper))
                {
                    block.Position = upper;
                }
                else
                {
                    _hasCurrentBlock = false;
                    return false;
                }
            }

            _hasCurrentBlock = true;
            RecalculateGhost();
            InvokeBlockChanged();
            return true;
        }

        public bool TryMove(int deltaX, int deltaY)
        {
            if (!_hasCurrentBlock)
            {
                return false;
            }
            var block = Context.Data.CurrentBlock;
            var board = Context.Board;
            var cells = block.GetCells();
            var next = new Vector2Int(block.Position.x + deltaX, block.Position.y + deltaY);
            if (!board.CanPlace(cells, next))
            {
                return false;
            }
            block.Position = next;
            if (deltaX != 0)
            {
                RecalculateGhost();
            }
            InvokeBlockChanged();
            return true;
        }

        public bool TrySoftDrop() => TryMove(0, -1);

        public int HardDrop()
        {
            if (!_hasCurrentBlock)
            {
                return 0;
            }
            var block = Context.Data.CurrentBlock;
            var board = Context.Board;
            var dropPosition = board.CalculateDropPosition(block.GetCells(), block.Position);
            var distance = block.Position.y - dropPosition.y;
            if (distance > 0)
            {
                block.Position = dropPosition;
                InvokeBlockChanged();
            }
            return distance;
        }

        public bool TryRotate(int direction)
        {
            if (!_hasCurrentBlock)
            {
                return false;
            }
            var block = Context.Data.CurrentBlock;
            var board = Context.Board;
            var nextRotation = TetrisBlockLogic.NextRotation(block.Rotation, direction);
            var nextCells = TetrisBlockData.GetCells(block.Type, nextRotation);

            var kicks = TetrisBlockLogic.KickOffsets;
            var kickCount = kicks.Length;
            for (var i = 0; i < kickCount; i++)
            {
                var position = new Vector2Int(block.Position.x + kicks[i].x, block.Position.y + kicks[i].y);
                if (board.CanPlace(nextCells, position))
                {
                    block.Rotation = nextRotation;
                    block.Position = position;
                    RecalculateGhost();
                    InvokeBlockChanged();
                    return true;
                }
            }
            return false;
        }

        public void LockBlock()
        {
            if (!_hasCurrentBlock)
            {
                return;
            }
            var block = Context.Data.CurrentBlock;
            var board = Context.Board;
            board.Place(block.GetCells(), block.Position, (int)block.Type + 1);
            _hasCurrentBlock = false;
            BlockLocked?.Invoke();
            InvokeBlockChanged();
        }

        void RecalculateGhost()
        {
            var data = Context.Data;
            var board = Context.Board;
            var block = data.CurrentBlock;
            data.CurrentGhostPosition = board.CalculateDropPosition(block.GetCells(), block.Position);
        }

        void InvokeBlockChanged()
        {
            try
            {
                BlockChanged?.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}
