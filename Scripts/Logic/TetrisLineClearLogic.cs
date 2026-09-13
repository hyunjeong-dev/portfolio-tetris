using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Game.Features.Tetris
{
    public static class TetrisLineClearLogic
    {
        public static async UniTask PlayAsync(SpriteRenderer[,] cells, int width, bool[] linesToClear, int playHeight, float duration, CancellationToken cancellationToken)
        {
            if (duration <= 0f || cells == null)
            {
                return;
            }

            var sequence = DOTween.Sequence();
            sequence.SetAutoKill(true);

            var hasTween = false;
            var flashDuration = duration * 0.35f;
            var clearDuration = duration * 0.65f;
            for (var y = 0; y < playHeight; y++)
            {
                if (!linesToClear[y])
                {
                    continue;
                }
                for (var x = 0; x < width; x++)
                {
                    var cell = cells[x, y];
                    if (cell == null)
                    {
                        continue;
                    }
                    AddCellTween(sequence, cell, flashDuration, clearDuration);
                    hasTween = true;
                }
            }

            if (!hasTween)
            {
                sequence.Kill();
                return;
            }

            await AwaitSequenceAsync(sequence, cancellationToken);
        }

        static void AddCellTween(Sequence sequence, SpriteRenderer cell, float flashDuration, float clearDuration)
        {
            cell.DOKill();
            cell.transform.DOKill();

            var baseColor = cell.color;
            var flashColor = Color.white;
            flashColor.a = baseColor.a;
            cell.transform.localScale = Vector3.one;

            sequence.Insert(0f, cell.DOColor(flashColor, flashDuration).SetEase(Ease.OutQuad));
            sequence.Insert(flashDuration, cell.DOFade(0f, clearDuration).SetEase(Ease.InQuad));
        }

        static async UniTask AwaitSequenceAsync(Sequence sequence, CancellationToken cancellationToken)
        {
            var completionSource = new UniTaskCompletionSource();
            sequence.OnComplete(() => completionSource.TrySetResult());
            sequence.OnKill(() =>
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    completionSource.TrySetCanceled(cancellationToken);
                }
                else
                {
                    completionSource.TrySetResult();
                }
            });

            using (cancellationToken.Register(() => sequence.Kill(false)))
            {
                await completionSource.Task;
            }
        }
    }
}
