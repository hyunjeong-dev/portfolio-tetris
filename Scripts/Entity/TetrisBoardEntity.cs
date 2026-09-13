using UnityEngine;

namespace Game.Features.Tetris
{
    public class TetrisBoardEntity : MonoBehaviour
    {
        [SerializeField] Transform _boardBackground;
        [SerializeField] Transform _fixedCellRoot;
        [SerializeField] Transform _currentBlockRoot;
        [SerializeField] Transform _ghostBlockRoot;
        [SerializeField] SpriteRenderer _cellTemplate;

        public Transform FixedCellRoot => _fixedCellRoot;
        public Transform CurrentBlockRoot => _currentBlockRoot;
        public Transform GhostBlockRoot => _ghostBlockRoot;
        public SpriteRenderer CellTemplate => _cellTemplate;

        public Vector3 CellToLocal(int x, int y, float cellSize)
        {
            return new Vector3(x * cellSize + cellSize * 0.5f, y * cellSize + cellSize * 0.5f, 0f);
        }

        public void FitBackground(int width, int height, float cellSize)
        {
            if (_boardBackground == null)
            {
                return;
            }
            _boardBackground.transform.localPosition = new Vector3(width * cellSize * 0.5f, height * cellSize * 0.5f, 0f);
        }
    }
}
