using Cysharp.Threading.Tasks;
using System;
using System.Threading;

// REF: [Cooperatively pausing async methods - .NET Blog](https://devblogs.microsoft.com/dotnet/cooperatively-pausing-async-methods/)

namespace NSequence
{
    /// <summary>
    /// 非同期処理のポーズ/レジューム機能を提供するトークンソース．
    /// </summary>
    public sealed class PausableTokenSource : IDisposable
    {
        // NOTE: この値がnullの場合，Pauseされていないことを示す
        private volatile UniTaskCompletionSource<bool> _paused;

        private bool _disposed;


        /// <summary>
        /// 現在ポーズ中かどうか．
        /// </summary>
        public bool IsPaused
        {
            get => _paused != null;
            set
            {
                ThrowIfDisposed();
                if (value)
                {
                    Interlocked.CompareExchange(ref _paused, new UniTaskCompletionSource<bool>(), null);
                }
                else
                {
                    while (true)
                    {
                        var tcs = _paused;
                        if (tcs == null) return;
                        if (Interlocked.CompareExchange(ref _paused, null, tcs) == tcs)
                        {
                            tcs.TrySetResult(true);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// このソースから生成されるPausableToken．
        /// </summary>
        public PausableToken Token => new (this);


        /// <summary>
        /// コンストラクタ．
        /// </summary>
        public PausableTokenSource()
        {
            _paused = new();
        }

        /// <summary>
        /// 解放処理．
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;

            // Pause中であれば解除する
            _paused?.TrySetResult(true);
            _paused = null;

            _disposed = true;
        }

        internal UniTask WaitWhilePaused()
        {
            var cur = _paused;
            return cur != null ? cur.Task : CompletedTask;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(PausableTokenSource));
        }


        internal static readonly UniTask CompletedTask = UniTask.FromResult(true);
    }
}
