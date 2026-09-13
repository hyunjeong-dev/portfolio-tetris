using UnityEngine;
using UnityEngine.UI;

namespace Game.Features.Tetris
{
    public class UITetrisHud : MonoBehaviour, ITetrisStateListener
    {
        [SerializeField] UITetrisScorePanel _scorePanel;
        [SerializeField] UITetrisNextBlockPanel _nextBlockPanel;
        [SerializeField] UITetrisMobileInputPanel _inputPanel;
        
        [SerializeField] UITetrisGameOver _uiGameOver;
        [SerializeField] Button _pauseButton;
        [SerializeField] GameObject _pausedIndicator;
        [SerializeField] RectTransform _safeAreaRoot;

        TetrisContext _context;
        bool _stateListenerRegistered;
        Rect _lastSafeArea;
        Vector2Int _lastScreenSize;

        RectTransform SafeAreaRoot => _safeAreaRoot != null ? _safeAreaRoot : transform as RectTransform;

        void Awake()
        {
            RefreshSafeArea(true);
        }

        void OnEnable()
        {
            RefreshSafeArea(true);
        }

        void OnDestroy()
        {
            UnregisterStateListener();
        }

        public void Bind(TetrisContext context)
        {
            _context = context;
            if (_inputPanel != null)
            {
                _inputPanel.Bind(context);
            }
            if (_uiGameOver != null)
            {
                _uiGameOver.Bind(context);
            }
            if (_pauseButton != null)
            {
                _pauseButton.onClick.AddListener(OnClickPause);
            }
            RegisterStateListener();
            HideGameOver();
            SetPausedIndicator(false);
        }

        public void Unbind()
        {
            UnregisterStateListener();
            if (_inputPanel != null)
            {
                _inputPanel.Unbind();
            }
            if (_uiGameOver != null)
            {
                _uiGameOver.Unbind();
            }
            if (_pauseButton != null)
            {
                _pauseButton.onClick.RemoveListener(OnClickPause);
            }
            _context = null;
        }

        void RegisterStateListener()
        {
            if (_stateListenerRegistered || _context == null)
            {
                return;
            }
            _context.StateMachine.AddListener(this);
            _stateListenerRegistered = true;
        }

        void UnregisterStateListener()
        {
            if (!_stateListenerRegistered)
            {
                return;
            }
            if (_context != null)
            {
                _context.StateMachine.RemoveListener(this);
            }
            _stateListenerRegistered = false;
        }

        public void Refresh(TetrisContext context)
        {
            RefreshSafeArea(false);

            if (_scorePanel != null)
            {
                _scorePanel.UpdateView(context.Data.Score);
            }
            if (_nextBlockPanel != null)
            {
                _nextBlockPanel.UpdateView(context.Data.NextBlock);
            }
        }


        public void OnStateChanged(TetrisStatus next)
        {
            SetPausedIndicator(next == TetrisStatus.Paused);

            if (next == TetrisStatus.GameOver)
            {
                if (_uiGameOver != null && _context != null)
                {
                    _uiGameOver.Show(_context);
                }
            }
            else
            {
                HideGameOver();
            }
        }
        
        void HideGameOver()
        {
            if (_uiGameOver != null)
            {
                _uiGameOver.Hide();
            }
        }

        void SetPausedIndicator(bool paused)
        {
            if (_pausedIndicator != null && _pausedIndicator.activeSelf != paused)
            {
                _pausedIndicator.SetActive(paused);
            }
        }

        void RefreshSafeArea(bool force)
        {
            var screenSize = new Vector2Int(Screen.width, Screen.height);
            var safeArea = Screen.safeArea;
            if (!force && screenSize == _lastScreenSize && safeArea == _lastSafeArea)
            {
                return;
            }

            ApplySafeArea(screenSize, safeArea);
        }

        void ApplySafeArea(Vector2Int screenSize, Rect safeArea)
        {
            if (screenSize.x <= 0 || screenSize.y <= 0)
            {
                return;
            }

            var safeAreaRoot = SafeAreaRoot;
            if (safeAreaRoot == null)
            {
                return;
            }

            var anchorMin = safeArea.position;
            var anchorMax = safeArea.position + safeArea.size;
            anchorMin.x = Mathf.Clamp01(anchorMin.x / screenSize.x);
            anchorMin.y = Mathf.Clamp01(anchorMin.y / screenSize.y);
            anchorMax.x = Mathf.Clamp01(anchorMax.x / screenSize.x);
            anchorMax.y = Mathf.Clamp01(anchorMax.y / screenSize.y);

            safeAreaRoot.anchorMin = anchorMin;
            safeAreaRoot.anchorMax = anchorMax;
            safeAreaRoot.offsetMin = Vector2.zero;
            safeAreaRoot.offsetMax = Vector2.zero;

            _lastScreenSize = screenSize;
            _lastSafeArea = safeArea;
        }

        void OnClickPause()
        {
            if (_context != null)
            {
                _context.Input.RequestPause();
            }
        }
    }
}
