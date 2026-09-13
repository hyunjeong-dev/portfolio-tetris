using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Features.Tetris
{
    public class TetrisStateMachine
    {
        public TetrisStatus Current { get; private set; } = TetrisStatus.None;

        public bool IsPlayable => Current == TetrisStatus.Falling;
        public bool IsRunning => Current != TetrisStatus.None && Current != TetrisStatus.GameOver;
        public bool IsBusy => Current == TetrisStatus.LineClearing || Current == TetrisStatus.Spawning;

        readonly List<ITetrisStateListener> _listeners = new(8);
        readonly List<ITetrisStateListener> _dispatchBuffer = new(8);

        public bool Is(TetrisStatus status) => Current == status;

        public void AddListener(ITetrisStateListener listener)
        {
            if (listener == null)
            {
                return;
            }
            var listenerCount = _listeners.Count;
            for (var i = 0; i < listenerCount; i++)
            {
                if (ReferenceEquals(_listeners[i], listener))
                {
                    return;
                }
            }
            _listeners.Add(listener);
        }

        public void RemoveListener(ITetrisStateListener listener)
        {
            if (listener == null)
            {
                return;
            }
            var listenerCount = _listeners.Count;
            for (var i = 0; i < listenerCount; i++)
            {
                if (ReferenceEquals(_listeners[i], listener))
                {
                    _listeners.RemoveAt(i);
                    return;
                }
            }
        }

        public void ChangeState(TetrisStatus next)
        {
            if (Current == next)
            {
                return;
            }
            Current = next;

            _dispatchBuffer.Clear();
            var listenerCount = _listeners.Count;
            for (var i = 0; i < listenerCount; i++)
            {
                _dispatchBuffer.Add(_listeners[i]);
            }

            foreach (var listener in _dispatchBuffer)
            {
                try
                {
                    listener.OnStateChanged(Current);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }
    }
}
