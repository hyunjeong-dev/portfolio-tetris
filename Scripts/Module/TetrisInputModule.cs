using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace Game.Features.Tetris
{
    public class TetrisInputModule : TetrisModule
    {
        bool _enabled;

        float _leftHeld;
        float _rightHeld;
        float _downHeld;

        protected override void OnBegin()
        {
            _enabled = true;
        }

        protected override void OnEnd()
        {
            _enabled = false;
        }

        public void SetEnabled(bool enabled) => _enabled = enabled;

        public void Tick(float deltaTime)
        {
            if (!_enabled)
            {
                return;
            }
            ProcessKeyboardInput(deltaTime);
        }

        void ProcessKeyboardInput(float deltaTime)
        {
#if ENABLE_INPUT_SYSTEM
            var keyboard = Keyboard.current;
            if (keyboard != null)
            {
                ProcessHorizontalInput(keyboard.leftArrowKey.isPressed, -1, ref _leftHeld, deltaTime);
                ProcessHorizontalInput(keyboard.rightArrowKey.isPressed, 1, ref _rightHeld, deltaTime);
                ProcessSoftDropInput(keyboard.downArrowKey.isPressed, deltaTime);
                if (keyboard.upArrowKey.wasPressedThisFrame || keyboard.xKey.wasPressedThisFrame)
                {
                    RequestRotate(1);
                }
                if (keyboard.zKey.wasPressedThisFrame)
                {
                    RequestRotate(-1);
                }
                if (keyboard.spaceKey.wasPressedThisFrame)
                {
                    RequestHardDrop();
                }
                if (keyboard.pKey.wasPressedThisFrame || keyboard.escapeKey.wasPressedThisFrame)
                {
                    RequestPause();
                }
            }
#elif ENABLE_LEGACY_INPUT_MANAGER
            ProcessHorizontalInput(Input.GetKey(KeyCode.LeftArrow), -1, ref _leftHeld, deltaTime);
            ProcessHorizontalInput(Input.GetKey(KeyCode.RightArrow), 1, ref _rightHeld, deltaTime);
            ProcessSoftDropInput(Input.GetKey(KeyCode.DownArrow), deltaTime);
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.X))
            {
                RequestRotate(1);
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                RequestRotate(-1);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RequestHardDrop();
            }
            if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
            {
                RequestPause();
            }
#endif
        }

        void ProcessHorizontalInput(bool pressed, int direction, ref float heldTime, float deltaTime)
        {
            if (pressed)
            {
                if (heldTime <= 0f)
                {
                    RequestMove(direction);
                    heldTime = TetrisConstants.MOVE_REPEAT_INITIAL_DELAY;
                }
                else
                {
                    heldTime -= deltaTime;
                    if (heldTime <= 0f)
                    {
                        RequestMove(direction);
                        heldTime = TetrisConstants.MOVE_REPEAT_INTERVAL;
                    }
                }
            }
            else
            {
                heldTime = 0f;
            }
        }

        void ProcessSoftDropInput(bool pressed, float deltaTime)
        {
            if (pressed)
            {
                if (_downHeld <= 0f)
                {
                    RequestSoftDrop();
                    _downHeld = TetrisConstants.SOFT_DROP_REPEAT_INTERVAL;
                }
                else
                {
                    _downHeld -= deltaTime;
                    if (_downHeld <= 0f)
                    {
                        RequestSoftDrop();
                        _downHeld = TetrisConstants.SOFT_DROP_REPEAT_INTERVAL;
                    }
                }
            }
            else
            {
                _downHeld = 0f;
            }
        }


        public void RequestMove(int dx)
        {
            if (!_enabled)
            {
                return;
            }
            if (!Context.StateMachine.IsPlayable)
            {
                return;
            }
            Context.Block.TryMove(dx, 0);
        }

        public void RequestSoftDrop()
        {
            if (!_enabled)
            {
                return;
            }
            if (!Context.StateMachine.IsPlayable)
            {
                return;
            }
            if (Context.Block.TrySoftDrop())
            {
                Context.ResetFallTimer();
            }
        }

        public void RequestRotate(int direction)
        {
            if (!_enabled)
            {
                return;
            }
            if (!Context.StateMachine.IsPlayable)
            {
                return;
            }
            Context.Block.TryRotate(direction);
        }

        public void RequestHardDrop()
        {
            if (!_enabled)
            {
                return;
            }
            if (!Context.StateMachine.IsPlayable)
            {
                return;
            }
            Context.Block.HardDrop();
            Context.RequestImmediateLock();
        }

        public void RequestPause() => Context.TogglePause();
    }
}
