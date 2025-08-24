using Cysharp.Threading.Tasks;
using System.Threading;

namespace NSequence
{
    public interface IProcess
    {
        UniTask RunAsync(PausableToken pausableToken, CancellationToken cancellationToken = default);
    }
}
