using UnityEngine;
using UnityEngine.UI;

namespace Game.Features.Tetris
{
    public class UITetrisMobileInputPanel : MonoBehaviour
    {
        [SerializeField] UITetrisHoldButton _leftButton;
        [SerializeField] UITetrisHoldButton _rightButton;
        [SerializeField] UITetrisHoldButton _softDropButton;
        [SerializeField] Button _rotateCwButton;
        [SerializeField] Button _hardDropButton;

        TetrisContext _context;

        public void Bind(TetrisContext context)
        {
            _context = context;
            if (_leftButton != null)
            {
                _leftButton.SetCallback(OnLeft);
            }
            if (_rightButton != null)
            {
                _rightButton.SetCallback(OnRight);
            }
            if (_softDropButton != null)
            {
                _softDropButton.SetCallback(OnSoftDrop);
            }
            if (_rotateCwButton != null)
            {
                _rotateCwButton.onClick.AddListener(OnRotateCw);
            }
            if (_hardDropButton != null)
            {
                _hardDropButton.onClick.AddListener(OnHardDrop);
            }
        }

        public void Unbind()
        {
            if (_leftButton != null)
            {
                _leftButton.SetCallback(null);
            }
            if (_rightButton != null)
            {
                _rightButton.SetCallback(null);
            }
            if (_softDropButton != null)
            {
                _softDropButton.SetCallback(null);
            }
            if (_rotateCwButton != null)
            {
                _rotateCwButton.onClick.RemoveListener(OnRotateCw);
            }
            if (_hardDropButton != null)
            {
                _hardDropButton.onClick.RemoveListener(OnHardDrop);
            }
            _context = null;
        }

        void OnLeft()
        {
            if (_context != null)
            {
                _context.Input.RequestMove(-1);
            }
        }

        void OnRight()
        {
            if (_context != null)
            {
                _context.Input.RequestMove(1);
            }
        }

        void OnSoftDrop()
        {
            if (_context != null)
            {
                _context.Input.RequestSoftDrop();
            }
        }

        void OnRotateCw()
        {
            if (_context != null)
            {
                _context.Input.RequestRotate(1);
            }
        }

        void OnRotateCcw()
        {
            if (_context != null)
            {
                _context.Input.RequestRotate(-1);
            }
        }

        void OnHardDrop()
        {
            if (_context != null)
            {
                _context.Input.RequestHardDrop();
            }
        }
    }
}
