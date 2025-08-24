using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace NSequence
{
    /// <summary>
    /// 条件分岐プロセス
    /// </summary>
    public sealed class ConditionalProcess : IProcess
    {
        private readonly Func<bool> _condition;
        private readonly IProcess _ifTrue;
        private readonly IProcess _ifFalse;


        /// <summary>
        /// コンストラクタ．
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="ifTrue"></param>
        /// <param name="ifFalse"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ConditionalProcess(Func<bool> condition, IProcess ifTrue, IProcess ifFalse = null)
        {
            _condition = condition ?? throw new ArgumentNullException(nameof(condition));
            _ifTrue = ifTrue ?? throw new ArgumentNullException(nameof(ifTrue));
            _ifFalse = ifFalse;
        }

        public async UniTask RunAsync(PausableToken pausableToken, CancellationToken cancellationToken)
        {
            await pausableToken.WaitWhilePaused(cancellationToken);

            if (_condition())
            {
                await _ifTrue.RunAsync(pausableToken, cancellationToken);
            }
            else if (_ifFalse != null)
            {
                await _ifFalse.RunAsync(pausableToken, cancellationToken);
            }
        }
    }
}
