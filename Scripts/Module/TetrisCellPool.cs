using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;

namespace Game.Features.Tetris
{
    public class TetrisCellPool : IDisposable
    {
        readonly TetrisBoardEntity _boardEntity;
        readonly TetrisBlockEntity _currentBlockEntity;
        readonly ObjectPool<SpriteRenderer> _pool;

        public TetrisCellPool(TetrisBoardEntity boardEntity, TetrisBlockEntity currentBlockEntity)
        {
            _boardEntity = boardEntity;
            _currentBlockEntity = currentBlockEntity;

            _pool = new(
                createFunc: CreateCell,
                actionOnGet: OnGetCell,
                actionOnRelease: OnReleaseCell,
                actionOnDestroy: OnDestroyCell,
                collectionCheck: false,
                defaultCapacity: 64,
                maxSize: 512
            );
        }

        public SpriteRenderer Get()
        {
            return _pool.Get();
        }

        public void Release(SpriteRenderer cell)
        {
            _pool.Release(cell);
        }

        public void Dispose()
        {
            _pool.Dispose();
        }

        SpriteRenderer CreateCell()
        {
            var template = _boardEntity != null ? _boardEntity.CellTemplate : null;
            if (template == null && _currentBlockEntity != null)
            {
                var cells = _currentBlockEntity.CellRenderers;
                if (cells != null && cells.Length > 0)
                {
                    template = cells[0];
                }
            }
            if (template == null)
            {
                Debug.LogError("[TetrisCellPool] 셀 템플릿을 찾을 수 없습니다.");
                return null;
            }

            var parent = _boardEntity != null ? _boardEntity.FixedCellRoot : null;
            var instance = UnityEngine.Object.Instantiate(template, parent);
            instance.gameObject.name = "Cell";
            instance.gameObject.SetActive(false);
            instance.transform.localScale = Vector3.one;
            return instance;
        }

        void OnGetCell(SpriteRenderer cell)
        {
            if (cell == null)
            {
                return;
            }
            cell.gameObject.SetActive(true);
        }

        void OnReleaseCell(SpriteRenderer cell)
        {
            if (cell == null)
            {
                return;
            }
            KillTweens(cell);
            cell.gameObject.SetActive(false);
            var color = cell.color;
            color.a = 1f;
            cell.color = color;
            cell.transform.localScale = Vector3.one;
        }

        void OnDestroyCell(SpriteRenderer cell)
        {
            if (cell == null)
            {
                return;
            }
            KillTweens(cell);
            UnityEngine.Object.Destroy(cell.gameObject);
        }

        static void KillTweens(SpriteRenderer cell)
        {
            cell.DOKill();
            cell.transform.DOKill();
        }
    }
}
