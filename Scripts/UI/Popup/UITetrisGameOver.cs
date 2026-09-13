using UnityEngine;
using UnityEngine.UI;

namespace Game.Features.Tetris
{
    public class UITetrisGameOver : MonoBehaviour
    {
        [SerializeField] GameObject _root;
        [SerializeField] Text _scoreText;
        [SerializeField] Text _linesText;
        [SerializeField] Button _restartButton;
        [SerializeField] Button _quitButton;

        TetrisContext _context;

        public void Bind(TetrisContext context)
        {
            _context = context;
            if (_restartButton != null)
            {
                _restartButton.onClick.AddListener(OnRestart);
            }
            if (_quitButton != null)
            {
                _quitButton.onClick.AddListener(OnQuit);
            }
            Hide();
        }

        public void Unbind()
        {
            if (_restartButton != null)
            {
                _restartButton.onClick.RemoveListener(OnRestart);
            }
            if (_quitButton != null)
            {
                _quitButton.onClick.RemoveListener(OnQuit);
            }
            _context = null;
        }

        public void Show(TetrisContext context)
        {
            _context = context;
            if (_root != null)
            {
                _root.SetActive(true);
            }
            if (_scoreText != null)
            {
                _scoreText.text = "Score " + context.Data.Score.Score;
            }
            if (_linesText != null)
            {
                _linesText.text = "Lines " + context.Data.Score.Lines;
            }
        }

        public void Hide()
        {
            if (_root != null && _root.activeSelf)
            {
                _root.SetActive(false);
            }
        }

        void OnRestart()
        {
            if (_context != null)
            {
                _context.StartGame();
            }
        }

        void OnQuit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
