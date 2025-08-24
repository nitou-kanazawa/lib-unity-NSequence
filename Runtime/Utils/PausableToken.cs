using System.Threading;
using Cysharp.Threading.Tasks;  

namespace NSequence
{
    /// <summary>
    /// ポーズ可能な処理で使用するトークン．
    /// </summary>
    public readonly struct PausableToken
    {
        private readonly PausableTokenSource _source;

        /// <summary>
        /// 現在ポーズ中かどうか．
        /// </summary>
        public bool IsPaused => _source?.IsPaused ?? false;


        internal PausableToken(PausableTokenSource source)
        {
            _source = source;
        }

        /// <summary>
        /// ポーズ中の間待機する．
        /// </summary>
        public UniTask WaitWhilePaused()
        {
            return IsPaused ? _source.WaitWhilePaused() : PausableTokenSource.CompletedTask;
        }

        /// <summary>
        /// ポーズ中の間待機する（キャンセル可能）．
        /// </summary>
        public async UniTask WaitWhilePaused(CancellationToken cancellationToken)
        {
            while (IsPaused && !cancellationToken.IsCancellationRequested)
            {
                await _source.WaitWhilePaused();
                cancellationToken.ThrowIfCancellationRequested();
            }
        }
    }
}
