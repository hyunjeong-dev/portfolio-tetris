using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Features.Tetris
{
    public class UITetrisHoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        [SerializeField] float _initialDelay = TetrisConstants.MOVE_REPEAT_INITIAL_DELAY;
        [SerializeField] float _repeatInterval = TetrisConstants.MOVE_REPEAT_INTERVAL;
        [SerializeField] bool _repeat = true;

        Action _onPressed;
        bool _held;
        float _next;

        public void SetCallback(Action onPressed) => _onPressed = onPressed;

        public void OnPointerDown(PointerEventData eventData)
        {
            _held = true;
            _onPressed?.Invoke();
            _next = _initialDelay;
        }

        public void OnPointerUp(PointerEventData eventData) => _held = false;
        public void OnPointerExit(PointerEventData eventData) => _held = false;

        void OnDisable() => _held = false;

        void Update()
        {
            if (!_held || !_repeat)
            {
                return;
            }
            _next -= Time.deltaTime;
            if (_next <= 0f)
            {
                _onPressed?.Invoke();
                _next = _repeatInterval;
            }
        }
    }
}
