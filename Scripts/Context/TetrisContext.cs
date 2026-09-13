using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Features.Tetris
{
    public partial class TetrisContext : MonoBehaviour
    {
        [Serializable]
        public class TetrisSetting
        {
            [Header("Game Setting")]
            public TetrisGameSetting config;
            public int randomSeed = -1;

            [Header("World")]
            public TetrisBoardEntity boardEntity;
            public TetrisBlockEntity currentBlockEntity;
            public TetrisBlockEntity ghostBlockEntity;

            [Header("UI")]
            public UITetrisHud hud;
        }

        public TetrisStateMachine StateMachine { get; private set; }
        public TetrisGameSetting GameSetting => setting.config;
        public int RandomSeed => setting.randomSeed;
        public TetrisBoardEntity BoardEntity => setting.boardEntity;
        public TetrisBlockEntity CurrentBlockEntity => setting.currentBlockEntity;
        public TetrisBlockEntity GhostBlockEntity => setting.ghostBlockEntity;
        public UITetrisHud Hud => setting.hud;

        [SerializeField] TetrisSetting setting;

        CancellationTokenSource _runCts;
        float _fallTimer;
        bool _immediateLockRequested;

        void Awake()
        {
            StateMachine = new();
            if (setting == null || setting.config == null)
            {
                Debug.LogError("[TetrisContext] GameSetting 이 할당되지 않았습니다.");
                enabled = false;
                return;
            }
            InitializeModules();
        }

        void Update()
        {
            if (StateMachine.IsRunning && !StateMachine.Is(TetrisStatus.Paused))
            {
                Data.Score.ElapsedTime += Time.deltaTime;
            }
            
            Input.Tick(Time.deltaTime);
            
            UI.Tick();
        }

        void OnDestroy()
        {
            CancelRun();
            
            DisposeModules();
        }
        
        public void StartGame()
        {
            CancelRun();
            
            _runCts = CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy());
            
            ResetGameState();
            RunGameAsync(_runCts.Token).Forget();
        }

        public void TogglePause()
        {
            if (StateMachine.Is(TetrisStatus.Falling))
            {
                StateMachine.ChangeState(TetrisStatus.Paused);
            }
            else if (StateMachine.Is(TetrisStatus.Paused))
            {
                StateMachine.ChangeState(TetrisStatus.Falling);
            }
        }

        public void ResetFallTimer() => _fallTimer = 0f;
        public void RequestImmediateLock() => _immediateLockRequested = true;


        void ResetGameState()
        {
            ResetModules();
            _fallTimer = 0f;
            _immediateLockRequested = false;
        }

        async UniTaskVoid RunGameAsync(CancellationToken cancellationToken)
        {
            try
            {
                StateMachine.ChangeState(TetrisStatus.Ready);
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);

                while (!cancellationToken.IsCancellationRequested)
                {
                    StateMachine.ChangeState(TetrisStatus.Spawning);
                    if (!Block.SpawnBlock())
                    {
                        TriggerGameOver();
                        return;
                    }
                    StateMachine.ChangeState(TetrisStatus.Falling);
                    _fallTimer = 0f;
                    _immediateLockRequested = false;

                    if (!Block.IsOnGround)
                    {
                        await RunFallLoopAsync(cancellationToken);
                        if (cancellationToken.IsCancellationRequested)
                        {
                            return;
                        }
                    }

                    var lockedInSpawnBuffer = Block.IsTouchingSpawnBuffer;
                    Block.LockBlock();

                    var cleared = Board.MarkFullLines();
                    if (cleared > 0)
                    {
                        StateMachine.ChangeState(TetrisStatus.LineClearing);
                        await Object.PlayLineClearAsync(Board.Width, Board.ScratchClearedLines, Board.Height, cancellationToken);
                        if (cancellationToken.IsCancellationRequested)
                        {
                            return;
                        }

                        Board.CollapseMarkedLines();
                        Object.CollapseVisualCells(Board.ScratchClearedLines, Board.Height);
                        Data.AddLines(cleared);
                        Data.AddScore(TetrisScoreLogic.CalculateLineScore(cleared));
                    }
                    else if (lockedInSpawnBuffer)
                    {
                        TriggerGameOver();
                        return;
                    }
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        async UniTask RunFallLoopAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (_immediateLockRequested)
                {
                    return;
                }
                if (StateMachine.Is(TetrisStatus.Paused))
                {
                    await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
                    continue;
                }
                _fallTimer += Time.deltaTime;
                var interval = Data.CurrentDropInterval;
                if (_fallTimer >= interval)
                {
                    _fallTimer = 0f;
                    if (Block.IsOnGround)
                    {
                        return;
                    }
                    Block.TryMove(0, -1);
                }
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
            }
        }

        void TriggerGameOver()
        {
            StateMachine.ChangeState(TetrisStatus.GameOver);
        }

        void CancelRun()
        {
            if (_runCts != null)
            {
                _runCts.Cancel();
                _runCts.Dispose();
                _runCts = null;
            }
        }
    }
}
