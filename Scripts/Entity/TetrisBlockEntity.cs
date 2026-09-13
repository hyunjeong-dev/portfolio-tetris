using UnityEngine;

namespace Game.Features.Tetris
{
    public class TetrisBlockEntity : MonoBehaviour
    {
        public SpriteRenderer[] CellRenderers => _cellRenderers;
        public int SortingOrder => _sortingOrder;

        [SerializeField] SpriteRenderer[] _cellRenderers = new SpriteRenderer[TetrisConstants.CURRENT_BLOCK_CELL_COUNT];
        [SerializeField] int _sortingOrder = 2;

        void Awake()
        {
            if (_cellRenderers != null)
            {
                foreach (var renderer in _cellRenderers)
                {
                    if (renderer != null)
                    {
                        renderer.sortingOrder = _sortingOrder;
                    }
                }
            }
        }

        public void SetColor(Color color)
        {
            if (_cellRenderers == null)
            {
                return;
            }
            foreach (var renderer in _cellRenderers)
            {
                if (renderer != null)
                {
                    renderer.color = color;
                }
            }
        }

        public void SetVisible(bool visible)
        {
            if (gameObject.activeSelf != visible)
            {
                gameObject.SetActive(visible);
            }
        }

        public void UpdateLocalPositions(Vector2Int[] cells, float cellSize)
        {
            if (_cellRenderers == null)
            {
                return;
            }
            var rendererCount = _cellRenderers.Length;
            for (var i = 0; i < rendererCount; i++)
            {
                var renderer = _cellRenderers[i];
                if (renderer == null)
                {
                    continue;
                }
                if (cells == null || i >= cells.Length)
                {
                    renderer.enabled = false;
                    continue;
                }
                renderer.enabled = true;
                var cell = cells[i];
                renderer.transform.localPosition = new Vector3(cell.x * cellSize + cellSize * 0.5f, cell.y * cellSize + cellSize * 0.5f, 0f);
                renderer.transform.localScale = Vector3.one;
            }
        }
    }
}
