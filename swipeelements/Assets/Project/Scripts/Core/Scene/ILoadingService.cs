using System.Threading;
using Cysharp.Threading.Tasks;

namespace Project.Core
{
    public interface ILoadingProcessor
    {
        UniTask ShowAsync(CancellationToken token);
        UniTask HideAsync(CancellationToken token);
        void SetProgress(float value);
    }
}