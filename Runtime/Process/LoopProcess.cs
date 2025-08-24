using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace NSequence
{
    /// <summary>
    /// ループ処理プロセス
    /// </summary>
    public sealed class LoopProcess : IProcess
    {
        private readonly IProcess _process;
        private readonly Func<bool> _condition;
        private readonly int _maxIterations;


        /// <summary>
        /// コンストラクタ．
        /// </summary>
        /// <param name="process"></param>
        /// <param name="whileCondition"></param>
        /// <param name="maxIterations"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public LoopProcess(IProcess process, Func<bool> whileCondition, int maxIterations = int.MaxValue)
        {
            _process = process ?? throw new ArgumentNullException(nameof(process));
            _condition = whileCondition ?? throw new ArgumentNullException(nameof(whileCondition));
            _maxIterations = maxIterations;
        }

        public async UniTask RunAsync(PausableToken pausableToken, CancellationToken cancellationToken)
        {
            int iterations = 0;

            while (_condition() && iterations < _maxIterations && !cancellationToken.IsCancellationRequested)
            {
                await pausableToken.WaitWhilePaused(cancellationToken);
                await _process.RunAsync(pausableToken, cancellationToken);
                iterations++;
            }
        }
    }
}
