using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace NSequence
{
    /// <summary>
    /// 遅延処理プロセス．
    /// </summary>
    public sealed class DelayProcess : IProcess
    {
        private readonly float _seconds;


        /// <summary>
        /// コンストラクタ．
        /// </summary>
        /// <param name="seconds"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public DelayProcess(float seconds)
        {
            if (seconds < 0f) throw new ArgumentOutOfRangeException(nameof(seconds), "DelayProcess requires non-negative seconds.");
            _seconds = seconds;
        }

        public async UniTask RunAsync(PausableToken pausableToken, CancellationToken cancellationToken)
        {
            var endTime = Time.time + _seconds;

            while (Time.time < endTime && !cancellationToken.IsCancellationRequested)
            {
                await pausableToken.WaitWhilePaused(cancellationToken);
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
            }
        }
    }
}
