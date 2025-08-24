using System;
using System.Threading;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace NSequence
{
    public sealed class Sequence : IProcess
    {
        private IEnumerable<IProcess> _processes;


        public Sequence(IEnumerable<IProcess> processes)
        {
            _processes = processes ?? throw new ArgumentNullException(nameof(processes));
        }

        public async UniTask RunAsync(PausableToken pausableToken, CancellationToken cancellationToken)
        {
            foreach(var process in _processes)
            {
                // ポーズ待機
                await pausableToken.WaitWhilePaused(cancellationToken);

                // 子プロセス
                await process.RunAsync(pausableToken, cancellationToken);
            }
        }
    }
}
