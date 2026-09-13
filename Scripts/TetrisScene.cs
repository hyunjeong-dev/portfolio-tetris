using UnityEngine;

namespace Game.Features.Tetris
{
    public class TetrisScene : MonoBehaviour
    {
        [SerializeField] TetrisContext _context;

        void Start()
        {
            if (_context == null)
            {
                _context = FindFirstObjectByType<TetrisContext>();
            }
            
            if (_context == null)
            {
                return;
            }
            
            _context.StartGame();
        }
    }
}
