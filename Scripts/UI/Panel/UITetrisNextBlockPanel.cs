using UnityEngine;
using UnityEngine.UI;

namespace Game.Features.Tetris
{
    public class UITetrisNextBlockPanel : MonoBehaviour
    {
        [SerializeField] GameObject _root;
        [SerializeField] Image[] _cellImages = new Image[TetrisConstants.CURRENT_BLOCK_CELL_COUNT];
        [SerializeField] float _cellPixelSize = 40f;

        TetrisBlockType _lastType = (TetrisBlockType)(-1);
        int _lastRotation = int.MinValue;
        bool _hasBlock;

        public void UpdateView(TetrisBlock next)
        {
            if (next == null)
            {
                Hide();
                return;
            }

            if (_hasBlock && next.Type == _lastType && next.Rotation == _lastRotation)
            {
                return;
            }
            _lastType = next.Type;
            _lastRotation = next.Rotation;
            _hasBlock = true;

            Show();
            ApplyShape(next.GetCells(), next.GetColor());
        }

        void ApplyShape(Vector2Int[] cells, Color color)
        {
            var minX = int.MaxValue;
            var maxX = int.MinValue;
            var minY = int.MaxValue;
            var maxY = int.MinValue;
            foreach (var cell in cells)
            {
                if (cell.x < minX)
                {
                    minX = cell.x;
                }
                if (cell.x > maxX)
                {
                    maxX = cell.x;
                }
                if (cell.y < minY)
                {
                    minY = cell.y;
                }
                if (cell.y > maxY)
                {
                    maxY = cell.y;
                }
            }
            var centerX = (minX + maxX + 1) * 0.5f;
            var centerY = (minY + maxY + 1) * 0.5f;

            var imageCount = _cellImages.Length;
            for (var i = 0; i < imageCount; i++)
            {
                var image = _cellImages[i];
                if (image == null)
                {
                    continue;
                }
                if (i >= cells.Length)
                {
                    image.enabled = false;
                    continue;
                }
                image.enabled = true;
                image.color = color;
                var rectTransform = image.rectTransform;
                var cell = cells[i];
                rectTransform.anchoredPosition = new Vector2(
                    (cell.x + 0.5f - centerX) * _cellPixelSize,
                    (cell.y + 0.5f - centerY) * _cellPixelSize);
                rectTransform.sizeDelta = new Vector2(_cellPixelSize, _cellPixelSize);
            }
        }

        public void Show()
        {
            if (_root != null && !_root.activeSelf)
            {
                _root.SetActive(true);
            }
        }

        public void Hide()
        {
            if (_root != null && _root.activeSelf)
            {
                _root.SetActive(false);
            }
            _hasBlock = false;
        }
    }
}
