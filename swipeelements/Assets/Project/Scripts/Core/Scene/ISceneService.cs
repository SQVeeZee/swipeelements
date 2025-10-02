using System.Threading;
using Cysharp.Threading.Tasks;

namespace Project.Core
{
    public interface ISceneService
    {
        UniTask LoadAsync(string sceneName, CancellationToken token);
        UniTask ReloadAsync(CancellationToken token);

        UniTask LoadAdditiveAsync(string sceneName, bool setActive, CancellationToken token);
        UniTask UnloadAsync(string sceneName, CancellationToken token = default);

        string Current { get; }
        string Previous { get; }
    }
}