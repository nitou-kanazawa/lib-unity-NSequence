using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace NSequence
{
    /// <summary>
    /// アクション実行プロセス．
    /// </summary>
    public sealed class ActionProcess : IProcess
    {
        private readonly Func<CancellationToken,UniTask> _action;

        public ActionProcess(Func<CancellationToken, UniTask> action)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public async UniTask RunAsync(PausableToken pausableToken, CancellationToken cancellationToken)
        {
            await pausableToken.WaitWhilePaused(cancellationToken);
            await _action(cancellationToken);
        }
    }
}
