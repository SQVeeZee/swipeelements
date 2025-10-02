using System.Threading;
using Cysharp.Threading.Tasks;

namespace Project.Core
{
    public interface ISceneModule
    {
        UniTask InitializeAsync(CancellationToken cancellationToken);
        void Dispose();
    }
}